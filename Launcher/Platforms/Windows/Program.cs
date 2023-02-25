using StereoKit;
using System;

namespace NazAR
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // This will allow the Launcher constructor to call a few SK methods
            // before Initialize is called.
            SK.PreLoadLibrary();

            // If the launcher has a constructor that takes a string array, then
            // we'll use that, and pass the command line arguments into it on
            // creation
            Type appType = typeof(Launcher);
            Launcher launcher = appType.GetConstructor(new Type[] { typeof(string[]) }) != null
                ? (Launcher)Activator.CreateInstance(appType, new object[] { args })
                : (Launcher)Activator.CreateInstance(appType);
            if (launcher == null)
                throw new Exception("StereoKit loader couldn't construct an instance of the Launcher!");

            // Initialize StereoKit, and the launcher
            if (!SK.Initialize(launcher.Settings))
                Environment.Exit(1);
            launcher.Initialize();

            // Now loop until finished, and then shut down
            SK.Run(launcher.Step);
        }
    }
}