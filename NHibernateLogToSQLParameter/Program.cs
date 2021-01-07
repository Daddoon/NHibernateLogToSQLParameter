using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateLogToSQLParameter
{
    class Program
    {
        private static Dictionary<string, string> TypeList = new Dictionary<string, string>()
        {
            { "[Type: Guid (0:0:0)]", "UNIQUEIDENTIFIER" },
            { "[Type: String (4000:0:0)]", "NVARCHAR(4000)" },
            { "[Type: Boolean (0:0:0)]", "BIT" },
            { "[Type: Int32 (0:0:0)]", "INT" }
        };

        /// <summary>
        /// Key is input type,
        /// Value is the SQL type
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static bool FindOldAndNewTypes(string input, out KeyValuePair<string, string> result)
        {
            result = default;

            if (TypeList.Any(p => input.Contains(p.Key)))
            {
                result = TypeList.FirstOrDefault(p => input.Contains(p.Key));
                return true;
            }

            return false;
        }

        private static string[] LineParser(string[] sourceLines)
        {
            List<string> lineList = new List<string>();

            foreach (string line in sourceLines)
            {
                if (!FindOldAndNewTypes(line, out KeyValuePair<string, string> foundTypes))
                {
                    //Do not convert
                    lineList.Add(line);
                    continue;
                }

                try
                {
                    string newLine = line;
                    newLine = line.TrimStart().TrimEnd(',');

                    newLine = newLine.Replace(foundTypes.Key, string.Empty).TrimEnd();

                    //Specific case for uniqueidentifier
                    if (foundTypes.Value == "UNIQUEIDENTIFIER")
                    {
                        newLine = newLine.Replace("= ", "= '");
                        newLine += "'"; //should quotes identifiers
                    }

                    //Specific case for Boolean
                    if (foundTypes.Value == "BIT")
                    {
                        newLine = newLine.Replace("False", "0").Replace("True", "1");
                    }

                    newLine = "DECLARE " + newLine.Replace("=", foundTypes.Value + " =") + ";";

                    lineList.Add(newLine);
                }
                catch (Exception)
                {
                }
            }

            return lineList.ToArray();
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough params. First argument should be a file with a NHibernate parameter list, and the second parameter should be the output file with translated code");
            }

            string inputFile = args[0];
            string outputFile = args[1];

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"{inputFile} does not exist");
                return;
            }

            string[] allLines = File.ReadAllLines(inputFile);
            string[] outputLines = LineParser(allLines);

            File.WriteAllLines(outputFile, outputLines);
        }
    }
}
