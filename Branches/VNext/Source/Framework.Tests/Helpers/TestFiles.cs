using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thinktecture.Wscf.Framework.Properties;

namespace Thinktecture.Wscf.Framework.Tests.Helpers
{
	internal static class TestFiles
	{
		private static readonly Dictionary<string, byte[]> files = new Dictionary<string, byte[]>(4);

		internal static readonly string TempFolder = Path.GetTempPath();

		internal const string RestaurantDataXsdFileName = "RestaurantData.xsd";
		internal const string RestaurantHeaderDataXsdFileName = "RestaurantHeaderData.xsd";
		internal const string RestaurantMessagesXsdFileName = "RestaurantMessages.xsd";
		internal const string RestaurantServiceWsdlFileName = "RestaurantService.wsdl";
        internal const string RestaurantServiceWsdlFileName2 = "RestaurantServiceSimple.wsdl";
		
		static TestFiles()
		{
			files.Add(Path.Combine(TempFolder, RestaurantDataXsdFileName), Encoding.UTF8.GetBytes(Resources.RestaurantDataXsd));
			files.Add(Path.Combine(TempFolder, RestaurantHeaderDataXsdFileName), Encoding.UTF8.GetBytes(Resources.RestaurantHeaderDataXsd));
			files.Add(Path.Combine(TempFolder, RestaurantMessagesXsdFileName), Encoding.UTF8.GetBytes(Resources.RestaurantMessagesXsd));
			files.Add(Path.Combine(TempFolder, RestaurantServiceWsdlFileName), Resources.RestaurantServiceWsdl);
            files.Add(Path.Combine(TempFolder, RestaurantServiceWsdlFileName2), Resources.RestaurantServiceSimple);
		}

		internal static void WriteFiles()
		{
			foreach (KeyValuePair<string, byte[]> file in files)
			{
				File.WriteAllBytes(file.Key, file.Value);
			}
		}

		internal static void DeleteFiles()
		{
			foreach (KeyValuePair<string, byte[]> file in files.Where(file => File.Exists(file.Key)))
			{
				File.Delete(file.Key);
			}
		}

		internal static string GetFilePath(string fileName)
		{
			return files.Where(file => Path.GetFileName(file.Key).Equals(fileName, StringComparison.OrdinalIgnoreCase))
				.Select(file => file.Key)
				.FirstOrDefault();
		}
	}
}
