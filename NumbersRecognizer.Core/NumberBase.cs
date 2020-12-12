using NumbersRecognizer.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumbersRecognizer.Core
{
  public abstract class NumberBase : INumber
  {
    #region Fileds

    private readonly IList<DnaMatch> _matches = new List<DnaMatch>();

    private IList<Gene> _dna;

    private IEnumerator<GenMatcher> _genumerator;

    #endregion

    protected NumberBase()
    {
      _dna = GetGenes();
      _genumerator = GetNextDnaPart().GetEnumerator();
      MoveNextMatcher();
    }

    #region Properties

    public abstract char Character { get; }

    public IEnumerable<int> RecognizedCharIndexes
    {
      get
      {
        //if (Recognized != true)
        //  throw new InvalidOperationException("No chars recognized at the moment.");

        return from m in _matches select m.Position;
      }
    }

    public bool? Recognized { get; protected set; }

    protected GenMatcher CurrentMatcher => _genumerator.Current;

    #endregion

    #region Methods

    public void Recognize(string line)
    {
      if (Recognized == false)
        return;

      var foundGenes = Tokenize(line);
      Analyze();

      if (!foundGenes && Recognized != false)
      {
        MoveNextMatcher();
        Tokenize(line);
        Analyze();
      }

      if (CurrentMatcher.Gene.Repeats == 1)
      {
        var gen = CurrentMatcher.GeneIndex + 1;
        var notFinished = MoveNextMatcher();
      }
    }

    public void Reset()
    {
      Recognized = null;
      _matches.Clear();
      _genumerator = GetNextDnaPart().GetEnumerator();
      MoveNextMatcher();
    }

    protected abstract IList<Gene> GetGenes();

    protected IEnumerable<GenMatcher> GetNextDnaPart()
    {
      for (var i = 0; i < _dna.Count; i++)
        yield return new GenMatcher { Gene = _dna[i], GeneIndex = i };
    }

    protected bool MoveNextMatcher() => _genumerator.MoveNext();

    private bool Tokenize(string line)
    {
      var dna = CurrentMatcher;
      var matches = dna.Gene.Matcher.Matches(line);

      if (!matches.Any())
        return false;

      foreach (Match m in matches)
        _matches.Add(new DnaMatch { Position = m.Index, DnaRate = dna.GeneIndex });

      return true;
    }

    private void Analyze()
    {
      if (!_matches.Any())
        Recognized = false;

      var currentGen = CurrentMatcher.GeneIndex;
      var maxGen = _dna.Count - 1;

      //var matchesStale = (from m in _matches where m.DnaRate < currentGen select m).ToList();
      //foreach (var m in matchesStale)
      //  _matches.Remove(m);

      var hasMatchesInCurrentGen = _matches.GroupBy(m => m.Position).Any(g => g.Any(m => m.DnaRate == currentGen));
      if (!hasMatchesInCurrentGen)
        Recognized = false;
      else if (currentGen == maxGen)
        Recognized = hasMatchesInCurrentGen;
    }

    #endregion

    #region Structs 

    protected struct Gene
    {
      public Gene(string code, byte repeats = 1)
      {
        Matcher = new Regex(code, RegexOptions.Compiled);
        Repeats = repeats;
      }

      public Regex Matcher { get; private set; }

      public byte Repeats { get; set; }
    }

    protected struct GenMatcher
    {
      public Gene Gene { get; set; }

      public int GeneIndex { get; set; }
    }

    private struct DnaMatch
    {
      public int Position { get; set; }

      public int DnaRate { get; set; }
    }

    #endregion
  }
}
