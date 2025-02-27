﻿using System;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(string title)
        {
            InitializeComponent();
            this.Text = title;
        }

        public void UpdateProgress(int current, int total, string fileName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(current, total, fileName)));
                return;
            }

            progressBar.Maximum = total;
            progressBar.Value = current;
            statusLabel.Text = $"Spracovávam súbor {current} z {total}: {fileName}";
            this.Update();
        }
    }
}
