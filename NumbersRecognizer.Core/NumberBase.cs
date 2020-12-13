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

    private int _handledLinesCount = 0;

    #endregion

    protected NumberBase()
    {
      _dna = GetGenes();
      _genumerator = GetNextDnaPart().GetEnumerator();
      MoveNextMatcher();
      RecognizedCharIndexes = Enumerable.Empty<int>();
    }

    #region Properties

    public abstract char Character { get; }

    public IEnumerable<int> RecognizedCharIndexes { get; private set; }

    public bool? Recognized { get; protected set; }

    protected GenMatcher CurrentMatcher => _genumerator.Current;

    #endregion

    #region Methods

    public void Recognize(string line)
    {
      if (Recognized.HasValue)
        return;

      _handledLinesCount++;
      var foundGenes = Tokenize(line);
      Analyze();

      if (Recognized.HasValue)
        return;

      if (!foundGenes)// If no matches with current gene - lets try found matches with next gene
      {
        MoveNextMatcher();
        Tokenize(line);
        Analyze();
      }

      if (CurrentMatcher.Gene.Repeats == 1)
        MoveNextMatcher();//If current gene should not repeat, take next for matching next line
    }

    public void Reset()
    {
      Recognized = null;
      _matches.Clear();
      _handledLinesCount = 0;
      RecognizedCharIndexes = Enumerable.Empty<int>(); ;
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
      {
        Recognized = false;
        return;
      }

      var currentGen = CurrentMatcher.GeneIndex;
      var maxGen = _dna.Sum(g => g.Repeats);
      var matchesForAllGenes = _matches.GroupBy(m => m.Position).Where(g => g.Any(m => m.DnaRate == currentGen) && g.Count() == maxGen);

      if (_handledLinesCount == maxGen)
        Recognized = matchesForAllGenes.Any();

      if (Recognized == true)
        RecognizedCharIndexes = from m in matchesForAllGenes select m.Key;
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
