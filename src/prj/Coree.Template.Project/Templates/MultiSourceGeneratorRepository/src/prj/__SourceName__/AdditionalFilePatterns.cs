#nullable disable
using System;

namespace __SourceName__
{
    internal static class AdditionalFilePatterns
    {
        internal static bool IsSelected(string path, string includes, string excludes, string projectDirectory)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(includes))
            {
                return false;
            }

            var relative = ToRelativePath(path, projectDirectory);
            if (!AnyMatch(relative, includes))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(excludes) && AnyMatch(relative, excludes))
            {
                return false;
            }

            return true;
        }

        internal static bool AnyMatch(string relativePath, string patterns)
        {
            if (string.IsNullOrWhiteSpace(relativePath) || string.IsNullOrWhiteSpace(patterns))
            {
                return false;
            }

            var parts = patterns.Split(new[] { ';', '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < parts.Length; i++)
            {
                var pattern = parts[i].Trim();
                if (pattern.Length == 0)
                {
                    continue;
                }

                if (IsMatch(relativePath, pattern))
                {
                    return true;
                }
            }

            return false;
        }

        internal static string ToRelativePath(string path, string projectDirectory)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            var normalizedPath = Normalize(path);
            if (string.IsNullOrWhiteSpace(projectDirectory))
            {
                return TrimDotSlash(normalizedPath);
            }

            var dir = Normalize(projectDirectory).TrimEnd('/');
            if (!normalizedPath.StartsWith(dir, StringComparison.OrdinalIgnoreCase))
            {
                return TrimDotSlash(normalizedPath);
            }

            if (normalizedPath.Length == dir.Length)
            {
                return string.Empty;
            }

            if (normalizedPath[dir.Length] != '/')
            {
                return TrimDotSlash(normalizedPath);
            }

            return normalizedPath.Substring(dir.Length + 1);
        }

        internal static bool IsMatch(string path, string pattern)
        {
            if (path == null || pattern == null)
            {
                return false;
            }

            return Match(Normalize(path), 0, Normalize(pattern), 0);
        }

        private static string Normalize(string value)
        {
            return value.Replace('\\', '/');
        }

        private static string TrimDotSlash(string path)
        {
            while (path.StartsWith("./", StringComparison.Ordinal))
            {
                path = path.Substring(2);
            }

            return path;
        }

        private static bool Match(string path, int pi, string pattern, int gi)
        {
            while (gi < pattern.Length)
            {
                if (pattern[gi] == '*' && gi + 1 < pattern.Length && pattern[gi + 1] == '*')
                {
                    var after = gi + 2;
                    if (after < pattern.Length && pattern[after] == '/')
                    {
                        after++;
                    }

                    if (after >= pattern.Length)
                    {
                        return true;
                    }

                    if (Match(path, pi, pattern, after))
                    {
                        return true;
                    }

                    for (var i = pi; i < path.Length; i++)
                    {
                        if (path[i] == '/' && Match(path, i + 1, pattern, gi))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                if (pattern[gi] == '*')
                {
                    gi++;
                    if (gi == pattern.Length)
                    {
                        return path.IndexOf('/', pi) < 0;
                    }

                    for (var i = pi; i <= path.Length; i++)
                    {
                        if (i > pi && path[i - 1] == '/')
                        {
                            break;
                        }

                        if (Match(path, i, pattern, gi))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                if (pattern[gi] == '?')
                {
                    if (pi >= path.Length || path[pi] == '/')
                    {
                        return false;
                    }

                    pi++;
                    gi++;
                    continue;
                }

                if (pi >= path.Length || !EqualsOrdinalIgnoreCase(path[pi], pattern[gi]))
                {
                    return false;
                }

                pi++;
                gi++;
            }

            return pi == path.Length;
        }

        private static bool EqualsOrdinalIgnoreCase(char a, char b)
        {
            if (a == b)
            {
                return true;
            }

            return char.ToUpperInvariant(a) == char.ToUpperInvariant(b);
        }
    }
}
