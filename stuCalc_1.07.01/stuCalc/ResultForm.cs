using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace testSpcAlc
{
	public partial class ResultForm : Form
	{
		private DataGridView dataGridView1;
		private DataTable mainDatas;
		public ResultForm(DataSet ds)
		{
			InitializeComponent();
			mainDatas = ds.Tables[0];
			DataGridInitializer();
		}
		private void InitializeComponent()
		{
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(375, 599);
			this.dataGridView1.TabIndex = 0;
			// 
			// ResultForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(375, 599);
			this.Controls.Add(this.dataGridView1);
			this.Name = "ResultForm";
			this.Text = "ResultForm";
			this.ResumeLayout(false);

		}

		private void DataGridInitializer()
		{
			this.dataGridView1.DataSource = mainDatas;
			this.dataGridView1.AutoResizeColumns();
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.ReadOnly = true;
		}
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < mainDatas.Columns.Count; i++)
			{
				sb.Append(mainDatas.Columns[i].ColumnName + " ");
			}
			sb.AppendLine();
			for(int i = 0; i < mainDatas.Rows.Count; i++)
			{
				for(int j = 0; j < mainDatas.Columns.Count; j++)
				{
					sb.Append(mainDatas.Rows[i][j] + " ");
				}
				sb.AppendLine();
			}
            Console.WriteLine(sb.ToString());
            base.OnFormClosing(e);
		}

	}
}
