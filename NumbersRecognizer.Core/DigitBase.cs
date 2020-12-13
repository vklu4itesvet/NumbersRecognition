using NumbersRecognizer.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumbersRecognizer.Core
{
  public abstract class DigitBase : IDigit
  {
    #region Fileds

    private readonly IList<DnaMatch> _matches = new List<DnaMatch>();

    private IList<Gene> _dna;

    private IEnumerator<GenMatcher> _genumerator;

    private bool? _recognized;

    private int _handledLinesCount = 0;

    #endregion

    protected DigitBase()
    {
      _dna = GetGenes();
      InitMatcher();
      RecognizedInIndexes = Enumerable.Empty<int>();
    }

    #region Properties

    public abstract char Character { get; }

    public IEnumerable<int> RecognizedInIndexes { get; private set; }

    public bool DidRecognition => _recognized.HasValue;

    private GenMatcher CurrentMatcher => _genumerator.Current;

    #endregion

    #region Methods

    public void Recognize(string line)
    {
      if (DidRecognition)
        return;

      _handledLinesCount++;
      var foundGenes = Tokenize(line);
      Analyze();

      if (DidRecognition)
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
      _recognized = null;
      _matches.Clear();
      _handledLinesCount = 0;
      RecognizedInIndexes = Enumerable.Empty<int>();
      InitMatcher();
    }

    protected abstract IList<Gene> GetGenes();

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
        _recognized = false;
        return;
      }

      var currentGen = CurrentMatcher.GeneIndex;
      var maxGen = _dna.Sum(g => g.Repeats);
      var matchesForAllGenes = _matches.GroupBy(m => m.Position).Where(g => g.Any(m => m.DnaRate == currentGen) && g.Count() == maxGen);

      if (_handledLinesCount == maxGen)
        _recognized = matchesForAllGenes.Any();

      if (_recognized == true)
        RecognizedInIndexes = from m in matchesForAllGenes select m.Key;
    }

    private IEnumerable<GenMatcher> GetGenMatchers()
    {
      for (var i = 0; i < _dna.Count; i++)
        yield return new GenMatcher { Gene = _dna[i], GeneIndex = i };
    }

    private bool MoveNextMatcher() => _genumerator.MoveNext();

    private void InitMatcher()
    {
      _genumerator = GetGenMatchers().GetEnumerator();
      MoveNextMatcher();
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

    private struct GenMatcher
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
