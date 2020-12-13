using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Digit5 : DigitBase
  {
    public override char Character => '5';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"-{5}"),
      new Gene(@"[|]_{3}"),
      new Gene(@"\s{4}[|]"),
      new Gene(@"_{4}[|]"),
    };
  }
}
