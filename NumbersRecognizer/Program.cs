using NumbersRecognizer.Core;
using NumbersRecognizer.IO;
using System;

namespace NumbersRecognizer.Host
{
  class Program
  {
    static void Main(string[] args)
    {
      var lineBreak = Environment.NewLine;
      using (var numbersDataSource = new NumbersDataFileSource())
      {
        Console.WriteLine($"Start parsing of {numbersDataSource.GetDataSourcePath} ...{lineBreak}");
        Console.WriteLine($"file content:{lineBreak}");

        while (numbersDataSource.TryReadLine(out var line))
          Console.WriteLine(line);

        Console.WriteLine($"{lineBreak}found numbers:{lineBreak}");

        numbersDataSource.Reset();

        var numbersRecognizer = new NumbersRecognizer(numbersDataSource, new DigitsFactory());
        foreach (var n in numbersRecognizer.Recognize())
          Console.WriteLine(n);
      }

      Console.WriteLine($"{lineBreak}Press any key for exit");
      Console.ReadKey();
    }
  }
}
