using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Digit4 : DigitBase
  {
    public override char Character => '4';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"[|]\s{3}[|]"),
      new Gene(@"[|]_{3}[|]"),
      new Gene(@"\s{4}[|]", repeats: 2),
    };
  }
}
