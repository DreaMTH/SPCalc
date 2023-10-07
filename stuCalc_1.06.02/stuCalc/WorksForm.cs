using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testSpcAlc
{
	class WorksForm : Form
	{
		private DataGridView dataGridView1;
		private Panel panel2;
		private Button SaveWorksButton;
		private Panel panel1;
		private bool isSaved { get; set; } = true;
		private DataSet mainDatas { get; set; }
		private DbWrapper dbWrapper { get; set; }
		private int NewRows { get; set; }
		public WorksForm(DbWrapper dbWrap)
		{
			InitializeComponent();
			NewRows = 0;
			mainDatas = dbWrap.GetWorks();
			dbWrapper = new DbWrapper(dbWrap.dataBase);
			DataGridInitializer();
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
						this.SaveWorksButton.PerformClick();
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
			var compareData = dbWrapper.GetWorks().Tables[0];
			for(int i = 0; i < mainDatas.Tables[0].Rows.Count - 1; i++)
			{
				for(int j = 1; j < mainDatas.Tables[0].Columns.Count; j++)
				{
					if (dataGridView1.Rows[i].Cells[j].Value != compareData.Rows[i][j])
					{
						switch (j)
						{
							case 1:
								dbWrapper.UpdateWorkName(dataGridView1.Rows[i].Cells[j].Value.ToString(), dataGridView1.Rows[i].Cells[0].Value.ToString());
								break;
							case 2:
								dbWrapper.UpdateWorkType(dataGridView1.Rows[i].Cells[j].Value.ToString(), dataGridView1.Rows[i].Cells[0].Value.ToString());
								break;
							case 3:
								dbWrapper.UpdateWorkMax(dataGridView1.Rows[i].Cells[j].Value.ToString(), dataGridView1.Rows[i].Cells[0].Value.ToString());
								break;
							case 4:
								dbWrapper.UpdateWorkFRep(dataGridView1.Rows[i].Cells[j].Value.ToString(), dataGridView1.Rows[i].Cells[0].Value.ToString());
								break;
							default:
								break;
						}
					}
				}
			}
			if(NewRows != 0)
			{
				for (int i = compareData.Rows.Count; i < dataGridView1.RowCount; i++)
				{
					if (dataGridView1.Rows[i].Cells[1].Value != null)
					{
						dbWrapper.WorkAdd(dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString()
							, int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()), int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
					}
				}
			}
			
		}
		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			this.isSaved = false;
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			this.isSaved = false;
		}
		private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			NewRows++;
		}
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.SaveWorksButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
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
			this.panel1.Size = new System.Drawing.Size(457, 411);
			this.panel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.SaveWorksButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 359);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(457, 52);
			this.panel2.TabIndex = 0;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(457, 359);
			this.dataGridView1.TabIndex = 1;
			this.dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
			this.dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			this.dataGridView1.UserAddedRow += dataGridView1_UserAddedRow;
			// 
			// SaveWorksButton
			// 
			this.SaveWorksButton.Location = new System.Drawing.Point(12, 17);
			this.SaveWorksButton.Name = "SaveWorksButton";
			this.SaveWorksButton.Size = new System.Drawing.Size(75, 23);
			this.SaveWorksButton.TabIndex = 0;
			this.SaveWorksButton.Text = "Save";
			this.SaveWorksButton.UseVisualStyleBackColor = true;
			this.SaveWorksButton.Click += SaveButton_Click;
			// 
			// WorksForm
			// 
			this.ClientSize = new System.Drawing.Size(457, 411);
			this.Controls.Add(this.panel1);
			this.Name = "WorksForm";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
