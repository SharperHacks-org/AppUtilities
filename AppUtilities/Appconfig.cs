// Copyright and trademark notices at the end of this file.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SharperHacks.CoreLibs.Constraints;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharperHacks.CoreLibs.AppUtilities;

/// <summary>
/// Wires up the appconfig data we need to initialize the app.
/// </summary>
public static class AppConfig
{
    /// <summary>
    /// Get the company name that created teh calling assembly.
    /// </summary>
    public static string CompanyName { get; }

    /// <summary>
    /// Get the IConfiguration instance.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <remarks>
    /// Other members of this class provide shorthand access to some, but not all config data.
    /// The IConfiguration instance is exposed here to allow our consumers to use the same
    /// configuation file for KVP's we haven't thought of.
    /// </remarks>
    public static IConfiguration Configuration { get; }

    /// <summary>
    /// Get {ProductName}:Logging:LogLevel:Console, or default LogEvenLevel.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   The default LogEvenLevel of Verbose will be retured if not found in the file.
    ///   </para>
    ///   <para>
    ///   Defaulting to a top-level context of product name avoids collisions in the default scope.
    ///   </para>
    /// </remarks>
    public static LogLevel DefaultConsoleLogEventLevel { get; }

    /// <summary>
    /// Get {ProductName}:Logging:LogLevel:File if it exists, or default LogEvenLevel.
    /// </summary>
    /// <remarks>
    /// The default LogEvenLevel of Verbose will be retured if not found in the file.
    /// </remarks>
    public static LogLevel DefaultFileLogEventLevel { get; }

    /// <summary>
    /// Get the json settings file name.
    /// </summary>
    public static string JsonAppSettingsFileName { get; set; }

    /// <summary>
    /// Get {ProductName}:Logging:LogPath:Directory if not null, or return the LocalApplicationData special folder.
    /// </summary>
    public static string LogDirectory { get; }

    /// <summary>
    /// Get {ProductName}:Logging:FilePrefix if not null, or return <see cref="AppConfig.ProductName"/>
    /// </summary>
    public static string LogFilePrefix { get; }

    /// <summary>
    /// Get {ProductName}:Logging:FilePostfix if not null, or return ".txt".
    /// </summary>
    public static string LogFilePostfix { get; }

    /// <summary>
    /// Get the production log path for the calling app. Constructed from <see cref="LogDirectory"/>,
    /// <see cref="LogFilePrefix"/> an intervening hyphen and <see cref="LogFilePostfix"/>
    /// </summary>
    public static string ProductionLogPath { get; }

    /// <summary>
    /// Get the product name.
    /// </summary>
    /// <remarks>
    /// This will be broken if called from unmanaged code, but we'll probably never really care about that.    
    /// </remarks>
    public static string ProductName { get; }

    private static string? _rootDataPath;

    /// <summary>
    /// Get the application's root data path.
    /// </summary>
    public static string RootDataPath
    {
        get
        {
            if (_rootDataPath is not null) return _rootDataPath;

            var path = Configuration.GetValue<string>($"{ProductName}:RootDataPath");

            if (string.IsNullOrEmpty(path))
            {
                path = UserAppDataFolder;
            }

            _rootDataPath = Path.Join(path, "Data");

            return _rootDataPath;
        }
    }

    /// <summary>
    /// Get or set whether trace events should be logged.
    /// </summary>
    public static bool TraceEnabled { get; }

    /// <summary>
    /// Returns a path of the form %LOCALAPPDATA%\CompanyName\ProductName on Windows,
    /// or $Home\CompanyName\ProductName on Linux.
    /// </summary>
    public static string UserAppDataFolder { get; }

    private static string GetUserAppDataFolder()
    {
        var userPath = Environment.GetEnvironmentVariable(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LOCALAPPDATA" : "Home");
        var assembly = Assembly.GetCallingAssembly();
        var companyName = assembly?.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault()?.Company;

        Verify.IsNotNullOrEmpty(userPath);
        Verify.IsNotNullOrEmpty(companyName);

        var path = Path.Combine(userPath, companyName, ProductName);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        return path;
    }

    private static string GetLogDirectory()
    {
        var path = Configuration.GetValue<string>($"{ProductName}:Logging:LogPath:Directory");

        if (string.IsNullOrEmpty(path))
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    ?? @".\";
        }

        return Path.Join(path, CompanyName, ProductName, "Logs");
    }

    static AppConfig()
    {
        ProductName = AppDomain.CurrentDomain.FriendlyName.Split(',')[0];
        Verify.IsNotNullOrEmpty(ProductName);

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var callingAssembly = Assembly.GetCallingAssembly();
        Verify.IsNotNull(callingAssembly);

        var companyName = callingAssembly.GetCustomAttributes<AssemblyCompanyAttribute>().FirstOrDefault()?.Company;
        Verify.IsNotNullOrEmpty(companyName);

        CompanyName = companyName;

        JsonAppSettingsFileName = $"{ProductName}.appsettings.json";

        Configuration = new ConfigurationBuilder()
          .AddJsonFile(JsonAppSettingsFileName, optional: false, reloadOnChange: true)
          .Build();

        DefaultConsoleLogEventLevel = Configuration.GetValue<LogLevel>($"{ProductName}:Logging:LogLevel:Console");
        DefaultFileLogEventLevel = Configuration.GetValue<LogLevel>($"{ProductName}:Logging:LogLevel:File");
        ProductionLogPath = Path.Join(LogDirectory, $"{LogFilePrefix}-{LogFilePostfix}");
        LogFilePrefix = Configuration.GetValue<string>($"{ProductName}:Logging:FilePrefix") ?? ProductName;
        LogFilePostfix = Configuration.GetValue<string>($"{ProductName}:Logging:FilePostfix") ?? ".log";
        LogDirectory = GetLogDirectory();
        TraceEnabled = Configuration.GetValue<bool>($"{ProductName}:Logging:LogLevel:TraceEnabled");

        UserAppDataFolder = GetUserAppDataFolder();

        if (!File.Exists(JsonAppSettingsFileName))
        {
            var defaultAppSettingsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                AppDomain.CurrentDomain.RelativeSearchPath ?? "",
                JsonAppSettingsFileName);
            if (File.Exists(defaultAppSettingsPath)) JsonAppSettingsFileName = defaultAppSettingsPath;
        }
    }
}

// Copyright Joseph W Donahue and Sharper Hacks LLC (US-WA)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// SharperHacks is a trademark of Sharper Hacks LLC (US-Wa), and may not be
// applied to distributions of derivative works, without the express written
// permission of a registered officer of Sharper Hacks LLC (US-WA).
