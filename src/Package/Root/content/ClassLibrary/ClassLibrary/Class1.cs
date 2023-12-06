using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
#if( AddNlog )
using NLog;
using NLog.Config;
using NLog.Targets;
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
        private static readonly LogFactory _ = NLog.LogManager.Setup(SetupBuilder => {
            SetupBuilder.LoadConfiguration(LoadConfigurationBuilder => {
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
                });
            });
        });
#pragma warning restore IDE0052 // Remove unread private members
        #endif

        /// <summary>
        /// 'Foo' is a simple method that returns a fixed string value. 
        /// This method can be used to demonstrate basic class functionality or for testing purposes.
        /// </summary>
        /// <returns>Returns a string "123".</returns>
        public static string Foo()
        {
            #if( AddNlog )
            logger.Info("call");
            #endif
            return "123";
        }
    }
}

