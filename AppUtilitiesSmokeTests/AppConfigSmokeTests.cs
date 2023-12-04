// Copyright and trademark notices at bottom of file.

using SharperHacks.CoreLibs.Constants;

namespace SharperHacks.CoreLibs.AppUtilities;

[TestClass]
public class AppConfigSmokeTests
{
#if DEBUG
    private const bool _debug = true;
#else
    private const bool _debug = false;
#endif

    [TestMethod]
    public void ProductNameIsCorrect()
    {
        Console.WriteLine($"Product name: {Appconfig.ProductName}");
        Assert.AreEqual("testhost", Appconfig.ProductName);
    }

    [TestMethod]
    public void TestConfigFileExists()
    {
        Console.WriteLine($"Config file: {Appconfig.JsonAppSettingsFileName}");
        Assert.IsTrue(File.Exists(Appconfig.JsonAppSettingsFileName));
    }

    [TestMethod]
    public void JsonAppSettingsFileNameIsCorrect()
    {
        var fileName = Appconfig.JsonAppSettingsFileName;

        Assert.IsNotNull(fileName);
        Assert.AreEqual($"{Appconfig.ProductName}.appsettings.json", fileName);
    }

    [TestMethod]
    public void DefaultConsoleLogEventLevelIsCorrect()
    {
        var expected = _debug ? LogLevel.Information : LogLevel.Warning;
        Assert.AreEqual(expected, Appconfig.DefaultConsoleLogEventLevel);
    }

    [TestMethod]
    public void DefaultFileLogEventLevelIsCorrect()
    {
        var expected = _debug ? LogLevel.Verbose : LogLevel.Information;
        Assert.AreEqual(expected, Appconfig.DefaultFileLogEventLevel);
    }

    [TestMethod]
    public void TraceIsEnabled()
    {
        var expected = _debug;
        Assert.AreEqual(expected, Appconfig.TraceEnabled);
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
