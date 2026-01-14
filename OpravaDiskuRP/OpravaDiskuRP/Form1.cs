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

        string logSoubor = "log.txt";

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
         
            if (!Directory.Exists(txtFolderPath.Text)) { MessageBox.Show("Chybná složka!"); return; }

            if (chkMazat.Checked && MessageBox.Show("Smazat soubory a prázdné složky?", "Pozor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

          
            lvResults.Items.Clear();
            celkovaVelikostVsech = 0;
            velikostVybranych = 0;
            pocetVybranych = 0;

            
            btnScan.Enabled = false;
            btnScan.Text = "PRACUJI...";
            btnScan.BackColor = Color.Gray;
            lblInfo.Text = "Pracuji...";

            File.AppendAllText(logSoubor, "--- START " + DateTime.Now + " ---\n");

            try
            {
                
                Scanuj(txtFolderPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }

            
            btnScan.Enabled = true;
            btnScan.Text = "SPUSTIT";
            btnScan.BackColor = Color.Blue;

            MessageBox.Show("Hotovo.");
            File.AppendAllText(logSoubor, "--- KONEC ---\n");
        }

        void Scanuj(string slozka)
        {
            // Aby okno nezamrzlo
            Application.DoEvents();

            try
            {
              
                foreach (var podslozka in Directory.GetDirectories(slozka))
                {
                    Scanuj(podslozka);
                }

                // Filtry
                foreach (var soubor in Directory.GetFiles(slozka))
                {
                    FileInfo info = new FileInfo(soubor);
                    celkovaVelikostVsech += info.Length;

                    string hledanyText = txtMaska.Text.ToLower().Replace("*", "");
                    string jmenoSouboru = info.Name.ToLower();

                    if (hledanyText.Length > 0 && !jmenoSouboru.Contains(hledanyText))
                        continue;

               
                    if (info.Length < numVelikost.Value * 1024 * 1024) continue;

             
                    if ((DateTime.Now - info.LastWriteTime).Days < numDny.Value) continue;

                    string stav = "Nalezeno";

                    // Mazání
                    if (chkMazat.Checked)
                    {
                        try
                        {
                            File.Delete(soubor);
                            stav = "SMAZÁNO";
                        }
                        catch
                        {
                            stav = "Chyba smazání";
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
                    //posuneme lv, aby byl videt konec
                    if (lvResults.Items.Count % 10 == 0)
                        lvResults.EnsureVisible(lvResults.Items.Count - 1);

                    File.AppendAllText(logSoubor, stav + ": " + soubor + "\n");

                    // Aktualizace statistik v reálném čase
                    lblInfo.Text = $"Celkem prohledáno: {(celkovaVelikostVsech / 1024 / 1024)} MB | Nalezeno k akci: {pocetVybranych} soub. ({(velikostVybranych / 1024.0 / 1024.0):F2} MB)";
                    Application.DoEvents();
                }

                // Mazání prázdné složky
                if (chkMazat.Checked && slozka != txtFolderPath.Text)
                {
                    if (Directory.GetFiles(slozka).Length == 0 && Directory.GetDirectories(slozka).Length == 0)
                    {
                        try
                        {
                            Directory.Delete(slozka);
                            File.AppendAllText(logSoubor, "Smazána prázdná složka: " + slozka + "\n");
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }
    }
}