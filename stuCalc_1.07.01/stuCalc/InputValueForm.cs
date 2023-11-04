using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testSpcAlc
{
	public partial class InputValueForm : Form
	{
		private Button inputButton;
		private TextBox inputTBox;
		public double estimate { get; set; }

		public InputValueForm()
		{
			InitializeComponent();
		}
		private void InitializeComponent()
		{
			this.inputTBox = new System.Windows.Forms.TextBox();
			this.inputButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// inputTBox
			// 
			this.inputTBox.Location = new System.Drawing.Point(90, 30);
			this.inputTBox.Name = "inputTBox";
			this.inputTBox.Size = new System.Drawing.Size(100, 20);
			this.inputTBox.TabIndex = 0;
			// 
			// inputButton
			// 
			this.inputButton.Location = new System.Drawing.Point(9, 30);
			this.inputButton.Name = "inputButton";
			this.inputButton.Size = new System.Drawing.Size(75, 23);
			this.inputButton.TabIndex = 1;
			this.inputButton.Text = "Input:";
			this.inputButton.UseVisualStyleBackColor = true;
			this.inputButton.Click += new System.EventHandler(this.inputButton_Click);
			// 
			// InputValueForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(220, 100);
			this.Controls.Add(this.inputButton);
			this.Controls.Add(this.inputTBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "InputValueForm";
			this.Text = "InputValueForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		private void inputButton_Click(object sender, EventArgs e)
		{
			double result;
			NumberFormatInfo nbf = new NumberFormatInfo();
			nbf.CurrencyDecimalSeparator = ".";
			nbf.NumberDecimalSeparator = ".";
			result = double.Parse(inputTBox.Text, nbf);
            if (result > 10)
            {
				nbf.CurrencyDecimalSeparator = ",";
				nbf.NumberDecimalSeparator = ",";
				result = double.Parse(inputTBox.Text, nbf);
			}
            this.estimate = result;
			DialogResult = DialogResult.OK;
		}
	}
}
