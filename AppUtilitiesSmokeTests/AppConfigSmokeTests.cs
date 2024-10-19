// Copyright and trademark notices at bottom of file.

using Microsoft.Extensions.Logging;

namespace SharperHacks.CoreLibs.AppUtilities.UnitTests;

[TestClass]
public class AppConfigSmokeTests
{
    private string _localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) ?? @".\";

    [TestMethod]
    public void ProductNameIsCorrect()
    {
        Console.WriteLine($"Product name: {AppConfig.ProductName}");
        Assert.AreEqual("testhost", AppConfig.ProductName);
    }

    [TestMethod]
    public void TestConfigFileExists()
    {
        Console.WriteLine($"Config file: {AppConfig.JsonAppSettingsFileName}");
        Assert.IsTrue(File.Exists(AppConfig.JsonAppSettingsFileName));
    }

    [TestMethod]
    public void JsonAppSettingsFileNameIsCorrect()
    {
        var fileName = AppConfig.JsonAppSettingsFileName;

        Assert.IsNotNull(fileName);
        Assert.AreEqual($"{AppConfig.ProductName}.appsettings.json", fileName);
    }

    [TestMethod]
    public void DefaultConsoleLogEventLevelIsCorrect()
    {
        Assert.AreEqual(LogLevel.Warning, AppConfig.DefaultConsoleLogEventLevel);
    }

    [TestMethod]
    public void DefaultFileLogEventLevelIsCorrect()
    {
        Assert.AreEqual(LogLevel.Information, AppConfig.DefaultFileLogEventLevel);
    }

    [TestMethod]
    public void DefaultLogDirectoryIsCorrect()
    {
        Console.WriteLine($"LogDirectory: {AppConfig.LogDirectory}");

        Assert.IsTrue(AppConfig.LogDirectory.StartsWith(_localAppDataPath));
        Assert.IsTrue(AppConfig.LogDirectory.EndsWith($@"AppData\Local\{AppConfig.ProductName}\Logs"));
    }

    [TestMethod]
    public void TraceIsEnabled()
    {
        Assert.AreEqual(false, AppConfig.TraceEnabled);
    }

    [TestMethod]
    public void DefaultRootDataPathIsCorrect()
    {
        var rootPath = AppConfig.RootDataPath;

        Console.WriteLine(rootPath);

        Assert.IsNotNull(rootPath);
        Assert.IsFalse(string.IsNullOrEmpty(rootPath));
        Assert.IsTrue(rootPath.StartsWith(_localAppDataPath));
        Assert.IsTrue(rootPath.EndsWith($@"AppData\Local\{AppConfig.ProductName}\Data"));
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
