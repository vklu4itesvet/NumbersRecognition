using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface IDigitsFactory
  {
    IEnumerable<IDigit> CreateDigits(); 
  }
}
