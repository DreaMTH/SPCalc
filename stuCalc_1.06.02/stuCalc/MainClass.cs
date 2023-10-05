using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace testSpcAlc
{
	class MainClass
	{
		/// <summary>
		/// Const hint string.
		/// </summary>
		protected const string usageHint = "Information about program:\tUsage SPCalc.exe [-?] [-as | StudentName] [-ams | Name of file with students list]" +
							" [-aw | WorkName WorkType WorksMaxGrade] [-amw | Name of file with works] [-ae | StudentId WorkId Estimate] [-ue | StudentId WorkId Estimate]" +
							" [-rf | TableFileName] [-p]\n\t[-as | StudentName] -> Add new student, example : SPCalc.exe -as MyName\n\t" +
							"[-ams | Name of file with students list] -> Add multiple students from file, from file, example : SPCalc.exe -ams FileName.txt\n\t" +
							"[-aw | WorkName WorkType WorksMaxGrade] -> Add new work, example : SPCalc.exe -aw lab1 lab 5\n\t" +
							"[-amw | Name of file with works] -> Add multiple works from file, from file, example : SPCalc.exe -amw FileName.txt\n\t" +
							"[-ae | StudentId WorkId Estimate] -> Add new estimate, example : SPCalc.exe -ae 1 1 5.0\n\t" +
							"[-ue | StudentId WorkId Estimate] -> Update existing estimate, example : SPCalc.exe -ue 1 1 4.0\n\t" +
							"[-rf | TableFileName] -> Read changes from printed table, example : SPCalc.exe -rf TableFileName.txt\n\t" +
							"[-p] -> Print table, example : SPCalc.exe -p";
		/// <summary>
		/// additional fields which may be used during runtime.
		/// </summary>
		public static string stName = "";
		public static string stFileName = "";
		public static string workName = "";
		public static string workType = "";
		public static string workMax = "";
		public static string workFileName = "";
		public static string studId = "";
		public static string workId = "";
		public static string estimate = "";
		public static string readFileName = "";
		/// <summary>
		/// start point.
		/// </summary>
		/// <param name="args">use -? to recive information about args(keys)</param>
		static void Main(string[] args)
		{

			List<Operation> operations = new List<Operation>();
			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i].ToLower())
				{
					case "-?":
						Console.WriteLine(usageHint);
						continue;
					case "-as":
						operations.Add(Operation.AddStudent);
						stName = args[i + 1];
						continue;
					case "-ams":
						operations.Add(Operation.MultipleAddStudents);
						stFileName = args[i + 1];
						continue;
					case "-aw":
						operations.Add(Operation.AddWork);
						workName = args[i + 1];
						workType = args[i + 2];
						workMax = args[i + 3];
						continue;
					case "-amw":
						operations.Add(Operation.MultipleAddWorks);
						workFileName = args[i + 1];
						continue;
					case "-p":
						operations.Add(Operation.PrintTable);
						continue;
					case "-ae":
						operations.Add(Operation.AddGrade);
						studId = args[i + 1];
						workId = args[i + 2];
						estimate = args[i + 3];
						continue;
					case "-ue":
						operations.Add(Operation.UpdateGrade);
						studId = args[i + 1];
						workId = args[i + 2];
						estimate = args[i + 3];
						continue;
					case "-rf":
						operations.Add(Operation.ReadTable);
						//readFileName = args[i + 1];
						continue;
					default:
						//Console.WriteLine("Unknown key ->" + args[i]) useless message;
						continue;
				}
			}
			//operations.Add(Operation.PrintTable); //test func
			DB dB = new DB("foo.db"); //ALWAYS use this constructor or programm would not work.
			DbWrapper dbw = new DbWrapper(dB);
			for (int i = 0; i < operations.Count; i++)
			{
				switch (operations[i])
				{
					case Operation.AddStudent:
						dbw.StudentAdd(stName);
						break;
					case Operation.MultipleAddStudents:
						//reading file
						string[] stds = ReadStudentsFromFile(stFileName);
						dbw.StudentsAdd(stds);
						break;
					case Operation.AddWork:
						dbw.WorkAdd(workName, workType, int.Parse(workMax));
						break;
					case Operation.MultipleAddWorks:
						string[] works;
						string[] worksType;
						int[] max;
						ReadWorksFromFile(workFileName, out works, out worksType, out max);
						dbw.WorksAdd(works, worksType, max);
						break;
					case Operation.AddGrade:
						dbw.SetEstimateByID(int.Parse(studId), int.Parse(workId), double.Parse(estimate, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case Operation.UpdateGrade:
						dbw.UpdateEstimate(int.Parse(studId), int.Parse(workId), double.Parse(estimate, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case Operation.PrintTable:
						Console.WriteLine(dbw.GetPivotTable());
						break;
					case Operation.ReadTable:
						using (DGVForm dialogWindow = new DGVForm(dbw, readFileName))
						{
							dialogWindow.ShowDialog();
						}
						//dbw.UpdateTable(readFileName);
						break;
					default:
						break;
				}
			}
		}
		/// <summary>
		/// Method for reading students names from file
		/// </summary>
		/// <param name="path">Path to the file</param>
		/// <returns> String Array with students names</returns>
		private static string[] ReadStudentsFromFile(string path)
		{
			return File.ReadAllLines(path);
		}
		/// <summary>
		/// Method to parse files with works.
		/// </summary>
		/// <param name="path">Path for file</param>
		/// <param name="workName">Returns works names</param>
		/// <param name="workType">Returns types of works</param>
		/// <param name="max">Returns works max grade</param>
		private static void ReadWorksFromFile(string path, out string[] workName, out string[] workType, out int[] max) //the example of fields in file : Lab1.1 lab 4
		{
			string[] lines = File.ReadAllLines(path);
			workName = new string[lines.Length];
			workType = new string[lines.Length];
			max = new int[lines.Length];
			for (int i = 0; i < lines.Length; i++)
			{
				string[] tmp = lines[i].Split(' ');
				workName[i] = tmp[0].Trim();
				workType[i] = tmp[1].Trim();
				max[i] = int.Parse(tmp[2].Trim());
			}
		}
	}
}
