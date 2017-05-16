using IZSlack.Core.Messengers;
using System.Configuration;
using System.IO;

namespace slackBF {
	public static class EnviromentLoader {


		public static void Load() {
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings.Clear();
			try {
				var lines = File.ReadLines(".env");
				foreach (var line in lines) {
					var data = line.Split('=');
					config.AppSettings.Settings.Add(data[0], data[1]);
					config.Save(ConfigurationSaveMode.Modified, true);
					ConfigurationManager.RefreshSection("appSettings");
				}
			}
			catch (FileNotFoundException) {
				ConsoleMessenger.PrintError("Could not locate .env file");
			}
			catch (PathTooLongException) {
				ConsoleMessenger.PrintError("File path too long");
			}
			ConsoleMessenger.PrintSuccess(".env loaded");
		}
	}
}
