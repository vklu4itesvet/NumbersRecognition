using System.Collections.Generic;

namespace NumbersRecognizer.Core
{
  internal class Number3 : NumberBase
  {
    public override char Character => '3';

    protected override IList<Gene> GetGenes() => new List<Gene>
    {
      new Gene(@"-{3}"),
      new Gene(@"\s[//]\s"),
      new Gene(@"\s[\\]\s"),
      new Gene(@"-{2}\s"),
    };
  }
}
