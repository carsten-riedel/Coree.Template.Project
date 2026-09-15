using Microsoft.Build.Framework;

namespace __SourceName__
{
    /// <summary>
    /// Sample MSBuild task: reports the file that loaded this task (<see cref="IBuildEngine.ProjectFileOfTaskNode"/>).
    /// Replace this type with your own task, or keep it as a host-path example.
    /// </summary>
    public class TaskNodeTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Directory of <see cref="TaskNodeFile"/>.
        /// </summary>
        [Output]
        public string? TaskNodeDir { get; set; }

        /// <summary>
        /// Full path of the file that invoked this task (typically the consumer props or targets file).
        /// </summary>
        [Output]
        public string? TaskNodeFile { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            TaskNodeFile = BuildEngine.ProjectFileOfTaskNode;
            TaskNodeDir = System.IO.Path.GetDirectoryName(TaskNodeFile);
            return true;
        }
    }
}
