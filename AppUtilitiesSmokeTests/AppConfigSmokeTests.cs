// Copyright and trademark notices at bottom of file.

using Microsoft.Extensions.Logging;

using SharperHacks.CoreLibs.StringExtensions;

namespace SharperHacks.CoreLibs.AppUtilities.UnitTests;

[TestClass]
public class AppConfigSmokeTests
{
    private string _localAppDataPath = 
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) ?? $".{Path.DirectorySeparatorChar}";

    [TestMethod]
    public void CompanyNameIsCorrect()
    {
        Console.WriteLine($"Company name: {AppConfig.CompanyName}");
        Assert.AreEqual("Sharper Hacks LLC (US-WA)", AppConfig.CompanyName);
    }

    [TestMethod]
    public void DefaultConsoleLogEventLevelIsCorrect()
    {
        Console.WriteLine($"Default console logging level: {AppConfig.DefaultConsoleLogEventLevel}");
        Assert.AreEqual(LogLevel.Warning, AppConfig.DefaultConsoleLogEventLevel);
    }

    [TestMethod]
    public void DefaultFileLogEventLevelIsCorrect()
    {
        Console.WriteLine($"Default file log event level: {AppConfig.DefaultFileLogEventLevel}");
        Assert.AreEqual(LogLevel.Information, AppConfig.DefaultFileLogEventLevel);
    }

    [TestMethod]
    public void JsonAppSettingsFileNameIsCorrect()
    {
        var fileName = AppConfig.JsonAppSettingsFileName;

        Assert.IsNotNull(fileName);
        Assert.AreEqual($"{AppConfig.ProductName}.appsettings.json", fileName);
    }

    [TestMethod]
    public void LogDirectoryIsCorrect()
    {
        Console.WriteLine($"LogDirectory: {AppConfig.LogDirectory}");

        Assert.StartsWith(_localAppDataPath, AppConfig.LogDirectory);
        Assert.EndsWith($@"AppData\Local\{AppConfig.CompanyName}\{AppConfig.ProductName}\Logs".CorrectOSPathSeparators(), AppConfig.LogDirectory);
    }

    [TestMethod]
    public void LogFilePrefixIsCorrect()
    {
        Console.WriteLine($"Log file prefix: {AppConfig.LogFilePrefix}");

        Assert.AreEqual("testhost", AppConfig.LogFilePrefix);
    }

    [TestMethod]
    public void ProductionLogPathIsCorrect()
    {
        Console.WriteLine(AppConfig.ProductionLogPath);

        Assert.EndsWith("testhost-.log", AppConfig.ProductionLogPath);
    }

    [TestMethod]
    public void ProductNameIsCorrect()
    {
        Console.WriteLine($"Product name: {AppConfig.ProductName}");

        Assert.AreEqual("testhost", AppConfig.ProductName);
    }

    [TestMethod]
    public void RootDataPathIsCorrect()
    {
        var rootPath = AppConfig.RootDataPath;
        var dataPathTail = $@"{AppConfig.CompanyName}\{AppConfig.ProductName}\Data".CorrectOSPathSeparators();

        Console.WriteLine(rootPath);

        Assert.IsNotNull(rootPath);
        Assert.IsFalse(string.IsNullOrEmpty(rootPath));
        Assert.StartsWith(_localAppDataPath, rootPath);
        Assert.EndsWith(dataPathTail, rootPath);
    }

    [TestMethod]
    public void TestConfigFileExists()
    {
        Console.WriteLine($"Config file: {AppConfig.JsonAppSettingsFileName}");

        Assert.IsTrue(File.Exists(AppConfig.JsonAppSettingsFileName));
    }

    [TestMethod]
    public void TraceIsEnabled()
    {
        Assert.IsFalse(AppConfig.TraceEnabled);
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
