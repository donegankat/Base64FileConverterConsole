using System;
using System.Collections.Generic;
using System.Text;

namespace Base64FileConverterConsole
{
	public static class ConsoleWriter
	{
		/// <summary>
		/// Prints a blank line.
		/// </summary>
		public static void Line()
		{
			_print(null);
		}

		/// <summary>
		/// Prints an info message.
		/// </summary>
		/// <param name="text"></param>
		public static void Info(string text)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			_print(text);
		}

		/// <summary>
		/// Prints a subdued secondary info message.
		/// </summary>
		/// <param name="text"></param>
		public static void SecondaryInfo(string text)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			_print(text);
		}

		/// <summary>
		/// Prints a success message.
		/// </summary>
		/// <param name="text"></param>
		public static void Success(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			_print(text);
		}

		/// <summary>
		/// Prints an error message.
		/// </summary>
		/// <param name="text"></param>
		public static void Error(string text)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			_print(text);
		}

		/// <summary>
		/// Prints a warning message.
		/// </summary>
		/// <param name="text"></param>
		public static void Warning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			_print(text);
		}

		private static void _print(string text)
		{
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}
}
