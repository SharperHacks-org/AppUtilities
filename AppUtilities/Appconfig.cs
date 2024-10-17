// Copyright and trademark notices at the end of this file.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//using Serilog.Events;

using SharperHacks.CoreLibs.Constraints;

using System.Text;

namespace SharperHacks.CoreLibs.AppUtilities;

/// <summary>
/// Wires up the appconfig data we need to initialize the app.
/// </summary>
public static class AppConfig
{
    internal static string _productName;
    internal static string _logDirectory = string.Empty;

    // ToDo: We should probably allow some of these to be modified by our consumers.

    /// <summary>
    /// Get the product name.
    /// </summary>
    /// <remarks>
    /// This will be broken if called from unmanaged code, but we'll probably never really care about that.    
    /// </remarks>
    public static string ProductName
    {
        get
        {
            if (string.IsNullOrEmpty(_productName))
            {
                _productName = AppDomain.CurrentDomain.FriendlyName.Split(',')[0];
                Verify.IsNotNullOrEmpty(_productName);
            }
            return _productName!;
        }
    }

    /// <summary>
    /// Get the json settings file name.
    /// </summary>
    public static string JsonAppSettingsFileName { get; set; } = $"{ProductName}.appsettings.json";

    /// <summary>
    /// Get the IConfiguration instance.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <remarks>
    /// Other members of this class provide shorthand access to some, but not all config data.
    /// The IConfiguration instance is exposed here to allow our consumers to use the same
    /// configuation file for KVP's we haven't thought of.
    /// </remarks>
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
      .AddJsonFile(JsonAppSettingsFileName, optional: false, reloadOnChange: true)
      .Build();

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
    public static LogLevel DefaultConsoleLogEventLevel => Configuration.GetValue<LogLevel>($"{ProductName}:Logging:LogLevel:Console");

    /// <summary>
    /// Get {ProductName}:Logging:LogLevel:File if it exists, or default LogEvenLevel.
    /// </summary>
    /// <remarks>
    /// The default LogEvenLevel of Verbose will be retured if not found in the file.
    /// </remarks>
    public static LogLevel DefaultFileLogEventLevel => Configuration.GetValue<LogLevel>($"{ProductName}:Logging:LogLevel:File");

    /// <summary>
    /// Get {ProductName}:Logging:LogPath:Directory if not null, or return the LocalApplicationData special folder.
    /// </summary>
    public static string LogDirectory
    { 
        get
        {
            if (string.IsNullOrEmpty(_logDirectory))
            {
                var path = Configuration.GetValue<string>($"{ProductName}:Logging:LogPath:Directory");

                if (string.IsNullOrEmpty(path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        ?? @".\";
                }

                _logDirectory = Path.Join(path, ProductName, "Logs");
            }

            return _logDirectory;
        } 
    }

    /// <summary>
    /// Get {ProductName}:Logging:FilePrefix if not null, or return <see cref="AppConfig.ProductName"/>
    /// </summary>
    public static string LogFilePrefix => 
        Configuration.GetValue<string>($"{ProductName}:Logging:FilePrefix") ?? ProductName;

    /// <summary>
    /// Get {ProductName}:Logging:FilePostfix if not null, or return ".txt".
    /// </summary>
    public static string LogFilePostfix =>
        Configuration.GetValue<string>($"{ProductName}:Logging:FilePostfix") ?? ".log";

    /// <summary>
    /// Get the production log path for the calling app. Constructed from <see cref="LogDirectory"/>,
    /// <see cref="LogFilePrefix"/> an intervening hyphen and <see cref="LogFilePostfix"/>
    /// </summary>
    public static string ProductionLogPath => Path.Join(LogDirectory, $"{LogFilePrefix}-{LogFilePostfix}");

    /// <summary>
    /// Get or set whether trace events should be logged.
    /// </summary>
    public static bool TraceEnabled => Configuration.GetValue<bool>($"{ProductName}:Logging:LogLevel:TraceEnabled");

    // ToDo: Console in/out encoding should be configurable!

    static AppConfig()
    {
        Verify.IsNotNullOrEmpty(_productName);
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

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
