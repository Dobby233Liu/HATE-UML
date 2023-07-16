using System;
using Eto.Forms;
using HATE;

namespace HATE.Gtk;

class Program
{
	/// <summary>
	/// The main entry point of the application
	/// </summary>
	[STAThread]
	static void Main()
	{
		new Application(Eto.Platforms.Gtk).Run(new HATE.MainForm());
	}
}