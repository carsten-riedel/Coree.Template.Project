using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace __SourceName__
{
    /// <summary>
    /// Returns a sample value so the generated module can be built, imported, tested, and debugged immediately.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "SampleValue")]
    [OutputType(typeof(string))]
    [ExcludeFromCodeCoverage]
    public sealed class GetSampleValueCommand : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the value returned by the command.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public string InputObject { get; set; } = string.Empty;

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            WriteObject(SampleValueFormatter.Format(InputObject));
        }
    }
}
