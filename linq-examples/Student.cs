using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linq_examples
{
    public class Student
    {
        public enum GradeLevel { FirstYear = 1, SecondYear, ThirdYear, FourthYear }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GradeLevel Year { get; set; }
        public List<int> ExamScores;

        protected static List<Student> students = new()
        {
            new Student
            {
                FirstName = "Terry",
                LastName = "Adams",
                Id = 120,
                Year = GradeLevel.SecondYear,
                ExamScores = new List<int> { 99, 82, 81, 79 }
            },
            new Student
            {
                FirstName = "Fadi",
                LastName = "Fakhouri",
                Id = 116,
                Year = GradeLevel.ThirdYear,
                ExamScores = new List<int> { 99, 86, 90, 94 }
            },
            new Student
            {
                FirstName = "Hanying",
                LastName = "Feng",
                Id = 117,
                Year = GradeLevel.FirstYear,
                ExamScores = new List<int> { 93, 92, 80, 87 }
            },
            new Student
            {
                FirstName = "Debra",
                LastName = "Garcia",
                Id = 115,
                Year = GradeLevel.ThirdYear,
                ExamScores = new List<int> { 35, 72, 91, 70 }
            },
            new Student
            {
                FirstName = "Cesar",
                LastName = "Garcia",
                Id = 114,
                Year = GradeLevel.FourthYear,
                ExamScores = new List<int> { 97, 89, 85, 82 }
            },
            new Student
            {
                FirstName = "Hugo",
                LastName = "Garcia",
                Id = 118,
                Year = GradeLevel.SecondYear,
                ExamScores = new List<int> { 92, 90, 83, 78 }
            },
            new Student
            {
                FirstName = "Sven",
                LastName = "Mortensen",
                Id = 113,
                Year = GradeLevel.FirstYear,
                ExamScores = new List<int> { 88, 94, 65, 91 }
            },
            new Student
            {
                FirstName = "Claire",
                LastName = "O'Donnell",
                Id = 112,
                Year = GradeLevel.FourthYear,
                ExamScores = new List<int> { 75, 84, 91, 39 }
            },
            new Student
            {
                FirstName = "Svetlana",
                LastName = "Omelchenko",
                Id = 111,
                Year = GradeLevel.SecondYear,
                ExamScores = new List<int> { 97, 92, 81, 60 }
            },
            new Student
            {
                FirstName = "Michael",
                LastName = "Tucker",
                Id = 122,
                Year = GradeLevel.FirstYear,
                ExamScores = new List<int> { 94, 92, 91, 91 }
            },
            new Student
            {
                FirstName = "Lance",
                LastName = "Tucker",
                Id = 119,
                Year = GradeLevel.ThirdYear,
                ExamScores = new List<int> { 68, 79, 88, 92 }
            },
            new Student
            {
                FirstName = "Eugene",
                LastName = "Zabokritski",
                Id = 121,
                Year = GradeLevel.FourthYear,
                ExamScores = new List<int> { 96, 85, 91, 60 }
            }
        };

        protected static int GetPercentile(Student s)
        {
            double avg = s.ExamScores.Average();
            return avg > 0 ? (int)avg / 10 : 0;
        }

        /// <summary>
        /// Group by a range example
        /// </summary>
        public static void GroupByRange()
        {
            Console.WriteLine("\r\nGroup by numeric range and project into a new anonymous type:");

            var queryNumericRange = from student in students
                                    let percentile = GetPercentile(student)
                                    group new { student.FirstName, student.LastName } by percentile into percentGroup
                                    orderby percentGroup.Key
                                    select percentGroup;
            // Nested foreach required to iterate over groups and group items.
            foreach (var studentGroup in queryNumericRange)
            {
                Console.WriteLine($"Key: {studentGroup.Key * 10}");
                foreach (var item in studentGroup)
                {
                    Console.WriteLine($"\t{item.LastName}, {item.FirstName}");
                }
            }
        }
        /// <summary>
        /// Group by comparison example
        /// </summary>
        public static void GroupByBoolean()
        {
            Console.WriteLine("\r\nGroup by a Boolean into two groups with string keys");
            Console.WriteLine("\"Tree\" and \"False\" and project into a new anonymous type:");
            var queryGroupByAverages = from student in students
                                       group new { student.FirstName, student.LastName }
                                       by student.ExamScores.Average() > 75 into studentGroup
                                       select studentGroup;

            foreach (var studentGroup in queryGroupByAverages)
            {
                Console.WriteLine($"Key: {studentGroup.Key}");
                foreach (var student in studentGroup)
                    Console.WriteLine($"\t{student.FirstName} {student.LastName}");
            }
        }
        /// <summary>
        /// Group by anonymous type: use an anonymous type to encapsulate a key that contains multiple values.
        /// </summary>
        public static void GroupByCompositeKey()
        {
            var queryHighScoreGroups = from student in students
                                       group student by new
                                       {
                                           FirstLetter = student.LastName[0],
                                           score = student.ExamScores[0] > 85
                                       } into studentGroup
                                       orderby studentGroup.Key.FirstLetter
                                       select studentGroup;
            Console.WriteLine("\r\nGroup and order by a compound key:");
            foreach (var studentGroup in queryHighScoreGroups)
            {
                string s = studentGroup.Key.score == true ? "more than" : "less than";
                Console.WriteLine($"Name starts with {studentGroup.Key.FirstLetter} who scored {s} 85");
                foreach (var student in studentGroup)
                {
                    Console.WriteLine($"\t{student.FirstName} {student.LastName}");
                }
            }

        }
        /// <summary>
        /// Perform a subquery on a grouping operation
        /// </summary>
        public static void QueryMax()
        {
            var queryGroupMax = from student in students
                                group student by student.Year into studentGroup
                                orderby studentGroup.Key
                                select new
                                {
                                    Level = studentGroup.Key,
                                    Name = studentGroup.Select(x => x.FirstName + " " + x.LastName).First(),
                                    HighestScore = (from student2 in studentGroup
                                                    select student2.ExamScores.Average()).Max()
                                };
            int count = queryGroupMax.Count();
            Console.WriteLine($"Number of groups = {count}");
            foreach (var item in queryGroupMax)
            {
                Console.WriteLine($"\t{item.Level} Highest Score = {item.HighestScore} by {item.Name}");
            }
        }
        /// <summary>
        /// Group by single property example
        /// </summary>
        public static void GroupBySingleProperty()
        {
            Console.WriteLine("Group by a single property in an object:");
            IEnumerable<IGrouping<string, Student>> queryByLastName = from student in students
                                                                      group student by student.LastName into newGroup
                                                                      orderby newGroup.Key
                                                                      select newGroup;
            foreach (var studentGroup in queryByLastName)
            {
                Console.WriteLine($"Key: { studentGroup.Key}");
                // foreach (var student in studentGroup.OrderBy(x => x.FirstName))
                foreach (var student in studentGroup)
                {
                    Console.WriteLine($"\t{student.LastName}, {student.FirstName}");
                }
            }
        }

        public static void QueryHighScores(int exam, int score)
        {
            var highScores = from student in students
                             where student.ExamScores[exam] > score
                             select new { Name = student.FirstName, Score = student.ExamScores[0] };
            foreach (var item in highScores)
            {
                Console.WriteLine($"{item.Name,-15}{item.Score}");
            }
        }

        /// <summary>
        /// To filter by using the Contains method
        /// </summary>
        /// <param name="ids"></param>
        public static void QueryByID(int[] ids)
        {
            var resultByIds = from student in students
                              let studentId = student.Id
                              where ids.Contains(studentId)
                              select student;

            foreach (var name in resultByIds)
            {
                Console.WriteLine($"{name.LastName}: {name.Id}");
            }
        }
    }
}
