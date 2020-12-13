using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface IDigit
  {
    char Character { get; }

    IEnumerable<int> RecognizedInIndexes { get; }

    bool DidRecognition { get; }

    void Recognize(string line);

    void Reset();
  }
}
