using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace demo.bigdata.client
{
    public partial class MainForm : Form
    {
        ImportOrderForm importOrderForm;
        ExportOrderForm exportOrderForm;

        public MainForm()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            importOrderForm = new ImportOrderForm()
            {
                TopLevel = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                MaximizeBox = false,
                MinimizeBox = false,
                Visible = false,
                Dock = DockStyle.Fill,
            };
            panel1.Controls.Add(importOrderForm);

            exportOrderForm = new ExportOrderForm()
            {
                TopLevel = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None,
                MaximizeBox = false,
                MinimizeBox = false,
                Visible = false,
                Dock = DockStyle.Fill,
            };
            panel1.Controls.Add(exportOrderForm);
        }

        private void mItem_ImportOrders_Click(object sender, EventArgs e)
        {
            exportOrderForm.Hide();
            importOrderForm.Show();
        }

        private void mItem_ExportOrders_Click(object sender, EventArgs e)
        {
            importOrderForm.Hide();
            exportOrderForm.Show();
        }
    }
}
