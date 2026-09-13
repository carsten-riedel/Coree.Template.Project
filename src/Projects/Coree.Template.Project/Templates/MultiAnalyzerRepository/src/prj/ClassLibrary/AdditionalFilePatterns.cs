#nullable disable
using System;
using System.IO;

namespace ClassLibrary
{
    internal static class AdditionalFilePatterns
    {
        internal static bool Matches(string path, string patterns)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(patterns))
            {
                return false;
            }

            var name = Path.GetFileName(path);
            var parts = patterns.Split(new[] { ';', '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < parts.Length; i++)
            {
                var pattern = parts[i].Trim();
                if (pattern.Length == 0)
                {
                    continue;
                }

                if (MatchesOne(name, pattern))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool MatchesOne(string fileName, string pattern)
        {
            if (pattern == "*" || pattern == "*.*")
            {
                return true;
            }

            if (pattern.Length >= 2 && pattern[0] == '*' && pattern[1] == '.')
            {
                return fileName.EndsWith(pattern.Substring(1), StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(fileName, pattern, StringComparison.OrdinalIgnoreCase);
        }
    }
}
