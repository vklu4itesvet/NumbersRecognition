using NumbersRecognizer.Interface;
using NumbersRecognizer.IO;
using System.Collections.Generic;
using System.Linq;

namespace NumbersRecognizer
{
  public class NumbersRecognizer
  {
    private readonly List<IDigit> _digits;

    private readonly INumbersDataSource _numbersRawDataSource;

    public NumbersRecognizer(INumbersDataSource numbersRawDataSource, IDigitsFactory digitsFactory)
    {
      _numbersRawDataSource = numbersRawDataSource;
      _digits = digitsFactory.CreateDigits().ToList();
    }

    public IEnumerable<int> Recognize()
    {
      while (_numbersRawDataSource.TryReadLine(out var l))
      {
        _digits.ForEach(n => n.Recognize(l));

        if (_digits.TrueForAll(n => n.Recognized.HasValue))
        {
          var number = GetNumber();
          _digits.ForEach(n => n.Reset());
          yield return number;
        }
      }
    }

    private int GetNumber()
    {
      var digitsWithIndexes = _digits.SelectMany(n => from i in n.RecognizedInIndexes select new { n.Character, Position = i });
      var digitsOrdered = from n in digitsWithIndexes orderby n.Position select n.Character;
      var digitChars = digitsOrdered.ToArray();
      return int.Parse(new string(digitChars));
    }
  }
}
