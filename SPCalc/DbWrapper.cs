using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace SPCalc
{
#pragma warning disable CS8602
#pragma warning disable CS8604 //the null references is quite imposible. Enable warning if crash appears.
    /// <summary>
    /// Struct which implements some information about Data Base
    /// </summary>
    public struct DB
    {
        public string DbName { get; set; }
        /// <summary>
        /// Generates connection string to sqlite database from data bases name.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return $"DataSource={DbName}";
            }
        }
        /// <summary>
        /// Readonly field to generate table
        /// </summary>
        public readonly string studentsTable = @"CREATE TABLE IF NOT EXISTS stds(
	                                            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                                            Name char(256),
                                                UNIQUE(Name)
	                                            );";
        /// <summary>
        /// Readonly field to generate table
        /// </summary>
        public readonly string worksTable = @"CREATE TABLE IF NOT EXISTS works(
	                                            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
	                                            Name char(256),
	                                            type char (4),
	                                            max int
		                                        CHECK(max > 3 and max < 6)
		                                                                   );";
        /// <summary>
        /// Readonly field to generate table
        /// </summary>
        public readonly string resultTable = @"CREATE TABLE IF NOT EXISTS result(
	                                            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	                                            student_id INTEGER NOT NULL,
	                                            work_id INTEGER NOT NULL,
	                                            estimate DOUBLE,
	                                            FOREIGN KEY(student_id) REFERENCES stds(id),
	                                            FOREIGN KEY(work_id) REFERENCES works(id),
	                                            CHECK(estimate < 6.0)
	                                            );";
        /// <summary>
        /// Constructor (Necessarily to use it)
        /// </summary>
        /// <param name="dbName">Name of the database</param>
        public DB(string dbName)
        {
            this.DbName = dbName;
        }


    }
    /// <summary>
    /// Simple wrapper which encapsulates methods from ADO.NET
    /// </summary>
    public class DbWrapper
    {
        /// <summary>
        /// Instance of DB struct
        /// </summary>
        public DB dataBase { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataBase">DB struct instance</param>
        public DbWrapper(DB dataBase)
        {
            this.dataBase = dataBase;
            InitializeDb();
        }
        /// <summary>
        /// Initializer checks if database existing. If no - creates it.
        /// </summary>
        protected void InitializeDb()
        {
            if (!File.Exists(dataBase.DbName))
            {
                File.Create(dataBase.DbName).Close();
                CreateTable();
            }
        }
        /// <summary>
        /// Method creates all standart tables. It can be overrided.
        /// </summary>
        protected virtual void CreateTable()
        {
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    using (DbCommand cmd = new SQLiteCommand(String.Join(Environment.NewLine, dataBase.studentsTable, dataBase.worksTable, dataBase.resultTable), (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (DbCommand cmd = new SQLiteCommand("INSERT INTO works('Name', 'type', 'max') VALUES ('Add', 'Add', 5)", (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Method wich add new student into the database.
        /// Every new student recived 0 balls for Additional works to be appeared in the pivot table
        /// </summary>
        /// <param name="studentName">field with students name</param>
        public void StudentAdd(string studentName)
        {
            int newId;
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    string addCommand = $"INSERT OR IGNORE INTO stds('Name') VALUES('{studentName}');";
                    using (DbCommand cmd = new SQLiteCommand(addCommand, (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    string getNewId = $"SELECT id FROM stds WHERE Name = '{studentName}'";
                    using (DbCommand cmd = new SQLiteCommand(getNewId, (SQLiteConnection)dbc))
                    {
                        newId = int.Parse(cmd.ExecuteScalar().ToString());
                    }
                    string placeDefaultAdd = $"INSERT INTO result('student_id', 'work_id', 'estimate') VALUES({newId}, 1, 0.0);";
                    using (DbCommand cmd = new SQLiteCommand(placeDefaultAdd, (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Same that StudentAdd method, but to add multiple students.
        /// </summary>
        /// <param name="studentsName">Array with students names</param>
        public void StudentsAdd(string[] studentsName)
        {
            int newId;
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    for (int i = 0; i < studentsName.Length; ++i)
                    {
                        string addCommand = $"INSERT INTO stds('Name') VALUES('{studentsName[i]}');";
                        using (DbCommand cmd = new SQLiteCommand(addCommand, (SQLiteConnection)dbc))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        string getNewId = $"SELECT id FROM stds WHERE Name = '{studentsName[i]}'";
                        using (DbCommand cmd = new SQLiteCommand(getNewId, (SQLiteConnection)dbc))
                        {
                            newId = int.Parse(cmd.ExecuteScalar().ToString());
                        }
                        string placeDefaultAdd = $"INSERT INTO result('student_id', 'work_id', 'estimate') VALUES({newId}, 1, 0.0);";
                        using (DbCommand cmd = new SQLiteCommand(placeDefaultAdd, (SQLiteConnection)dbc))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Method which add new work.
        /// </summary>
        /// <param name="workName">Name of the work</param>
        /// <param name="workType">Type of the work(add, lab, mod)</param>
        /// <param name="workMax">The max grade for work</param>
        public void WorkAdd(string workName, string workType, int workMax)
        {
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    string addCommand = $"INSERT INTO works('Name', 'type', 'max') VALUES('{workName}','{workType}',{workMax});";
                    using (DbCommand cmd = new SQLiteCommand(addCommand, (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Same method that WorkAdd is, but for multiple works
        /// </summary>
        /// <param name="worksName">Array with works names</param>
        /// <param name="worksTypes">Array with works types</param>
        /// <param name="worksMax">Array with works max grades</param>
        public void WorksAdd(string[] worksName, string[] worksTypes, int[] worksMax)
        {
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    for (int i = 0; i < worksName.Length; ++i)
                    {
                        string addCommand = $"INSERT INTO works('Name', 'type', 'max') VALUES('{worksName[i]}','{worksTypes[i]}',{worksMax[i]});";
                        using (DbCommand cmd = new SQLiteCommand(addCommand, (SQLiteConnection)dbc))
                        {
                            cmd.ExecuteNonQuery();
                        }

                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Method which setting sudents estimate for the task
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="workId">Task id</param>
        /// <param name="estimate">Estimate for the task</param>
        public void SetEstimateByID(int studentId, int workId, double estimate)
        {
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    string InsertCommand = $"INSERT INTO result('student_id', 'work_id', 'estimate') VALUES({studentId}, {workId}, {estimate})";
                    using (DbCommand cmd = new SQLiteCommand(InsertCommand, (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Method which updates students estimate for the work if estimate has not equaled 0.
        /// </summary>
        /// <param name="studentId">Student id</param>
        /// <param name="workId">Work id</param>
        /// <param name="estimate">Estimate</param>
        public void UpdateEstimate(int studentId, int workId, double estimate)
        {
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    string UpdatingCommand = $"UPDATE result SET estimate = {estimate} WHERE student_id = {studentId} AND work_id = {workId};";
                    using (DbCommand cmd = new SQLiteCommand(UpdatingCommand, (SQLiteConnection)dbc))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    dbc.Close();
                }
            }
        }
        /// <summary>
        /// Method which generates SQL Query to build dynamic pivot table.
        /// This method can be overrited.
        /// </summary>
        /// <returns>String with query request</returns>
        protected virtual string GetRequest()
        {
            StringBuilder sb = new StringBuilder();
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    SQLiteDataAdapter sqlda = new SQLiteDataAdapter($"select * from works;", (SQLiteConnection)dbc);
                    DataSet ds = new DataSet();
                    sqlda.Fill(ds);
                    sb.Append(@"SELECT stds.id, stds.Name");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sb.Append($"\n, SUM(CASE WHEN stds.id = result.student_id AND result.work_id = {i + 1} THEN result.estimate ELSE 0 END)'{ds.Tables[0].Rows[i][1].ToString().Trim()}'");
                    }
                    sb.Append(@"FROM stds
	                            JOIN result ON stds.id = result.student_id
	                            JOIN works ON result.work_id = works.id
	                            GROUP BY stds.Id;");
                    dbc.Close();
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Method which making Querry request and recives pivot table.
        /// </summary>
        /// <returns>String field with pivot table</returns>
        public string GetPivotTable()
        {
            StringBuilder sb = new StringBuilder();
            using (SQLiteFactory sqf = new SQLiteFactory())
            {
                using (DbConnection dbc = sqf.CreateConnection())
                {
                    dbc.ConnectionString = dataBase.ConnectionString;
                    dbc.Open();
                    SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(GetRequest(), (SQLiteConnection)dbc);
                    DataSet dataSet = new DataSet();
                    sQLiteDataAdapter.Fill(dataSet);
                    for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                    {
                        sb.Append($"{dataSet.Tables[0].Columns[i].ColumnName.Trim()}\t");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
                        {
                            sb.Append($"{dataSet.Tables[0].Rows[i][j].ToString().Trim()}\t");
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }
                    sb.Remove(sb.Length - 2, 2);
                    dbc.Close();
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Method which gets pivot table from database, reads pivot table from user-input and compares students estimates. After that method updates old values. 
        /// </summary>
        /// <param name="path">Path to the file with users table</param>
        public void UpdateTable(string path)
        {
            string currentTable = GetPivotTable();
            string[,] cTable = splitTable(currentTable);
            string[,] newTable = readFromFile(path);
            for (int i = 1; i < cTable.GetLength(0); i++)
            {

                for (int j = 2; j < cTable.GetLength(1); j++)
                {
                    //Console.WriteLine($"i={i} j = {j} old = {cTable[i,j]} new = {newTable[i,j]}"); debug output
                    double oldEstimate = 0.0;
                    double newEstimate = 0.0;
                    double.TryParse(cTable[i, j], out oldEstimate);
                    double.TryParse(newTable[i, j], out newEstimate);

                    if (oldEstimate != newEstimate)
                    {
                        if (oldEstimate == 0)
                        {
                            SetEstimateByID(i, j - 1, newEstimate);
                        }
                        else
                        {
                            UpdateEstimate(i, j - 1, newEstimate);
                            //Console.WriteLine($"j-1 = {j - 1} i = {i}"); debug output
                        }

                    }



                }
            }
        }
        /// <summary>
        /// Method which parses file with users pivot table
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>2D array with users pivot table</returns>
        protected string[,] readFromFile(string path)
        {
            string[,] result;
            string[] lines = File.ReadAllLines(path);
            int rowCount = lines.Length;
            int columnCount = lines[0].Split('\t').Length;
            result = new string[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                string[] tempLine = lines[i].Split("\t");
                for (int j = 0; j < columnCount; j++)
                {
                    result[i, j] = tempLine[j].Trim();
                    //Console.Write($"[{i},{j}] = {result[i,j]}"); 
                }
                //Console.WriteLine(); 
            }
            return result;
        }
        /// <summary>
        /// Method wich parses string field with pivot table from database into 2D array
        /// </summary>
        /// <param name="tableString">String field with pivot table from the database</param>
        /// <returns>2D array with pivot table from the database</returns>
        protected string[,] splitTable(string tableString)
        {
            string[] lines = tableString.Split("\n");
            int rowsCount = lines.Length;
            int columnsCount = lines[0].Split('\t', StringSplitOptions.RemoveEmptyEntries).Length;
            string[,] result = new string[rowsCount, columnsCount];
            for (int i = 0; i < rowsCount; i++)
            {
                string[] tempLine = lines[i].Split("\t");
                for (int j = 0; j < columnsCount; j++)
                {
                    result[i, j] = tempLine[j].Trim();
                    //Console.Write($"[{i},{j}] = {result[i, j]}"); debug console output.
                }
            }
            return result;
        }

    }
}
