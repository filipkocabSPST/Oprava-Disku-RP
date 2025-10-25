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
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            string rootPath = txtFolderPath.Text;
            if (string.IsNullOrEmpty(rootPath) || !Directory.Exists(rootPath))
            {
                MessageBox.Show("Vyberte platnou složku k prohledání.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            lvResults.Items.Clear();

            try
            {
                ScanDirectory(rootPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k neočekávané chybě: {ex.Message}", "Kritická chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScanDirectory(string path)
        {
            try
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    
                    AddFileToListView(fileInfo);
                }

                foreach (string dirPath in Directory.GetDirectories(path))
                {
                    ScanDirectory(dirPath); 
                }
            }
            catch (UnauthorizedAccessException)
            {
                
            }
        }

        
        private void AddFileToListView(FileInfo fileInfo)
        {
           
            ListViewItem item = new ListViewItem(fileInfo.Name);

          
            item.SubItems.Add(fileInfo.DirectoryName);
            item.SubItems.Add(FormatFileSize(fileInfo.Length));

           
            lvResults.Items.Add(item);
        }

        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double dblSByte = bytes;
            while (dblSByte >= 1024 && i < suffixes.Length - 1)
            {
                dblSByte /= 1024;
                i++;
            }
            return String.Format("{0:0.##} {1}", dblSByte, suffixes[i]);
        }
    }
}