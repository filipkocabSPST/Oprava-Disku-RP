using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpravaDiskuRP
{
    public partial class Form1 : Form
    {
        // --- Barvy ---
        private readonly Color darkBackground = Color.FromArgb(45, 45, 48);
        private readonly Color controlBackground = Color.FromArgb(63, 63, 70);
        private readonly Color accentColor = Color.FromArgb(0, 122, 204);
        private readonly Color textColor = Color.Gainsboro;
        private readonly Color errorColor = Color.FromArgb(200, 60, 60);

        // --- Definice prvků ---
        public TextBox txtFolderPath;
        public Button btnSelectFolder;
        public Button btnScan;
        public ListView lvResults;
        public Label lblFolderPath;
        public Panel pnlPathBorder;

        // Prvky filtry
        public TextBox txtMaska;
        public NumericUpDown numVelikost;
        public NumericUpDown numDny;
        public CheckBox chkMazat;
        public Label lblInfo;

        private void InitializeModernUi()
        {
            // 1. Vytvoření prvků
            txtFolderPath = new TextBox();

            btnSelectFolder = new Button();
            // TADY Byla chyba - musíme tlačítko propojit s funkcí ve Form1.cs
            btnSelectFolder.Click += btnSelectFolder_Click;

            btnScan = new Button();
            // TADY Byla chyba - propojení tlačítka Start
            btnScan.Click += btnScan_Click;

            lvResults = new ListView();
            lblFolderPath = new Label();
            pnlPathBorder = new Panel();

            txtMaska = new TextBox();
            numVelikost = new NumericUpDown();
            numDny = new NumericUpDown();
            chkMazat = new CheckBox();
            lblInfo = new Label();

            // 2. Nastavení vzhledu
            ConfigureWindow();
            CreateCustomTitleBar();
            StyleSpecificControls();

            // 3. Přidání prvků na formulář
            this.Controls.Add(lblFolderPath);
            this.Controls.Add(pnlPathBorder);
            this.Controls.Add(btnSelectFolder);

            this.Controls.Add(CreateLabel("Maska (*.tmp):", 20, 120));
            this.Controls.Add(txtMaska);
            this.Controls.Add(CreateLabel("Min. MB:", 210, 120));
            this.Controls.Add(numVelikost);
            this.Controls.Add(CreateLabel("Starší (dny):", 350, 120));
            this.Controls.Add(numDny);
            this.Controls.Add(chkMazat);
            this.Controls.Add(btnScan);

            this.Controls.Add(lblInfo);
            this.Controls.Add(lvResults);
        }

        private void ConfigureWindow()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = darkBackground;
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(850, 650);
            this.Padding = new Padding(2);
            this.DoubleBuffered = true;
        }

        private void CreateCustomTitleBar()
        {
            Panel pnlTitleBar = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = accentColor };
            Label lblTitle = new Label { Text = "Čištění Disku", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true };

            Button btnClose = CreateTitleBarButton("✕", DockStyle.Right);
            btnClose.Click += (s, e) => this.Close();

            Button btnMinimize = CreateTitleBarButton("—", DockStyle.Right);
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            pnlTitleBar.Controls.Add(lblTitle);
            pnlTitleBar.Controls.Add(btnMinimize);
            pnlTitleBar.Controls.Add(btnClose);
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            this.Controls.Add(pnlTitleBar);
        }

        private Button CreateTitleBarButton(string text, DockStyle dock)
        {
            return new Button { Text = text, ForeColor = Color.White, Dock = dock, Width = 45, FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 } };
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), ForeColor = textColor, AutoSize = true };
        }

        private void StyleSpecificControls()
        {
            // Cesta
            lblFolderPath.Text = "Cesta:";
            lblFolderPath.ForeColor = textColor;
            lblFolderPath.Location = new Point(20, 50);

            pnlPathBorder.Location = new Point(20, 75);
            pnlPathBorder.Size = new Size(400, 28);
            pnlPathBorder.BackColor = Color.Gray;
            pnlPathBorder.Controls.Add(txtFolderPath);

            txtFolderPath.Dock = DockStyle.Fill;
            txtFolderPath.BackColor = controlBackground;
            txtFolderPath.ForeColor = textColor;
            txtFolderPath.BorderStyle = BorderStyle.None;

            btnSelectFolder.Location = new Point(430, 75);
            btnSelectFolder.Text = "Vybrat";
            btnSelectFolder.Size = new Size(80, 28);
            ApplyButtonStyle(btnSelectFolder);

            // Filtry
            txtMaska.Location = new Point(110, 118);
            txtMaska.Width = 80;
            txtMaska.Text = "*.*";
            txtMaska.BackColor = controlBackground;
            txtMaska.ForeColor = textColor;

            numVelikost.Location = new Point(270, 118);
            numVelikost.Width = 60;
            numVelikost.Maximum = 100000;
            numVelikost.BackColor = controlBackground;
            numVelikost.ForeColor = textColor;

            numDny.Location = new Point(430, 118);
            numDny.Width = 60;
            numDny.Maximum = 10000;
            numDny.BackColor = controlBackground;
            numDny.ForeColor = textColor;

            chkMazat.Text = "Smazat nalezené";
            chkMazat.Location = new Point(520, 118);
            chkMazat.ForeColor = errorColor;
            chkMazat.AutoSize = true;
            chkMazat.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            btnScan.Location = new Point(680, 110);
            btnScan.Size = new Size(140, 35);
            btnScan.Text = "SPUSTIT";
            ApplyButtonStyle(btnScan, true);

            // Info a Tabulka
            lblInfo.Text = "Připraveno...";
            lblInfo.Location = new Point(20, 160);
            lblInfo.ForeColor = accentColor;
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            lvResults.Location = new Point(20, 190);
            lvResults.Size = new Size(800, 440);
            lvResults.BackColor = controlBackground;
            lvResults.ForeColor = textColor;
            lvResults.BorderStyle = BorderStyle.None;
            lvResults.View = View.Details;
            lvResults.FullRowSelect = true;

            lvResults.Columns.Add("Soubor", 200);
            lvResults.Columns.Add("Stav", 100);
            lvResults.Columns.Add("Velikost", 100);
            lvResults.Columns.Add("Cesta", 350);
        }

        private void ApplyButtonStyle(Button btn, bool isPrimary = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = isPrimary ? accentColor : controlBackground;
            btn.ForeColor = Color.White;
        }

        [DllImport("user32.dll")] private extern static void ReleaseCapture();
        [DllImport("user32.dll")] private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); }
        }
    }
}