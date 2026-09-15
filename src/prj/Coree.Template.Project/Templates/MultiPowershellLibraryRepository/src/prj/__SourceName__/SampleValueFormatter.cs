namespace __SourceName__
{
    /// <summary>
    /// Contains the host-independent behavior used by <see cref="GetSampleValueCommand"/>.
    /// </summary>
    public static class SampleValueFormatter
    {
        /// <summary>
        /// Formats the value returned by the sample cmdlet.
        /// </summary>
        /// <param name="value">Value supplied by the caller.</param>
        /// <returns>The formatted value.</returns>
        public static string Format(string value)
        {
            return $"Value: {value}";
        }
    }
}
