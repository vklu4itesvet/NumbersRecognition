namespace NumbersRecognizer.IO
{
  public interface INumbersDataSource
  {
    bool TryReadLine(out string line);
  }
}
