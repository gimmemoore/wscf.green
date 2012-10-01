using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;

namespace Thinktecture.Wscf.Framework.Metadata
{
	/// <summary>
	/// Provides support for the discovery of metadata from local and remote sources.
	/// </summary>
	public class MetadataDiscovery : IMetadataDiscovery
	{
		private List<MetadataSection> metadataDocuments;
		private readonly IMexMetadataResolver mexMetadataResolver;
		private readonly IXmlMetadataResolver xmlMetadataResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataDiscovery"/> class.
		/// </summary>
		public MetadataDiscovery() : this(new MexMetadataResolver(), new XmlMetadataResolver())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MetadataDiscovery"/> class.
		/// </summary>
		/// <param name="mexMetadataResolver">The MEX metadata resolver.</param>
		/// <param name="xmlMetadataResolver">The XML metadata resolver.</param>
		internal MetadataDiscovery(IMexMetadataResolver mexMetadataResolver, IXmlMetadataResolver xmlMetadataResolver)
		{
			this.mexMetadataResolver = mexMetadataResolver;
			this.xmlMetadataResolver = xmlMetadataResolver;
		}

		/// <summary>
		/// Discovers any known types of metadata in the specified paths.
		/// </summary>
		/// <param name="paths">The paths to search.</param>
		/// <returns>A complete set of all discovered metadata.</returns>
		public MetadataSet Process(IEnumerable<string> paths)
		{
			return Process(paths.ToArray());
		}

		/// <summary>
		/// Discovers any known types of metadata in the specified paths.
		/// </summary>
		/// <param name="paths">The paths to search.</param>
		/// <returns>A complete set of all discovered metadata.</returns>
		public MetadataSet Process(params string[] paths)
		{
			metadataDocuments = new List<MetadataSection>();

			foreach (string path in paths)
			{
				ProcessInput(path);
			}

			return new MetadataSet(metadataDocuments);
		}

		private void ProcessInput(string path)
		{
			Uri uri;
			if (Uri.TryCreate(path, UriKind.Absolute, out uri) && !uri.IsFile)
			{
				ProcessInputAsUri(uri);
			}
			else
			{
				FileInfo[] files;
				if (TryFindFiles(path, out files))
				{
					try
					{
						ProcessInputAsFilePath(files, path);
						return;
					}
					catch (Exception exception)
					{
						if (exception.IsFatal()) throw;

						throw new ArgumentException(string.Format("The input argument '{0}' could not be read.", path), exception);
					}
				}
				string message = string.Format("The input path '{0}' does not appear to refer to any existing files and does not appear to be a valid URI.", path);
				throw new ArgumentException(message);
			}
		}

		private void ProcessInputAsUri(Uri serviceUri)
		{
			IEnumerable<MetadataSection> resolvedMetadata = mexMetadataResolver.Resolve(serviceUri);
			metadataDocuments.AddRange(resolvedMetadata);
		}

		private void ProcessInputAsFilePath(IEnumerable<FileInfo> inputFiles, string path)
		{
			foreach (FileInfo inputFile in inputFiles)
			{
				try
				{
					IEnumerable<MetadataSection> resolvedMetadata = xmlMetadataResolver.Resolve(inputFile);
					metadataDocuments.AddRange(resolvedMetadata);
				}
				catch (MetadataDiscoveryException ex)
				{
					string message = string.Format("An exception was thrown while processing the input argument '{0}': {1}", path, ex.Message);
					throw new MetadataDiscoveryException(message, ex);
				}
			}
		}

		private static bool TryFindFiles(string path, out FileInfo[] files)
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			if (path.IndexOfAny(invalidPathChars) != -1)
			{
				files = null;
				return false;
			}
			string directoryName = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(directoryName))
			{
				directoryName = @".\";
			}
			DirectoryInfo directory = new DirectoryInfo(directoryName);
			if (!directory.Exists)
			{
				string message = string.Format("The directory '{0}' could not be found. Verify that the directory exists and that you have the appropriate permissions to read it.", directory.FullName);
				throw new ArgumentException(message);
			}
			string fileName = Path.GetFileName(path);
			if (string.IsNullOrEmpty(fileName))
			{
				string message = string.Format("The input path '{0}' appears to be a directory. Inputs must be either URLs or file paths.", path);
				throw new ArgumentException(message);
			}
			FileInfo[] directoryFiles = directory.GetFiles(fileName);
			if (directoryFiles.Length == 0)
			{
				string message = string.Format("The input path '{0}' doesn't appear to refer to any existing files.", path);
				throw new ArgumentException(message);
			}
			files = directoryFiles;
			return true;
		}
	}
}
