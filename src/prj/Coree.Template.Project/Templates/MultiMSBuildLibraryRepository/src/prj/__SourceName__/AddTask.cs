using Microsoft.Build.Framework;

namespace __SourceName__
{
    /// <summary>
    /// Sample MSBuild task: adds two integers. Replace this type with your own task.
    /// </summary>
    public class AddTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// First addend.
        /// </summary>
        [Required]
        public int Param1 { get; set; }

        /// <summary>
        /// Second addend.
        /// </summary>
        [Required]
        public int Param2 { get; set; }

        /// <summary>
        /// Sum of <see cref="Param1"/> and <see cref="Param2"/>.
        /// </summary>
        [Output]
        public int AddResult { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            AddResult = Param1 + Param2;
            return true;
        }
    }
}
