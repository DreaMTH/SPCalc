using System.Data;
using System.Windows.Forms;
using System.Drawing;
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
						SaveButton.PerformClick();
						isSaved = true;
						this.Close();
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
	}
}
