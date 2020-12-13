using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Digit1 : DigitBase
  {
    public override char Character => '1';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"\s??[|]\s??", repeats: 4),
    };
  }
}
