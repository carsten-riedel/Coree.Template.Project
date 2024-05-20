using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
#if( AddNlog )
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Layouts;
#endif

namespace ClassLibrary
{
    /// <summary>
    /// The 'Class1' provides foundational functionalities for the library.
    /// It is a static class, meaning it cannot be instantiated and its members are accessed at the class level.
    /// </summary>
    public static class Class1
    {
        #if( AddNlog )
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1823 // Unused field _
        private static readonly LogFactory _ = NLog.LogManager.Setup(SetupBuilder =>
        {
            SetupBuilder.LoadConfiguration(LoadConfigurationBuilder =>
            {
                LoadConfigurationBuilder.Configuration = new LoggingConfiguration();
                LoadConfigurationBuilder.Configuration.AddRuleForAllLevels(new ConsoleTarget() { Name = "Console" });
                LoadConfigurationBuilder.Configuration.AddRuleForAllLevels(new FileTarget()
                {
                    Name = "File",
                    ConcurrentWrites = true,
                    MaxArchiveFiles = 24,
                    FileName = "${processname}.log",
                    KeepFileOpen = true,
                    ArchiveFileName = "archive\\${processname}.{#}.log",
                    ArchiveNumbering = ArchiveNumberingMode.Date,
                    ArchiveEvery = FileArchivePeriod.Day,
                    ArchiveOldFileOnStartup = true,
                    Layout = new JsonLayout
                    {
                        Attributes =
                        {
                            new JsonAttribute("time", "${longdate}"),
                            new JsonAttribute("level", "${level}"),
                            new JsonAttribute("hostname", "${hostname}"),
                            new JsonAttribute("environment-user", "${environment-user}"),
                            new JsonAttribute("message", "${message}"),
                            new JsonAttribute("properties", new JsonLayout { IncludeEventProperties = true, MaxRecursionLimit = 2}, encode: false),
                            new JsonAttribute("exception", new JsonLayout
                            {
                                Attributes =
                                {
                                    new JsonAttribute("type", "${exception:format=type}"),
                                    new JsonAttribute("message", "${exception:format=message}"),
                                    new JsonAttribute("stacktrace", "${exception:format=tostring}"),
                                }
                            },
                            encode: false) // don't escape layout
                        }
                    }
                });
            });
        });
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore CA1823 // Unused field _
        #endif

        /// <summary>
        /// 'Foo' is a simple method that returns a fixed string value. 
        /// This method can be used to demonstrate basic class functionality or for testing purposes.
        /// </summary>
        /// <returns>Returns a string "123".</returns>
        public static string Foo()
        {
            #if( AddNlog )
            logger.Info("{shopitem} added to basket by {user}", new { Id = 6, Name = "Jacket", Color = "Orange" }, "Kenny");
            #endif
            return "123";
        }
    }
}

