using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Digit2 : DigitBase
  {
    public override char Character => '2';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"-{3}"),
      new Gene(@"\s_[|]"),
      new Gene(@"[|]\s{2}"),
      new Gene(@"-{3}"),
    };
  }
}
