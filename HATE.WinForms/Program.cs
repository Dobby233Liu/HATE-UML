﻿using System;
using Eto.Forms;

namespace HATE.WinForms;

class Program
{
	/// <summary>
	/// The main entry point of the application
	/// </summary>
	[STAThread]
	static void Main()
	{
		new Application(Eto.Platforms.WinForms).Run(new HATE.MainForm());
	}
}
