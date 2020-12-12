using System.Collections.Generic;

namespace NumbersRecognizer.Interface
{
  public interface INumbersFactory
  {
    IEnumerable<INumber> CreateNumbers(); 
  }
}
