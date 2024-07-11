![SharperHacks logo](SHLLC-Logo.jpg)
# AppUtilities Library for .NET
## SharperHacks.CoreLibs.AppUtilites.

Some useful bits for writing console apps.

Licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE).

Contact: joseph@sharperhacks.org

Nuget: https://www.nuget.org/packages/SharperHacks.CoreLibs.AppUtilities

### Targets
- net8.0
- net9.0

### Classes

#### AppConfig

```
/// Get the product name.
public static string ProductName { get; }

/// Get the json settings file name.
public static string JsonAppSettingsFileName => $"{ProductName}.appsettings.json";

/// Get the IConfiguration instance.
public static IConfiguration Configuration { get; 

/// Get {ProductName}:Logging:LogLevel:Console, or default LogEvenLevel.
public static LogLevel DefaultConsoleLogEventLevel { get; }

/// Get {ProductName}:Logging:LogLevel:File if it exists, or default LogEvenLevel.
public static LogLevel DefaultFileLogEventLevel { get; }

/// Get {ProductName}:Logging:LogPath:Directory if not null, or return the
/// LocalApplicationData special folder.
public static string LogDirectory { get; }

/// Get {ProductName}:Logging:FilePrefix if not null, or return <see cref="AppConfig.ProductName"/>
public static string LogFilePrefix { get; }

/// Get {ProductName}:Logging:FilePostfix if not null, or return ".txt".
public static string LogFilePostfix { get; }

/// Get the production log path for the calling app. Constructed from <see cref="LogDirectory"/>
public static string ProductionLogPath { get; }

/// Get whether trace events should be logged.
public static bool TraceEnabled { get; }

```