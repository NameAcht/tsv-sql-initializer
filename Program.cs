using System;
using System.IO;

namespace txtTablesToSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filePath in Directory.GetFiles("../"))
            {
                if (!filePath.Contains(".txt"))
                    continue;
                var lines = File.ReadAllLines(filePath);
                string newPath = filePath.Substring(0, filePath.Length - 3) + "sql";
                File.Create(newPath).Close();
                var write = new StreamWriter(newPath, false);
                write.Write("INSERT INTO " + filePath.Substring(3, filePath.Length - 7) + "\nVALUES");
                for (int lineI = 0; lineI < lines.Length; lineI++)
                {
                    if (lineI == 0)
                        continue;
                    write.Write("\n" + "(");
                    var line = lines[lineI];
                    var splitLine = line.Split('\t');
                    
                    for (int colI = 0; colI < splitLine.Length; colI++)
                    {
                        var item = splitLine[colI];
                        if (int.TryParse(item, out int parse))
                            write.Write(item);
                        else if (item.Length > 0 && !item.Contains("'"))
                            write.Write("'" + item + "'");
                        else if(!item.Contains("'"))
                            write.Write("NULL");
                        else
                        {
                            string newItem = "'";
                            foreach (var c in item)
                                if (c != '\'')
                                    newItem += c;
                                else
                                    newItem += "''";
                            newItem += "'";
                            write.Write(newItem);
                        }    
                        if (colI < splitLine.Length - 1)
                            write.Write(", ");
                        else if (lineI < lines.Length - 1)
                            write.Write("),");
                        else
                            write.Write(");");
                    }
                }
                write.Close();
            }
            Console.ReadLine();
        }
    }
}