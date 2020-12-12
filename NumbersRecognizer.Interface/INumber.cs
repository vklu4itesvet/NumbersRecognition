namespace NumbersRecognizer.Interface
{
  public interface INumber
  {
    char Character { get; }

    bool? Recognized { get; }

    void Recognize(string line);

    void Reset();
  }
}
