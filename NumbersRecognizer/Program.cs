using NumbersRecognizer.Core;
using NumbersRecognizer.IO;
using System;

namespace NumbersRecognizer.Host
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var numbersDataSource = new NumbersDataFileSource())
      {
        Console.WriteLine($"Start parsing of {numbersDataSource.GetDataSourcePath} ...");
        Console.WriteLine($"file content:{Environment.NewLine}");

        while (numbersDataSource.TryReadLine(out var line))
          Console.WriteLine(line);

        Console.WriteLine($"{Environment.NewLine}found numbers:{Environment.NewLine}");

        numbersDataSource.Reset();

        var numbersRecognizer = new NumbersRecognizer(numbersDataSource, new NumbersFactory());
        foreach (var n in numbersRecognizer.Recognize())
          Console.WriteLine(n);
      }

      Console.WriteLine($"{Environment.NewLine}Press any key for exit");
      Console.ReadKey();
    }
  }
}
