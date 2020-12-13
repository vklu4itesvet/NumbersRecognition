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

    public IEnumerable<int> Recognize()
    {
      while (_numbersRawDataSource.TryReadLine(out var l))
      {
        _numbers.ForEach(n => n.Recognize(l));

        if (_numbers.TrueForAll(n => n.Recognized.HasValue))
        {
          var number = GetNumber();
          _numbers.ForEach(n => n.Reset());
          yield return number;
        }
      }
    }

    private int GetNumber()
    {
      var numbersWithPositions = _numbers.SelectMany(n => from i in n.RecognizedCharIndexes select new { n.Character, Position = i });
      var numbersOrdered = from n in numbersWithPositions orderby n.Position select n.Character;
      var numberChars = numbersOrdered.ToArray();
      return int.Parse(new string(numberChars));
    }
  }
}
