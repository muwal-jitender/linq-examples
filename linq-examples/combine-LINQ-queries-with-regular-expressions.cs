using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace linq_examples
{
    internal class Combine_LINQ_queries_with_regular_expressions
    {
        public static void Call()
        {
            PrintResult();
        }

        static void PrintResult()
        {
            // Modify this path as necessary so that it accesses your version of Visual Studio.  
            string startFolder = @"C:\Program Files\Microsoft Visual Studio\2022\Community";
            // Take a snapshot of the file system.  
            IEnumerable<System.IO.FileInfo> fileList = GetFiles(startFolder);

            // Create the regular expression to find all things "Visual". 
            Regex searchTerm = new(@"Visual (Basic|C#|C\+\+|Studio)");

            var queryMatchingFiles = from file in fileList
                                     where file.Extension == ".htm"
                                     let fileText = File.ReadAllText(file.FullName)
                                     let matches = searchTerm.Matches(fileText)
                                     where matches.Count() > 0
                                     select new
                                     {
                                         Name = file.FullName,
                                         MatchedValue = from match in matches
                                                        select match.Value
                                     };

            // Execute the query.  
            Console.WriteLine("The term \"{0}\" was found in:", searchTerm.ToString());
            foreach (var v in queryMatchingFiles)
            {
                string s = v.Name.Substring(startFolder.Length - 1);
                Console.WriteLine(s);
                foreach (var v2 in v.MatchedValue)
                {
                    Console.WriteLine("  " + v2);
                }
            }
        }
        static IEnumerable<FileInfo> GetFiles(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();

            string[] fileNames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            List<FileInfo> files = new();
            foreach (var file in fileNames)
            {
                files.Add(new FileInfo(file));
            }
            return files;
        }
    }
}
