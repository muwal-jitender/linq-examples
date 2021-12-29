using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linq_examples
{
    internal class RandomExample
    {
        public static void Grouping()
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
    }
}
