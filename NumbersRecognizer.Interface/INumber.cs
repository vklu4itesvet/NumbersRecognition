using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface INumber
  {
    char Character { get; }

    bool? Recognized { get; }

    IEnumerable<int> RecognizedCharIndexes { get; }

    void Recognize(string line);

    void Reset();
  }
}
