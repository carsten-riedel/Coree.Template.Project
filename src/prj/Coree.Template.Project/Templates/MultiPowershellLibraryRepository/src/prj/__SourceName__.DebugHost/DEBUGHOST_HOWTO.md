# DebugHost

This project must be the **startup project**. F5 builds and stages the module, starts the real host, then imports that manifest. Breakpoints go on the module cmdlet (`GetSampleValueCommand.ProcessRecord`), not in this empty launcher.

<!--#if (SupportsPowerShellCore) -->
Use the **PowerShell 7** profile. DebugHost matches `pwsh.exe` (Core). A gray breakpoint that “will not be hit” usually means **Windows PowerShell 5.1** is selected: that starts `powershell.exe` (.NET Framework), and the Core debugger does not bind those symbols.
<!--#else -->
F5 launches `powershell.exe`. DebugHost matches that Framework TFM, so cmdlet breakpoints bind in this profile.
<!--#endif -->
