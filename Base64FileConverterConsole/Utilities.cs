using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Base64FileConverterConsole
{
	public static class Utilities
	{
		public static byte[] Compress(byte[] dataToCompress)
		{
			try
			{
				using (var memStream = new MemoryStream())
				{
					using (var compressionStream = new DeflaterOutputStream(memStream))
					{
						compressionStream.Write(dataToCompress, 0, dataToCompress.Length);
						compressionStream.Close();
					}

					return memStream.ToArray();
				}
			}
			catch
			{
				return dataToCompress;
			}
		}

		public static byte[] Decompress(byte[] dataToDecompress)
		{
			try
			{
				int size;
				var buffer = new byte[2048];

				using (var memStream = new MemoryStream())
				{
					using (var decompressionStream = new InflaterInputStream(new MemoryStream(dataToDecompress)))
					{
						do
						{
							size = decompressionStream.Read(buffer, 0, buffer.Length);
							if (size > 0)
								memStream.Write(buffer, 0, size);
							else
								break;
						}
						while (true);

						decompressionStream.Close();
					}

					return memStream.ToArray();
				}
			}
			catch
			{
				return dataToDecompress;
			}
		}
	}
}
