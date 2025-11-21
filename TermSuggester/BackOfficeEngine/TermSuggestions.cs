using TermSuggester.BackOfficeEngine.Interfaces;

namespace TermSuggester.BackOfficeEngine
{
    public class TermSuggestions : IAmTheTest
    {
        public IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions)
        {
            // Sanitize input term (lowercase, alphanumeric only)
            term = Sanitize(term);
            var normalizedCorpus = choices.Select(Sanitize).ToList();

            // Select terms that contain the searched term directly
            var exactMatches = normalizedCorpus.Where(x => x.Contains(term)).ToList();

            
            // Select terms by computing their difference score            
            var distanceMatches = normalizedCorpus.Select(x => new
                                                     {
                                                         Word = x,
                                                         Score = GetDifferenceScore(x, term),
                                                         Length = x.Length
                                                     })
                                                     .Where(x => x.Score != int.MaxValue)     // eliminate go, ros, gro
                                                     .Where(x => x.Score < term.Length)       // filters words that are too different
                                                     .OrderBy(x => x.Score)                   // score
                                                     .ThenBy(x => x.Length)                   // shorter words first
                                                     .ThenBy(x => x.Word)                     // alphabetical
                                                     .ToList();

            var results = new List<string>();
            // Add exact matches first, then distance-based ones
            results.AddRange(exactMatches);
            results.AddRange(distanceMatches.Select(x => x.Word));

            return results.Distinct().Take(numberOfSuggestions);
        }

        /// <summary>
        /// Computes how many character replacements are needed between two strings
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int GetDifferenceScore(string dest, string src)
        {
            int termLength = src.Length;

            // Too short => not comparable
            if (dest.Length < termLength)
                return int.MaxValue;

            int bestScore = int.MaxValue;

            // Slide on all substrings of length equal to the term
            for (int i = 0; i <= dest.Length - termLength; i++)
            {
                int diff = 0;

                for (int j = 0; j < termLength; j++)
                {
                    if (dest[i + j] != src[j])
                        diff++;
                }

                if (diff < bestScore)
                    bestScore = diff;
            }

            return bestScore;
        }

        /// <summary>
        /// Converts a string into lowercase alphanumeric only
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
