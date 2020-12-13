using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface IDigit
  {
    char Character { get; }

    IEnumerable<int> RecognizedInIndexes { get; }

    bool? Recognized { get; }

    void Recognize(string line);

    void Reset();
  }
}
