using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO Design form, overwrite reading method overwrite updating method
namespace SPCalc
{
	public partial class DialogDVG : Form
	{
		private bool isSaved { get; set; } = true;
		private DataSet mainDatas { get; set; }
		private string SaveFileName { get; set; }
		private DbWrapper dbWrapper { get; set; }
		public DialogDVG(DbWrapper dbWrap, string fileName)
		{
			InitializeComponent();
			this.mainDatas = dbWrap.GetData();
			this.dbWrapper = new DbWrapper(dbWrap.dataBase);
			DataGridInitializer();
			SaveFileName = fileName;
		}
		private void DataGridInitializer()
		{
			this.dataGridView1.DataSource = mainDatas.Tables[0];
			this.dataGridView1.AutoResizeColumns();	
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!isSaved)
			{
				var response = MessageBox.Show("Save changes before exit?", "Changes has not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (response)
				{
					case DialogResult.Yes:
						//logic
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
				for(int j = 2; j < this.dataGridView1.ColumnCount; j++)
				{
					double oldEstimate = 0.0;
					double newEstimate = 0.0;
					double.TryParse(compareDataSet.Tables[0].Rows[i][j].ToString(), out oldEstimate);
					double.TryParse(this.dataGridView1.Rows[i].Cells[j].Value.ToString(), out newEstimate);        
                    if (oldEstimate != newEstimate)
					{
						if (oldEstimate == 0)
						{
							this.dbWrapper.SetEstimateByID(i+1, j - 1, newEstimate);
						}
						else
						{
							this.dbWrapper.UpdateEstimate(i+1, j - 1, newEstimate);
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
