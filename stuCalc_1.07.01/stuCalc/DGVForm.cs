using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testSpcAlc
{
	class DGVForm : Form
	{
		private Panel panel1;
		private DataGridView dataGridView1;
		private Panel panel2;
		private Button SaveButton;
		private bool isSaved { get; set; }
		private DataSet mainDatas { get; set; }
		private DbWrapper dbWrapper { get; set; }
		public DGVForm(DbWrapper dbWrap)
		{
			InitializeComponent();
			isSaved = true;
			dbWrapper = new DbWrapper(dbWrap.dataBase);
			mainDatas = dbWrapper.GetData();
			DataGridInitializer();
        }
		private void DataGridInitializer()
		{
			this.dataGridView1.DataSource = mainDatas.Tables[0];
			this.dataGridView1.AutoResizeColumns();
			System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Globalization.CultureInfo.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

		}
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.SaveButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.dataGridView1);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(686, 390);
			this.panel1.TabIndex = 0;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 25;
			this.dataGridView1.Size = new System.Drawing.Size(686, 342);
			this.dataGridView1.TabIndex = 1;
			this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.SaveButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 342);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(686, 48);
			this.panel2.TabIndex = 0;
			// 
			// SaveButton
			// 
			this.SaveButton.Location = new System.Drawing.Point(10, 17);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(64, 20);
			this.SaveButton.TabIndex = 0;
			this.SaveButton.Text = "Save";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// DGVForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(686, 390);
			this.Controls.Add(this.panel1);
			this.Name = "DGVForm";
			this.Text = "DialogDVG";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!isSaved)
			{
				var response = MessageBox.Show("Save changes before exit?", "Changes has not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (response)
				{
					case DialogResult.Yes:
						SaveButton.PerformClick();
						break;
					case DialogResult.No:
						Console.WriteLine("Exit without changes...");
						break;
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
				}
			}
			base.OnFormClosing(e);

		}
		private void SaveButton_Click(object sender, EventArgs e)
		{
			this.isSaved = true;
			DataSet compareDataSet = dbWrapper.GetData();
			Console.WriteLine(dataGridView1.RowCount);
			for (int i = 0; i < this.dataGridView1.RowCount - 1; i++)
			{
				for (int j = 2; j < this.dataGridView1.ColumnCount; j++)
				{
					double oldEstimate = 0.0;
					double newEstimate = 0.0;
					double.TryParse(compareDataSet.Tables[0].Rows[i][j].ToString(), out oldEstimate);
					double.TryParse(this.dataGridView1.Rows[i].Cells[j].Value.ToString(), out newEstimate);
					if (oldEstimate != newEstimate)
					{
						if (oldEstimate == 0)
						{
							this.dbWrapper.SetEstimateByID(i + 1, j - 1, newEstimate);
						}
						else
						{
							this.dbWrapper.UpdateEstimate(i + 1, j - 1, newEstimate);
						}

					}
				}
			}
			Console.WriteLine(dbWrapper.GetPivotTable());
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			this.isSaved = false;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			this.isSaved = false;
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			isSaved = false;	
			using(InputValueForm ipv = new InputValueForm())
			{
				if(ipv.ShowDialog(this) == DialogResult.OK)
				{
                    Console.WriteLine(ipv.estimate);
                    double a = ipv.estimate;
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = a;
				}
			}
		}
	}
}
