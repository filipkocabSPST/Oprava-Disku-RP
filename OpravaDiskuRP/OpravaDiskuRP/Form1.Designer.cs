namespace OpravaDiskuRP
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblFolderPath = new System.Windows.Forms.Label();
            this.pnlPathBorder = new System.Windows.Forms.Panel();
            this.lvResults = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtFolderPath.Location = new System.Drawing.Point(614, 205);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(100, 22);
            this.txtFolderPath.TabIndex = 0;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(123, 37);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "button1";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(123, 217);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "button1";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblFolderPath
            // 
            this.lblFolderPath.AutoSize = true;
            this.lblFolderPath.Location = new System.Drawing.Point(519, 110);
            this.lblFolderPath.Name = "lblFolderPath";
            this.lblFolderPath.Size = new System.Drawing.Size(44, 16);
            this.lblFolderPath.TabIndex = 4;
            this.lblFolderPath.Text = "label1";
            // 
            // pnlPathBorder
            // 
            this.pnlPathBorder.Location = new System.Drawing.Point(303, 180);
            this.pnlPathBorder.Name = "pnlPathBorder";
            this.pnlPathBorder.Size = new System.Drawing.Size(200, 100);
            this.pnlPathBorder.TabIndex = 5;
            // 
            // lvResults
            // 
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(574, 288);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(121, 97);
            this.lvResults.TabIndex = 6;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvResults);
            this.Controls.Add(this.pnlPathBorder);
            this.Controls.Add(this.lblFolderPath);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.txtFolderPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label lblFolderPath;
        private System.Windows.Forms.Panel pnlPathBorder;
        private System.Windows.Forms.ListView lvResults;
    }
}

