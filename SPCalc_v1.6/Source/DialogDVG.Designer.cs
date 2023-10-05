namespace SPCalc
{
	partial class DialogDVG
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
			panel1 = new Panel();
			dataGridView1 = new DataGridView();
			panel2 = new Panel();
			SaveButton = new Button();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			panel2.SuspendLayout();
			SuspendLayout();
			// 
			// panel1
			// 
			panel1.Controls.Add(dataGridView1);
			panel1.Controls.Add(panel2);
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new Size(800, 450);
			panel1.TabIndex = 0;
			// 
			// dataGridView1
			// 
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Dock = DockStyle.Fill;
			dataGridView1.Location = new Point(0, 0);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.RowTemplate.Height = 25;
			dataGridView1.Size = new Size(800, 395);
			dataGridView1.TabIndex = 1;
			dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			// 
			// panel2
			// 
			panel2.Controls.Add(SaveButton);
			panel2.Dock = DockStyle.Bottom;
			panel2.Location = new Point(0, 395);
			panel2.Name = "panel2";
			panel2.Size = new Size(800, 55);
			panel2.TabIndex = 0;
			// 
			// SaveButton
			// 
			SaveButton.Location = new Point(12, 20);
			SaveButton.Name = "SaveButton";
			SaveButton.Size = new Size(75, 23);
			SaveButton.TabIndex = 0;
			SaveButton.Text = "Save";
			SaveButton.UseVisualStyleBackColor = true;
			SaveButton.Click += SaveButton_Click;
			// 
			// DialogDVG
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(panel1);
			Name = "DialogDVG";
			Text = "DialogDVG";
			panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			panel2.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel panel1;
		private DataGridView dataGridView1;
		private Panel panel2;
		private Button SaveButton;
	}
}
