using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Base64FileConverterConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				ConsoleWriter.Info("Select a run mode.");
				ConsoleWriter.Info("Press 'c' to convert a file to Base64 or press 'v' to convert Base64 to a file.");
				ConsoleWriter.Line();

				var exitApp = false;

				var input = Console.ReadKey();
				ConsoleWriter.Line();

				if (input.Key == ConsoleKey.C)
				{
					Console.Clear();
					exitApp = FileToBase64();
				}
				else if (input.Key == ConsoleKey.V)
				{
					Console.Clear();
					exitApp = Base64ToFile();
				}
				else
				{
					ConsoleWriter.Warning("Unrecognized input.");
					ConsoleWriter.Line();
				}

				if (exitApp)
					break;				
			}
		}

		/// <summary>
		/// Converts a file to a Base64 string and copies either the compressed or decompressed versions to the clipboard.
		/// </summary>
		/// <returns>Returns true if the console app should exit after this method completes.</returns>
		public static bool FileToBase64()
		{
			var exitApp = false;
			var filePath = string.Empty;

			while (true)
			{
				ConsoleWriter.Info("Enter the full directory path and file name for the file you wish to convert to a Base64 string.");

				filePath = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
				{
					ConsoleWriter.Error("File path was invalid.");
					ConsoleWriter.Line();
				}
				else
					break;
			}

			ConsoleWriter.Line();
			ConsoleWriter.Line();

			var bytes = File.ReadAllBytes(filePath);
			var base64String = Convert.ToBase64String(bytes);

			ConsoleWriter.Success("File has been converted!");
			ConsoleWriter.Line();

			ConsoleWriter.Info("Press 'c' to copy the raw, decompressed Base64 string to clipboard.");
			ConsoleWriter.Info("Press 'v' to copy the compressed (deflated) Base64 string to clipboard.");
			ConsoleWriter.Info("Press 'Enter' to clear the current file and start over.");
			ConsoleWriter.Info("Press 'Escape' to exit.");

			// Prompt for the user's next action.
			while (true)
			{
				var key = Console.ReadKey();
				ConsoleWriter.Line();

				if (key.Key == ConsoleKey.C)
				{
					var clipboard = new TextCopy.Clipboard();
					clipboard.SetText(base64String);

					ConsoleWriter.Line();
					ConsoleWriter.Success("Decompressed Base64 file contents copied to clipboard.");
				}
				else if (key.Key == ConsoleKey.V)
				{
					var compressedBytes = Utilities.Compress(bytes);
					var compressedBase64String = Convert.ToBase64String(compressedBytes);

					var clipboard = new TextCopy.Clipboard();
					clipboard.SetText(compressedBase64String);

					ConsoleWriter.Line();
					ConsoleWriter.Success("Compressed (deflated) Base64 file contents copied to clipboard.");
				}
				else if (key.Key == ConsoleKey.Enter)
				{
					// Clear and repeat.
					Console.Clear();
					break;
				}
				else if (key.Key == ConsoleKey.Escape)
				{
					exitApp = true;
					break;
				}
				else
				{
					ConsoleWriter.Warning("Unrecognized input.");
				}
			}

			return exitApp;
		}

		/// <summary>
		/// Converts a Base64 string to a file and saves it to a directory.
		/// </summary>
		/// <returns>Returns true if the console app should exit after this method completes.</returns>
		public static bool Base64ToFile()
		{
			var exitApp = false;
			var bytes = new byte[0];

			// Prompt for the Base64 string.
			while (true)
			{
				//ConsoleWriter.Info("Enter the Base64 string you wish to convert to a file.");
				//ConsoleWriter.SecondaryInfo("The string will be decompressed before being saved to a file, so either a compressed (deflated) or decompressed string is valid and will produce the same result.");

				//var base64 = Console.ReadLine();
				//ConsoleWriter.Line();

				//if (string.IsNullOrWhiteSpace(base64))
				//{
				//	ConsoleWriter.Error("No string was provided.");
				//	ConsoleWriter.Line();
				//}
				//else
				//{
				//	try
				//	{
				//		bytes = Convert.FromBase64String(base64);							
				//		break;
				//	}
				//	catch (FormatException)
				//	{
				//		ConsoleWriter.Error("String was invalid Base64.");
				//		ConsoleWriter.Line();
				//	}
				//}

				ConsoleWriter.Info("Enter the full directory path and file name for the Base64 string file you wish to convert to a file.");
				ConsoleWriter.SecondaryInfo("The Base64 string will be decompressed before being saved to a file, so either a compressed (deflated) or decompressed string is valid and will produce the same result.");

				var sourceFile = Console.ReadLine();
				ConsoleWriter.Line();

				if (string.IsNullOrWhiteSpace(sourceFile))
				{
					ConsoleWriter.Error("No file path was provided.");
					ConsoleWriter.Line();
				}
				else if (!File.Exists(sourceFile))
				{
					ConsoleWriter.Error("Invalid file path was provided; the file does not exist.");
					ConsoleWriter.Line();
				}
				else
				{
					var base64 = File.ReadAllText(sourceFile);

					try
					{
						bytes = Convert.FromBase64String(base64);
						break;
					}
					catch (FormatException)
					{
						ConsoleWriter.Error("File content was an invalid Base64 string.");
						ConsoleWriter.Line();
					}
				}
			}

			var destinationDirectory = string.Empty;

			// Prompt for the directory to save the file to.
			while (true)
			{
				var defaultDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ConvertedFromBase64";
				ConsoleWriter.Info($"Enter the directory path you wish to save the file to or press 'Enter' to save the file to:");
				ConsoleWriter.SecondaryInfo(defaultDirectory);

				destinationDirectory = Console.ReadLine();
				ConsoleWriter.Line();

				if (string.IsNullOrWhiteSpace(destinationDirectory))
					destinationDirectory = defaultDirectory;

				try
				{
					Directory.CreateDirectory(destinationDirectory);
					break;
				}
				catch (Exception ex)
				{
					// Print error and re-prompt for a new directory.
					ConsoleWriter.Error($"Failed to create directory. {ex.Message}");
					ConsoleWriter.Line();
				}
			}

			var fileName = string.Empty;

			// Prompt for the file name to save the string as.
			while (true)
			{
				ConsoleWriter.Info("Enter the desired file name including the file extension.");

				fileName = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(fileName))
				{
					ConsoleWriter.Error("No file name was provided.");
					ConsoleWriter.Line();
				}
				else if (!fileName.Contains("."))
				{
					ConsoleWriter.Error("File name did not contain an extension.");
					ConsoleWriter.Line();
				}
				else
					break;
			}

			var fullFilePath = $"{destinationDirectory}\\{fileName}";

			// Decompress the bytes and save it to a file. If the bytes were already decompressed this doesn't do anything.
			File.WriteAllBytes(fullFilePath, Utilities.Decompress(bytes));
			ConsoleWriter.Line();
			ConsoleWriter.Line();

			ConsoleWriter.Success($"Base64 has been saved to {fullFilePath}!");
			ConsoleWriter.Line();

			ConsoleWriter.Info("Press 'Enter' to start over.");
			ConsoleWriter.Info("Press 'Escape' to exit.");

			// Prompt for the next action.
			while (true)
			{
				var key = Console.ReadKey();
				ConsoleWriter.Line();

				if (key.Key == ConsoleKey.Enter)
				{
					// Clear and repeat.
					Console.Clear();
					break;
				}
				else if (key.Key == ConsoleKey.Escape)
				{
					exitApp = true;
					break;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Unrecognized input.");
				}
			}

			return exitApp;
		}
	}
}
