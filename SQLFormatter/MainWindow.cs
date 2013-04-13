using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SQLFormatter
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();

		}

		private void inputBox_TextChanged(object sender, EventArgs e)
		{
			outputBox.Text = SQLFormatter.formatSQL(inputBox.Text, cbxSingle.Checked);
		}

		private void cbxSingle_CheckedChanged(object sender, EventArgs e)
		{
			outputBox.Text = SQLFormatter.formatSQL(inputBox.Text, cbxSingle.Checked);
		}

		private void btnCopyInput_Click(object sender, EventArgs e)
		{
			inputBox.Text = outputBox.Text;
		}

		private void btnCopyClipboard_Click(object sender, EventArgs e)
		{
			try
			{
				Clipboard.SetText(outputBox.Text);
			}
			catch (Exception ex) { 
				//Clipboard generates random failures, but retries automatically, we can ignore these.
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			inputBox.Text = String.Empty;
		}

	}
}
