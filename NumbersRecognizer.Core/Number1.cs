using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Number1 : NumberBase
  {
    public override char Character => '1';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"[|]", repeats: 4),
    };
  }
}
