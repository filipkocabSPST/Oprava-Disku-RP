using System;
using System.IO;
using System.Windows.Forms;

namespace OpravaDiskuRP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeModernUi();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialogSlozky = new FolderBrowserDialog())
            {
                if (dialogSlozky.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = dialogSlozky.SelectedPath;
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            string korenovaCesta = txtFolderPath.Text;
            
            if (string.IsNullOrEmpty(korenovaCesta) || !Directory.Exists(korenovaCesta))
            {
                MessageBox.Show("Vyberte platnou složku k prohledání");
                return;
            }

            lvResults.Items.Clear();

            try
            {
                ProhledejAdresar(korenovaCesta);
            }
            catch (Exception chyba)
            {
                MessageBox.Show($"Došlo k neočekávané chybě: {chyba.Message}");
            }
        }

        private void ProhledejAdresar(string cesta)
        {
            try
            {
                // Directory.EnumerateFiles prohleda slozku a diky SearchOption.AllDirectories 
                // automaticky projde i vsechny podslozky te slozky.
                foreach (string cestaKSouboru in Directory.EnumerateFiles(cesta, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        FileInfo infoOSouboru = new FileInfo(cestaKSouboru);
                        PridejSouborDoSeznamu(infoOSouboru);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("pristup do slozky " + cesta + " nebyl povolen");
            }
        }


        private void PridejSouborDoSeznamu(FileInfo infoOSouboru)
        {
            ListViewItem polozka = new ListViewItem(infoOSouboru.Name);

            polozka.SubItems.Add(infoOSouboru.DirectoryName);
            polozka.SubItems.Add(ZformatujVelikostSouboru(infoOSouboru.Length));

            lvResults.Items.Add(polozka);
        }

        private string ZformatujVelikostSouboru(long bajty)
        {
            string[] pripony = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double velikostVDouble = bajty;
            while (velikostVDouble >= 1024 && i < pripony.Length - 1)
            {
                velikostVDouble /= 1024;
                i++;
            }
            return String.Format("{0:0.##} {1}", velikostVDouble, pripony[i]);
        }
    }
}