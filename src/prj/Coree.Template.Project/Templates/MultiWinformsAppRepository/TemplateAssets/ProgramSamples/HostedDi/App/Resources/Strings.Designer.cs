#nullable enable

using System.Globalization;
using System.Resources;

namespace __SourceName__.Resources
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager = new(
            "__SourceName__.Resources.Strings",
            typeof(Strings).Assembly);

        internal static CultureInfo? Culture { get; set; }

        internal static string DefaultWindowTitle =>
            _resourceManager.GetString(nameof(DefaultWindowTitle), Culture) ?? "MainForm";

        internal static string MultipleInstancesTitle =>
            _resourceManager.GetString(nameof(MultipleInstancesTitle), Culture) ?? "Multiple instances are not supported.";

        internal static string MultipleInstancesMessage =>
            _resourceManager.GetString(nameof(MultipleInstancesMessage), Culture) ?? "{0} is already running!";
    }
}
