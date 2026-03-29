using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpravaDiskuRP
{
    public partial class Form1 : Form
    {
        // --- Definice prvků ---
        public TextBox txtFolderPath, txtMaska;
        public Button btnSelectFolder, btnScan;
        public ListView lvResults;
        public NumericUpDown numVelikost;
        public DateTimePicker dtpDatum;
        public CheckBox chkMazat;
        public Label lblInfo;

        // DLL importy pro posouvání okna (bez nutnosti složité logiky)
        [DllImport("user32.dll")] private extern static void ReleaseCapture();
        [DllImport("user32.dll")] private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void InitializeModernUi()
        {
            // 1. Nastavení hlavního okna
            this.FormBorderStyle = FormBorderStyle.None;         // Skrytí standardního rámečku Windows
            this.BackColor = Color.FromArgb(45, 45, 48);          // Tmavé pozadí aplikace
            this.ClientSize = new Size(850, 650);                // Nastavení fixní velikosti okna
            this.StartPosition = FormStartPosition.CenterScreen; // Otevření okna na středu obrazovky

            // 2. Cesta k souborům (vstupní pole)
            txtFolderPath = new TextBox();
            txtFolderPath.Location = new Point(20, 75);          // Pozice na formuláři
            txtFolderPath.Size = new Size(400, 28);              // Velikost pole
            txtFolderPath.BackColor = Color.FromArgb(63, 63, 70);// Barva pozadí pole
            txtFolderPath.ForeColor = Color.Gainsboro;           // Barva textu
            txtFolderPath.BorderStyle = BorderStyle.None;        // Moderní vzhled bez rámečku

            btnSelectFolder = new Button();
            btnSelectFolder.Location = new Point(430, 75);       // Pozice tlačítka
            btnSelectFolder.Size = new Size(80, 28);             // Velikost tlačítka
            btnSelectFolder.Text = "Vybrat";                     // Text na tlačítku
            btnSelectFolder.BackColor = Color.FromArgb(63, 63, 70);
            btnSelectFolder.ForeColor = Color.White;
            btnSelectFolder.FlatStyle = FlatStyle.Flat;          // Plochý vzhled
            btnSelectFolder.Click += btnSelectFolder_Click;      // Propojení s metodou pro výběr složky

            // 3. Filtry (Maska, Velikost, Datum)
            // Maska souborů
            Label lblMaska = new Label { Text = "Maska:", Location = new Point(20, 100), ForeColor = Color.Gainsboro, AutoSize = true };
            txtMaska = new TextBox();
            txtMaska.Location = new Point(20, 118);
            txtMaska.Width = 70;
            txtMaska.BackColor = Color.FromArgb(63, 63, 70);
            txtMaska.ForeColor = Color.Gainsboro;

            // Minimální velikost v MB
            Label lblVelikost = new Label { Text = "Min. MB:", Location = new Point(110, 100), ForeColor = Color.Gainsboro, AutoSize = true };
            numVelikost = new NumericUpDown();
            numVelikost.Location = new Point(110, 118);
            numVelikost.Width = 70;
            numVelikost.Maximum = 100000;                        // Limit 100 GB
            numVelikost.BackColor = Color.FromArgb(63, 63, 70);
            numVelikost.ForeColor = Color.Gainsboro;

            // Datum poslední změny (filtr stáří)
            Label lblDatum = new Label { Text = "Starší než:", Location = new Point(200, 100), ForeColor = Color.Gainsboro, AutoSize = true };
            dtpDatum = new DateTimePicker();
            dtpDatum.Location = new Point(200, 118);
            dtpDatum.Width = 120;
            dtpDatum.Format = DateTimePickerFormat.Short;        // Zobrazí formát DD.MM.RRRR
            dtpDatum.Value = DateTime.Now.AddDays(-1);           // Výchozí hodnota je včerejšek

            // 4. Ovládací prvky a výsledky
            chkMazat = new CheckBox();
            chkMazat.Text = "Smazat nalezené";
            chkMazat.Location = new Point(340, 120);
            chkMazat.ForeColor = Color.FromArgb(200, 60, 60);    // Červený text varuje, že jde o mazání
            chkMazat.AutoSize = true;
            chkMazat.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            btnScan = new Button();
            btnScan.Location = new Point(680, 110);
            btnScan.Size = new Size(140, 35);
            btnScan.Text = "SPUSTIT";
            btnScan.BackColor = Color.FromArgb(0, 122, 204);     // Modrá barva pro hlavní akci
            btnScan.ForeColor = Color.White;
            btnScan.FlatStyle = FlatStyle.Flat;
            btnScan.Click += btnScan_Click;

            lblInfo = new Label();
            lblInfo.Text = "Připraveno...";
            lblInfo.Location = new Point(20, 160);
            lblInfo.ForeColor = Color.FromArgb(0, 122, 204);     // Akcentová modrá barva
            lblInfo.AutoSize = true;

            // Tabulka pro zobrazení výsledků
            lvResults = new ListView();
            lvResults.Location = new Point(20, 190);
            lvResults.Size = new Size(800, 440);
            lvResults.BackColor = Color.FromArgb(63, 63, 70);
            lvResults.ForeColor = Color.Gainsboro;
            lvResults.View = View.Details;                       // Režim zobrazení sloupců
            lvResults.FullRowSelect = true;                      // Vybere celý řádek najednou
            lvResults.BorderStyle = BorderStyle.None;
            lvResults.Columns.Add("Soubor", 200);                // Definice sloupců tabulky
            lvResults.Columns.Add("Stav", 100);
            lvResults.Columns.Add("Velikost", 100);
            lvResults.Columns.Add("Cesta", 350);

            // 5. Přidání všech prvků do okna
            this.Controls.AddRange(new Control[] {
            txtFolderPath, btnSelectFolder, lblMaska, txtMaska, lblVelikost,
            numVelikost, lblDatum, dtpDatum, chkMazat, btnScan, lvResults, lblInfo
        });

            // 6. Horní lišta pro posouvání okna
            Panel pnlTitle = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = Color.FromArgb(0, 122, 204) };
            pnlTitle.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };

            Button btnClose = new Button { Text = "✕", Dock = DockStyle.Right, Width = 45, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnClose.Click += (s, e) => this.Close(); // Ukončení aplikace

            pnlTitle.Controls.Add(btnClose);
            this.Controls.Add(pnlTitle);
        }
    }
}