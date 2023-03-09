using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentScoreDemo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var studentCollection = InputStudents();

			Console.WriteLine("所有考生成績報表");
			const int len = 30;
			Console.WriteLine(new string('=', len));

			int counter = 1;
			foreach (var student in studentCollection)
			{
				Console.WriteLine($"序號 {counter++}:{student.ToString()}");
			}

			Console.WriteLine(new string('=', len));

			Console.WriteLine($"實際到考人數: {studentCollection.Count} 人");

			Console.WriteLine($"最高總分: {studentCollection.MaxScore:F2}分");
			Console.WriteLine($"最低總分: {studentCollection.MinScore:F2}分");
			Console.WriteLine($"所有考生總成績平均: {studentCollection.Average.ToString("F2").PadLeft(5)}");

			Console.Read();
		}

		/// <summary>
		/// 輸入多位學生成績, 直到輸入 'n' 為止
		/// </summary>
		/// <returns></returns>
		static StudentCollection InputStudents()
		{
			List<Student> students = new List<Student>();

			while (true)
			{
				students.Add(InputScore());

				Console.Write("是否繼續輸入下一筆(y/n)？");
				string input = Console.ReadLine();

				while (input != "y" && input != "n")
				{
					Console.Write("輸入無效，請重新輸入(y/n)：");
					input = Console.ReadLine();
				}

				if (input == "n")
				{
					break;
				}
			}

			return new StudentCollection(students);
		}


		/// <summary>
		/// 輸入一位學生成績
		/// </summary>
		/// <returns></returns>
		static Student InputScore()
		{
			#region 學科成績

			Console.Write("請輸入學科成績（0~100）：");
			int academic;

			while (!int.TryParse(Console.ReadLine(), out academic) || academic < 0 || academic > 100)
			{
				Console.Write("成績必須介於0~100之間，請重新輸入：");
			}

			#endregion

			#region 術科成績

			Console.Write("請輸入術科成績（0~100）：");
			int technical;

			while (!int.TryParse(Console.ReadLine(), out technical) || technical < 0 || technical > 100)
			{
				Console.Write("成績必須介於0~100之間，請重新輸入：");
			}

			#endregion


			#region 姓名

			Console.Write("請輸入姓名：");
			string name = Console.ReadLine();

			while (string.IsNullOrEmpty(name))
			{
				Console.Write("姓名不可為空白，請重新輸入：");
				name = Console.ReadLine();
			}

			#endregion

			return new Student(academic, technical, name);
		}
	}

	/// <summary>
	/// 單一學生成績
	/// </summary>
	public class Student
	{
		public string Name { get; private set; } // 姓名
		public int Academic { get; private set; } // 學科成績
		public int Technical { get; private set; } // 術科成績

		public double Total { get; private set; } // 總成績

		public Student(int academic, int technical, string name)
		{
			Academic = academic;
			Technical = technical;
			Name = name;

			Total = Academic / 2.0 + Technical / 2.0; // 事先算好,方便稍後重覆取用,不必每次都計算
		}
		

		public override string ToString()
		{
			return $"{Name,-10}		學科:{Academic,3}分	術科:{Technical,3} 分  總成績:{Total,-5:F2} 分";
		}
	}

	public class StudentCollection:IEnumerable<Student>
	{
		private List<Student> _students;

		public StudentCollection(List<Student> students)
		{
			_students = students;
		}

		// 取得最高分與最低分
		public double MaxScore => _students.Max(s => s.Total);
		public double MinScore => _students.Min(s => s.Total);

		// 總平均
		public double Average =>_students.Average(s => s.Total);

		public int Count => _students.Count;

		public IEnumerator<Student> GetEnumerator()
		{
            return _students.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
