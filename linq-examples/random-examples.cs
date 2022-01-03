using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linq_examples
{
    internal class RandomExample
    {
        // Create the IEnumerable data sources.
        private static readonly string[] _list1 = System.IO.File.ReadAllLines(@"../../../files/names1.txt");
        private static readonly string[] _list2 = System.IO.File.ReadAllLines(@"../../../files/names2.txt");
        private static readonly string[] _studentNames = System.IO.File.ReadAllLines(@"../../../files/student-names.csv");
        private static readonly string[] _scores = System.IO.File.ReadAllLines(@"../../../files/scores.csv");
        public static void Call()
        {
            //RandomExample.Grouping();
            //RandomExample.FindSentenceContainSpecifiedSetOfWords();
            //RandomExample.FindSetDifferenceBetweenTwoLists();
            //RandomExample.CombineAndCompareStringCollections();
            //RandomExample.ReorderFieldsOfADelimitedFile();
            //RandomExample.JoinContentFromDissimilarFiles();
            //RandomExample.SplitFileIntoManyFileUsingGroups();
            RandomExample.ComputeColumnValuesInCSV();
        }
        private static void ComputeColumnValuesInCSV()
        {
            var result = (from score in _scores
                          let scoreArray = score.Split(',')
                          let scores = scoreArray.Skip(1)
                          select (from num in scores
                                  select Convert.ToInt32(num)
                                  )).ToList();

            int columnCount = result[0].Count();


            // Perform aggregate calculations Average, Max, and  
            // Min on each column.
            // Perform one iteration of the loop for each column
            // of scores.  
            // You can use a for loop instead of a foreach loop
            // because you already executed the multiColQuery
            // query by calling ToList. 
            for (int col = 0; col < columnCount; col++)
            {
                var result2 = from row in result
                              let x = row
                              select row.ElementAt(col);

                double avg = result2.Average();
                int max = result2.Max();
                int min = result2.Min();
                Console.WriteLine("Exam #{0} Average: {1:##.##} High Score: {2} Low Score: {3}",
                col + 1, avg, max, min);

            }

            //Exam #1 Average: 86.08 High Score: 99 Low Score: 35  
            //Exam #2 Average: 86.42 High Score: 94 Low Score: 72  
            //Exam #3 Average: 84.75 High Score: 91 Low Score: 65  
            //Exam #4 Average: 76.92 High Score: 94 Low Score: 39  
        }

        private static void SplitFileIntoManyFileUsingGroups()
        {
            // IOrderedEnumerable<IGrouping<char, string>>
            var namesInGroup = from name in _list1.Union(_list2)
                               group name by name[0] into newGroup
                               orderby newGroup.Key
                               select newGroup;
            //select new
            //{
            //    Key = newGroup.Key,
            //    Value = newGroup.OrderBy(x => x)
            //};

            foreach (var group in namesInGroup)
            {
                // Here you can save the result based on keys in multile files if you want
                Console.WriteLine(group.Key);
                foreach (var name in group)
                {
                    Console.WriteLine($"\t{name}");
                }
            }

        }
        private class Student
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public List<int> ExamScores;
        }
        private static void JoinContentFromDissimilarFiles()
        {
            //IEnumerable<string> studentScores = from student in _studentNames
            //                                    let studentDetail = student.Split(',')
            //                                    from score in _scores
            //                                    let scoreDetail = score.Split(',')
            //                                    where studentDetail[2].Trim().Equals(scoreDetail[0].Trim())
            //                                    orderby student[1]
            //                                    select studentDetail[1].Trim() + "," +
            //                                            scoreDetail[1].Trim() + "," +
            //                                            scoreDetail[2].Trim() + "," +
            //                                            scoreDetail[3].Trim() + "," +
            //                                            scoreDetail[4].Trim();


            IEnumerable<Student> studentScores = from student in _studentNames
                                                 let studentDetail = student.Split(',')
                                                 from score in _scores
                                                 let scoreDetail = score.Split(',')
                                                 where studentDetail[2].Trim().Equals(scoreDetail[0].Trim())
                                                 orderby student[1]
                                                 select new Student
                                                 {
                                                     FirstName = studentDetail[0].Trim(),
                                                     LastName = studentDetail[1].Trim(),
                                                     Id = Convert.ToInt32(studentDetail[2].Trim()),
                                                     ExamScores = (from scoreAsText in scoreDetail.Skip(1)
                                                                   select Convert.ToInt32(scoreAsText)).ToList()
                                                 };

            List<Student> students = studentScores.ToList();
            // Display each student's name and exam score average.
            foreach (var student in students)
            {
                Console.WriteLine("The average score of {0} {1} is {2}.",
                    student.FirstName, student.LastName,
                    student.ExamScores.Average());
            }
        }
        private static void ReorderFieldsOfADelimitedFile()
        {
            // Create the query. Put field 2 first, then  
            // reverse and combine fields 0 and 1 from the old field  
            IEnumerable<string> query = from line in _studentNames
                                        let values = line.Split(',')
                                        orderby values[2]
                                        select values[2].Trim() + ", " + values[1] + " " + values[0];
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }
        private static void Grouping()
        {
            string[] groupingQuery = { "carrots", "cabbage", "broccoli", "beans", "barley", "apple", "apricoat", "banana" };
            IEnumerable<IGrouping<char, string>> querySyntex = from item in groupingQuery
                                                               group item by item[0] into groupFruit
                                                               orderby groupFruit.Key
                                                               select groupFruit;

            var methodSyntex = groupingQuery.GroupBy(x => x[0]).OrderBy(x => x.Key).ToList();

            foreach (var item in querySyntex)
            {
                Console.WriteLine(item.Key.ToString().ToUpper());
                foreach (var value in item)
                {
                    Console.WriteLine("\tName: " + value);
                }
            }
        }

        /// <summary>
        /// Query for sentences that contain a specified set of words
        /// </summary>
        private static void FindSentenceContainSpecifiedSetOfWords()
        {
            string text = @"Historically, the world of data and the world of objects " +
                            @"have not been well integrated. Programmers work in C# or Visual Basic " +
                            @"and also in SQL or XQuery. On the one side are concepts such as classes, " +
                            @"objects, fields, inheritance, and .NET APIs. On the other side " +
                            @"are tables, columns, rows, nodes, and separate languages for dealing with " +
                            @"them. Data types often require translation between the two worlds; there are " +
                            @"different standard functions. Because the object world has no notion of query, a " +
                            @"query can only be represented as a string without compile-time type checking or " +
                            @"IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to " +
                            @"objects in memory is often tedious and error-prone.";

            // Split the text block into an array of sentences.
            string[] sentences = text.Split(new char[] { '.', '?', '!' });
            // Define the search terms. This list could also be dynamically populated at run time.  
            string[] wordsToMatch = { "Historically", "data", "integrated" };

            // Find sentences that contain all the terms in the wordsToMatch array. 
            var sentenceQuery = from sentence in sentences
                                let words = sentence.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                where words.Distinct().Intersect(wordsToMatch).Count() == wordsToMatch.Count()
                                select sentence;


            foreach (var sentence in sentenceQuery)
            {
                Console.WriteLine(sentence);
            }

            //Output: Historically, the world of data and the world of objects have not been well integrated
        }

        /// <summary>
        /// This example shows how to use LINQ to compare two lists of strings and 
        /// output those lines that are in names1.txt but not in names2.txt.
        /// </summary>
        private static void FindSetDifferenceBetweenTwoLists()
        {

            var differenceQuery = _list1.Except(_list2);
            // Execute the query.  
            Console.WriteLine("The following lines are in names1.txt but not names2.txt");

            foreach (string s in differenceQuery)
                Console.WriteLine(s);
        }

        /// <summary>
        /// The example shows how to perform a simple concatenation, a union, and an intersection on the two sets of text lines.
        /// </summary>
        private static void CombineAndCompareStringCollections()
        {
            //Simple concatenation and sort. Duplicates are preserved.
            var contactQuery = _list1.Concat(_list2).OrderBy(x => x);
            OutputQueryResults(contactQuery, "Simple concatenate and sort. Duplicates are preserved:");

            // Concatenate and remove duplicate names based on default string comparer.
            IEnumerable<string> uniqueNamesQuery = _list1.Union(_list2).OrderBy(x => x);
            OutputQueryResults(uniqueNamesQuery, "Union removes duplicate names:");

            // Find the names that occur in both files (based on default string comparer).
            IEnumerable<string> commonNamesQuery = _list1.Intersect(_list2).OrderBy(x => x);
            OutputQueryResults(commonNamesQuery, "Merge based on intersect:");

            // Find the matching fields in each list.
            // Merge the two results by using Concat,  
            // And then sort using the default string comparer.  
            string nameMatch = "Garcia";
            IEnumerable<string> query1 = from name in _list1
                                         let firstName = name.Split(',')[0]
                                         where firstName.Equals(nameMatch)
                                         orderby name
                                         select name;
            IEnumerable<string> query2 = from name in _list2
                                         let firstname = name.Split(',')[0]
                                         where firstname.Equals(nameMatch)
                                         orderby name
                                         select name;
            IEnumerable<string> mergedNames = query1.Concat(query2).OrderBy(x => x);

            OutputQueryResults(mergedNames, $"Concat based on partial name match \"{nameMatch}\":");

        }

        private static void OutputQueryResults(IEnumerable<string> query, string message)
        {
            Console.WriteLine(message + Environment.NewLine);
            foreach (string item in query)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(Environment.NewLine + "Total {0} names in list", query.Count());
        }
    }
}
