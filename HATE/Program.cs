using System;
using Eto.Forms;

namespace HATE
{
	class Program
	{
		/// <summary>
		/// The main entry point for the application
		/// </summary>
		[STAThread]
		static void Main()
		{
			new Application(Eto.Platform.Detect).Run(new MainForm());
		}
	}
}