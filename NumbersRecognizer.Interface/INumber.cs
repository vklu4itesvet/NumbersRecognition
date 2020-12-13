using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface INumber
  {
    char Character { get; }

    bool? Recognized { get; }

    IEnumerable<int> FoundInIndexes { get; }

    void Recognize(string line);

    void Reset();
  }
}
