using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace OpravaDiskuRP
{
    public partial class Form1 : Form
    {
        // Statistiky
        long celkovaVelikostVsech = 0;
        long velikostVybranych = 0;
        int pocetVybranych = 0;

        // Promenna pro stop
        bool zastavit = false;

        //Cesta k logu směřuje přímo na Plochu
        string logSoubor = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log_cisteni.txt");

        public Form1()
        {
            InitializeComponent();
            InitializeModernUi();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK) txtFolderPath.Text = fbd.SelectedPath;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            // Tlacitko STOP
            if (btnScan.Text == "STOP")
            {
                zastavit = true;
                return;
            }

            if (!Directory.Exists(txtFolderPath.Text)) { MessageBox.Show("Chybná složka!"); return; }

            if (chkMazat.Checked && MessageBox.Show("Smazat soubory a prázdné složky?", "Pozor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;


            lvResults.Items.Clear();
            celkovaVelikostVsech = 0;
            velikostVybranych = 0;
            pocetVybranych = 0;

            // Nastaveni pro start
            zastavit = false;
            btnScan.Text = "STOP";
            btnScan.BackColor = Color.Red;
            lblInfo.Text = "Pracuji...";

            // Zápis hlavičky do logu na plochu
            File.AppendAllText(logSoubor, "\n--- START SKENOVÁNÍ " + DateTime.Now + " ---\n");
            File.AppendAllText(logSoubor, "Prohledávaná složka: " + txtFolderPath.Text + "\n");

            try
            {
                Scanuj(txtFolderPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
                File.AppendAllText(logSoubor, "KRITICKÁ CHYBA: " + ex.Message + "\n");
            }

            // Nastaveni zpet po dokonceni
            btnScan.Text = "SPUSTIT";
            btnScan.BackColor = Color.Blue;

            if (zastavit) MessageBox.Show("Přerušeno.");
            else MessageBox.Show("Hotovo. Detailní log byl uložen na plochu.");

            File.AppendAllText(logSoubor, "--- KONEC SKENOVÁNÍ ---\n");
        }

        void Scanuj(string slozka)
        {
            if (zastavit) return;

            // Aby okno nezamrzlo
            Application.DoEvents();

            string[] podslozky = null;
            try
            {
                podslozky = Directory.GetDirectories(slozka);
            }
            catch (UnauthorizedAccessException)
            {
                // Program nespadne, jen si do logu poznamená, že sem nesmí
                File.AppendAllText(logSoubor, "[ODEPŘENO] Nemáte práva ke složce: " + slozka + "\n");
                return;
            }
            catch { return; }

            foreach (var podslozka in podslozky)
            {
                if (zastavit) return;
                Scanuj(podslozka);
            }

            // Osetreni prav pro soubory
            string[] soubory = null;
            try
            {
                soubory = Directory.GetFiles(slozka);
            }
            catch (UnauthorizedAccessException)
            {
                File.AppendAllText(logSoubor, "[ODEPŘENO] Nemáte práva číst soubory ve složce: " + slozka + "\n");
                return;
            }
            catch { return; }

            // Filtry
            foreach (var soubor in soubory)
            {
                if (zastavit) return;

                FileInfo info = new FileInfo(soubor);
                celkovaVelikostVsech += info.Length;

                string hledanyText = txtMaska.Text.ToLower();
                string jmenoSouboru = info.Name.ToLower();

                if (hledanyText.Length > 0 && !jmenoSouboru.Contains(hledanyText))
                    continue;

                if (info.Length < numVelikost.Value * 1024 * 1024) continue;

                
                // Přeskočí všechny soubory, které jsou novější než datum vybrané uživatelem
                if (info.LastWriteTime.Date > dtpDatum.Value.Date) continue;

                string stav = "Nalezeno";

                // Mazání
                if (chkMazat.Checked)
                {
                    try
                    {
                        File.Delete(soubor);
                        stav = "SMAZÁNO";
                    }
                    catch (Exception ex)
                    {
                        stav = "Chyba smazání";
                        // Psani do logu proc se soubor nesmazal
                        File.AppendAllText(logSoubor, "[CHYBA MAZÁNÍ] " + soubor + " - " + ex.Message + "\n");
                    }
                }

                velikostVybranych += info.Length;
                pocetVybranych++;

                ListViewItem item = new ListViewItem(info.Name);
                item.SubItems.Add(stav);
                item.SubItems.Add((info.Length / 1024.0 / 1024.0).ToString("F2") + " MB");
                item.SubItems.Add(info.DirectoryName);

                if (stav == "SMAZÁNO") item.ForeColor = Color.Red;

                lvResults.Items.Add(item);
                // posuneme lv, aby byl videt konec
                if (lvResults.Items.Count % 10 == 0)
                    lvResults.EnsureVisible(lvResults.Items.Count - 1);

                // Zápis nalezeného/smazaného souboru do logu na plochu
                File.AppendAllText(logSoubor, stav + ": " + soubor + " (Změněno: " + info.LastWriteTime.ToShortDateString() + ")\n");

                // Aktualizace statistik v reálném čase
                lblInfo.Text = $"Celkem prohledáno: {(celkovaVelikostVsech / 1024 / 1024)} MB | Nalezeno k akci: {pocetVybranych} soub. ({(velikostVybranych / 1024.0 / 1024.0):F2} MB)";
                Application.DoEvents();
            }

            // Mazání prázdné složky
            if (chkMazat.Checked && slozka != txtFolderPath.Text)
            {
                try
                {
                    if (Directory.GetFiles(slozka).Length == 0 && Directory.GetDirectories(slozka).Length == 0)
                    {
                        Directory.Delete(slozka);
                        File.AppendAllText(logSoubor, "[SMAZÁNO] Prázdná složka: " + slozka + "\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logSoubor, "[CHYBA] Nelze smazat složku: " + slozka + " - " + ex.Message + "\n");
                }
            }
        }
    }
}