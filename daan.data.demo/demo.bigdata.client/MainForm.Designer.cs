namespace demo.bigdata.client
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.mItem_ImportOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.mItem_ExportOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItem_ImportOrders,
            this.mItem_ExportOrders});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(782, 28);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // mItem_ImportOrders
            // 
            this.mItem_ImportOrders.Name = "mItem_ImportOrders";
            this.mItem_ImportOrders.Size = new System.Drawing.Size(81, 24);
            this.mItem_ImportOrders.Text = "导入订单";
            this.mItem_ImportOrders.Click += new System.EventHandler(this.mItem_ImportOrders_Click);
            // 
            // mItem_ExportOrders
            // 
            this.mItem_ExportOrders.Name = "mItem_ExportOrders";
            this.mItem_ExportOrders.Size = new System.Drawing.Size(81, 24);
            this.mItem_ExportOrders.Text = "导出订单";
            this.mItem_ExportOrders.Click += new System.EventHandler(this.mItem_ExportOrders_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 527);
            this.panel1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 555);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem mItem_ImportOrders;
        private System.Windows.Forms.ToolStripMenuItem mItem_ExportOrders;
        private System.Windows.Forms.Panel panel1;
    }
}