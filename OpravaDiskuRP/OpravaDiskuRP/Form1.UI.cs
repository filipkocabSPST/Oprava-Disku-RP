using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpravaDiskuRP
{
    public partial class Form1 : Form
    {
        private readonly Color darkBackground = Color.FromArgb(45, 45, 48);
        private readonly Color controlBackground = Color.FromArgb(63, 63, 70);
        private readonly Color accentColor = Color.FromArgb(0, 122, 204);
        private readonly Color hoverColor = Color.FromArgb(28, 151, 234);
        private readonly Color textColor = Color.Gainsboro;
        private readonly Color borderColor = Color.FromArgb(80, 80, 80);

        private void InitializeModernUi()
        {
            ConfigureWindow();
            CreateCustomTitleBar();
            StyleSpecificControls();
        }

        private void ConfigureWindow()
        {
            // Beze změny
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = darkBackground;
            this.Font = new Font("Segoe UI", 9f);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(800, 600);
            this.Padding = new Padding(2);
            this.DoubleBuffered = true;
        }

        private void CreateCustomTitleBar()
        {
            Panel pnlTitleBar = new Panel { Dock = DockStyle.Top, Height = 32, BackColor = accentColor, };
            Label lblTitle = new Label { Text = "Oprava Disku RP", ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true };

            // Změna 1: Pořadí tlačítek (Zavření je poslední)
            Button btnClose = CreateTitleBarButton("✕", DockStyle.Right);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(190, 30, 30); // Červené efekty pro zavření
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 50, 50);
            btnClose.Click += (s, e) => this.Close();

            Button btnMinimize = CreateTitleBarButton("—", DockStyle.Right); // Minimalizace je před zavřením
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            lblTitle.MouseDown += TitleBar_MouseDown;

            pnlTitleBar.Controls.Add(lblTitle);
            pnlTitleBar.Controls.Add(btnMinimize); // Minimalizace se přidá první
            pnlTitleBar.Controls.Add(btnClose);    // Zavření až po ní
            this.Controls.Add(pnlTitleBar);
        }

        private Button CreateTitleBarButton(string text, DockStyle dock)
        {
            Button btn = new Button { Text = text, ForeColor = Color.White, Font = new Font("Segoe UI", 11f, FontStyle.Regular), Dock = dock, Width = 45, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 75); // Výchozí hover efekt
            return btn;
        }

        private void StyleSpecificControls()
        {
            lblFolderPath.Text = "Cesta ke složce:";
            lblFolderPath.ForeColor = textColor;
            lblFolderPath.Location = new Point(20, 50);
            lblFolderPath.AutoSize = true;

            pnlPathBorder.Location = new Point(20, 75);
            pnlPathBorder.Size = new Size(500, 28);
            pnlPathBorder.BackColor = borderColor;
            pnlPathBorder.Padding = new Padding(1);
            pnlPathBorder.Controls.Add(txtFolderPath);

            txtFolderPath.Dock = DockStyle.Fill;
            txtFolderPath.BorderStyle = BorderStyle.None;
            txtFolderPath.BackColor = controlBackground;
            txtFolderPath.ForeColor = textColor;
            txtFolderPath.Font = new Font("Segoe UI", 10f);

            btnSelectFolder.Location = new Point(530, 75);
            btnSelectFolder.Size = new Size(100, 28);
            btnSelectFolder.Text = "Vybrat...";
            ApplyButtonStyle(btnSelectFolder);

            btnScan.Location = new Point(640, 75);
            btnScan.Size = new Size(120, 28);
            btnScan.Text = " PROHLEDAT";
            ApplyButtonStyle(btnScan, true);

            // --- Změna 2: Stylování ListView místo TextBoxu ---
            lvResults.Location = new Point(20, 120);
            lvResults.Size = new Size(760, 460);
            lvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvResults.BorderStyle = BorderStyle.None;
            lvResults.BackColor = controlBackground;
            lvResults.ForeColor = textColor;
            lvResults.Font = new Font("Segoe UI", 9f);

            // Definice sloupců pro náš ListView
            lvResults.Columns.Add("Název souboru", 250);
            lvResults.Columns.Add("Složka", 350);
            lvResults.Columns.Add("Velikost", 100, HorizontalAlignment.Right); // zarovnat doprava
        }

        private void ApplyButtonStyle(Button btn, bool isPrimary = false)
        {
            // beze změny
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 9f, isPrimary ? FontStyle.Bold : FontStyle.Regular);
            btn.ForeColor = Color.White;
            btn.BackColor = isPrimary ? accentColor : Color.FromArgb(70, 70, 75);
            btn.MouseEnter += (s, e) => btn.BackColor = isPrimary ? hoverColor : Color.FromArgb(90, 90, 95);
            btn.MouseLeave += (s, e) => btn.BackColor = isPrimary ? accentColor : Color.FromArgb(70, 70, 75);
        }

        // Win32 API kód pro pohyb oknem zůstává stejný
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }
    }
}