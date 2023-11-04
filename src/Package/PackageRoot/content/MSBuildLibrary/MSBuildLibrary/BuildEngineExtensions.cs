using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace MSBuildLibrary
{
    public static class BuildEngineExtensions
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

        public static IEnumerable<string> GetVariable(this IBuildEngine buildEngine, string key, bool throwIfNotFound)
        {
            var projectInstance = GetProjectInstance(buildEngine);

            List<ProjectItemInstance> items = projectInstance.Items.Where(x => string.Equals(x.ItemType, key, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (items.Count > 0)
            {
                return items.Select(x => x.EvaluatedInclude);
            }

            List<ProjectPropertyInstance> properties = projectInstance.Properties.Where(x => string.Equals(x.Name, key, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (properties.Count > 0)
            {
                return properties.Select(x => x.EvaluatedValue);
            }

            if (throwIfNotFound)
            {
                throw new Exception(string.Format("Could not extract from '{0}' environmental variables.", key));
            }

            return Enumerable.Empty<string>();
        }

        public static IEnumerable<ProjectItemInstance> GetProjectItems(this IBuildEngine buildEngine)
        {
            var projectInstance = GetProjectInstance(buildEngine);
            return projectInstance.Items;
        }

        public static IEnumerable<ProjectPropertyInstance> GetProjectProperties(this IBuildEngine buildEngine)
        {
            var projectInstance = GetProjectInstance(buildEngine);
            return projectInstance.Properties;
        }

        public static Dictionary<string, string> GetDicProjectItems(this IBuildEngine buildEngine)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var projectInstance = GetProjectInstance(buildEngine);
            projectInstance.Items.ToList().ForEach(e => { keyValuePairs.Add(e.ItemType, e.EvaluatedInclude); });
            return keyValuePairs;
        }

        public static Dictionary<string, string> GetDicProjectProperties(this IBuildEngine buildEngine)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var projectInstance = GetProjectInstance(buildEngine);
            projectInstance.Properties.ToList().ForEach(e => { keyValuePairs.Add(e.Name, e.EvaluatedValue); });
            return keyValuePairs;
        }

        static ProjectInstance GetProjectInstance(IBuildEngine buildEngine)
        {
            var buildEngineType = buildEngine.GetType();
            var targetBuilderCallbackField = buildEngineType.GetField("_targetBuilderCallback", bindingFlags) ?? throw new Exception("Could not extract _targetBuilderCallback from " + buildEngineType.FullName);
            var targetBuilderCallback = targetBuilderCallbackField.GetValue(buildEngine);
            var targetCallbackType = targetBuilderCallback.GetType();
            var projectInstanceField = targetCallbackType.GetField("_projectInstance", bindingFlags) ?? throw new Exception("Could not extract projectInstance from " + targetCallbackType.FullName);
            return (ProjectInstance)projectInstanceField.GetValue(targetBuilderCallback);
        }
    }
}
