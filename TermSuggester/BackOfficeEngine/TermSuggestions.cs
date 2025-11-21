using TermSuggester.BackOfficeEngine.Interfaces;

namespace TermSuggester.BackOfficeEngine
{
    public class TermSuggestions : IAmTheTest
    {
        public IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions)
        {
            term = Sanitize(term);
            var normalizedCorpus = choices.Select(Sanitize).ToList();

            var exactMatches = normalizedCorpus.Where(x => x.Contains(term)).ToList();

            // distance-based matches only for equal-length terms
            var distanceMatches = normalizedCorpus
            .Where(x => x.Length == term.Length)
            .Select(x => new { Word = x, Score = GetDifferenceScore(x, term) })
            .OrderBy(x => x.Score)
            .ThenBy(x => Math.Abs(x.Word.Length - term.Length))
            .ThenBy(x => x.Word)
            .ToList();

            var results = new List<string>();
            results.AddRange(exactMatches);
            results.AddRange(distanceMatches.Select(x => x.Word));

            return results.Distinct().Take(numberOfSuggestions);
        }

        public int GetDifferenceScore(string dest, string src)
        {
            if (dest.Length != src.Length)
                throw new ArgumentException("Strings must have same length");

            int diff = 0;
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != dest[i]) 
                    diff++;
            }
            return diff;
        }

        private string Sanitize(string input)
        {
            var clean = new string(input
            .Where(char.IsLetterOrDigit)
            .Select(char.ToLower)
            .ToArray());
            return clean;
        }
    }
}
