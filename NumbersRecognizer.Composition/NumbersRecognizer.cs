using NumbersRecognizer.Interface;
using NumbersRecognizer.IO;
using System.Collections.Generic;
using System.Linq;

namespace NumbersRecognizer
{
  public class NumbersRecognizer
  {
    private readonly List<INumber> _numbers;

    private readonly INumbersDataSource _numbersRawDataSource;

    public NumbersRecognizer(INumbersDataSource numbersRawDataSource, INumbersFactory numbersFactory)
    {
      _numbersRawDataSource = numbersRawDataSource;
      _numbers = numbersFactory.CreateNumbers().ToList();
    }

    public IEnumerable<string> Recognize()
    {
      while (_numbersRawDataSource.TryReadLine(out var l))
      {
        _numbers.ForEach(n => n.Recognize(l));

        if (_numbers.TrueForAll(n => n.Recognized.HasValue))
        {
          var number = GetNumber();
          _numbers.ForEach(n => n.Reset());
          //If we are here - all number recognizers made decision if they found smth, 
          //but also need to check if real number was found.
          if(number.Any())
            yield return new string(number);
        }
      }
    }

    private char[] GetNumber()
    {
      var numbersWithPositions = _numbers.SelectMany(n => from i in n.RecognizedCharIndexes select new { Character = n.Character, Position = i });
      var numbersOrdered = from n in numbersWithPositions orderby n.Position select n.Character;
      return numbersOrdered.ToArray();
    }
  }
}
