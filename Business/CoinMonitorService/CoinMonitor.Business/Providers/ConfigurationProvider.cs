using System;
using System.Configuration;
using log4net;

namespace CoinMonitor.Business.Providers
{
	public class ConfigurationProvider
	{
		public static T GetConfigurationByName<T>(string name, T defaultValue)
		{
			try
			{
				string configValue = ConfigurationManager.AppSettings[name];

				if (!string.IsNullOrEmpty(configValue))
				{
					Type configType = typeof(T);

					if (configType == typeof(string))
					{
						return (T) (object) configValue;
					}

					if (configType == typeof(int))
					{
						object value = int.Parse(configValue);
						return (T)value;
					}

					if (configType == typeof(DateTime))
					{
						object value = DateTime.Parse(configValue);
						return (T) value;
					}

					if (configType == typeof(TimeSpan))
					{
						object value = TimeSpan.Parse(configValue);
						return (T) value;
					}

				}
			}
			catch (Exception e)
			{
				LogManager.GetLogger("ConfigurationProvider").DebugFormat($"Exception trying to resolve configuration type: {e.Message}");
			}
			

			return defaultValue;
		}
	}
}
