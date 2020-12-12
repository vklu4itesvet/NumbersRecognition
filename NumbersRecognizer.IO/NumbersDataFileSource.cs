using System;
using System.IO;
using System.Reflection;

namespace NumbersRecognizer.IO
{
  public class NumbersDataFileSource : INumbersDataSource, IDisposable
  {
    private readonly StreamReader _fileReader;

    public NumbersDataFileSource() => _fileReader = new StreamReader(GetDataSourcePath.OriginalString);

    ~NumbersDataFileSource() => Dispose(false);

    public Uri GetDataSourcePath => new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NumberParserExtended.txt"));

    public bool TryReadLine(out string line)
    {
      line = _fileReader.ReadLine();
      return line != null;
    }

    public void Reset() => _fileReader.BaseStream.Position = 0;

    public void Dispose() => Dispose(true);

    private void Dispose(bool _) => _fileReader.Dispose();
  }
}
