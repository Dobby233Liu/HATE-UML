using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace HATE;

public class SmallSpacer : Label
{
	public SmallSpacer()
	{
		this.Handler.Height = 10;
	}
}
	
public partial class MainForm : Form
{
	public MainForm()
	{
		Title = "HATE-UML";
		MinimumSize = new Size(200, 460);
		BackgroundColor = Colors.Black;
		
		// GTK currently looks weird af with background colors
		if (Eto.Platform.Instance.IsWinForms)
		{
			btnCorrupt.BackgroundColor = Colors.Black;
			btnLaunch.BackgroundColor = Colors.Black;
		}

		DynamicLayout mainLayout = new DynamicLayout();
		mainLayout.BeginCentered();
		mainLayout.AddSpace();
		mainLayout.AddRange(
			label3,
			lblGameName,
			new SmallSpacer(),
			chbShuffleAudio,
			chbShuffleBG,
			chbShuffleGFX,
			chbHitboxFix,
			chbShuffleFont,
			chbShuffleText);
		mainLayout.Add(new SmallSpacer());
		mainLayout.BeginVertical();
		mainLayout.AddRow(label1, null, txtSeed);
		mainLayout.AddRow(new SmallSpacer());
		mainLayout.AddRow(label2, null, txtPower);
		mainLayout.AddRow(new SmallSpacer());
		mainLayout.EndVertical();
		mainLayout.AddColumn(btnCorrupt, new SmallSpacer(), btnLaunch);
		mainLayout.AddSpace();
		mainLayout.EndCentered();

		Content = mainLayout;
			
		// Events linkage
		LoadComplete += MainForm_Load;
		Closing += MainForm_FormClosing;
		btnCorrupt.Click += button_Corrupt_Clicked;
		chbShuffleText.CheckedChanged += chbShuffleText_CheckedChanged;
		chbShuffleGFX.CheckedChanged += chbShuffleGFX_CheckedChanged;
		chbHitboxFix.CheckedChanged += chbHitboxFix_CheckedChanged;
		chbShuffleFont.CheckedChanged += chbShuffleFont_CheckedChanged;
		chbShuffleBG.CheckedChanged += chbShuffleBG_CheckedChanged;
		chbShuffleAudio.CheckedChanged += chbShuffleAudio_CheckedChanged;
		txtSeed.MouseEnter += txtPower_Enter;
		txtSeed.MouseLeave += txtPower_Leave;
		txtPower.MouseEnter += txtPower_Enter;
		txtPower.MouseLeave += txtPower_Leave;
		btnLaunch.Click += btnLaunch_Clicked;

	}

	private IEnumerable<FontFamily> foo = Fonts.AvailableFontFamilies;
		
	private Button btnCorrupt = new Button
	{
		Font = new Font(SystemFont.Bold, 8.25F),
		TextColor = Colors.Coral,
		Text = "-CORRUPT-"
		//CLICK
	};
	private CheckBox chbShuffleText = new CheckBox
	{
		Text = "Shuffle Text",
		Height = 40,
		//comic sans
		Font = new Font(SystemFont.Bold, 14.25F),
		//CheckedChanged
	};
	private CheckBox chbShuffleGFX = new CheckBox
	{
		Text = "Shuffle Sprites",
		Height = 40,
		// comic sans
		Font = new Font(SystemFont.Bold, 14.25F)
		//CheckedChanged
	};
	private CheckBox chbHitboxFix= new CheckBox
	{
		Text = "Hitbox Fix",
		Height = 40,
		// comic sans
		Font = new Font(SystemFont.Bold, 14.25F)
		//CheckedChanged
	};
	private CheckBox chbShuffleFont= new CheckBox
	{
		Text = "Shuffle Fonts",
		Height = 40,
		// comic sans
		Font = new Font(SystemFont.Bold, 14.25F)
		//CheckedChanged
	};
	private CheckBox chbShuffleBG= new CheckBox
	{
		Text = "Shuffle GFX/BG",
		Height = 40,
		// comic sans
		Font = new Font(SystemFont.Bold, 14.25F)
		//CheckedChanged
	};
	private CheckBox chbShuffleAudio= new CheckBox
	{
		Text = "Shuffle Audio",
		Height = 40,
		// comic sans
		Font = new Font(SystemFont.Bold, 14.25F)
		//CheckedChanged
	};
	private TextBox txtSeed= new TextBox
	{
		BackgroundColor = Colors.Black,
		TextColor = Colors.White,
		//Enter
		//Leave
	};
	private TextBox txtPower = new TextBox
	{
		BackgroundColor = Colors.Black,
		TextColor = Colors.White,
		Text = "0 - 255",
		//Enter
		//Leave
	};
	private Label label1 = new Label
	{
		Font = new Font(SystemFont.Bold, 10F),
		Text = "Seed:"
	};
	private Label label2= new Label
	{
		Font = new Font(SystemFont.Bold, 10F),
		Text = "Power:"
	};
	private Button btnLaunch= new Button
	{
		Font = new Font(SystemFont.Bold, 8.25F),
		TextColor = Colors.Fuchsia,
		Text = "-LAUNCH-"
		//CLICK
	};
	private Label label3 = new Label
	{
			
		Font = new Font(SystemFont.Bold, 10F),
		Text = "Current Game:"
	};
	private Label lblGameName = new Label()
	{
		BackgroundColor = Colors.Black,
		TextColor = Colors.White,
		Text = "Loading game...",
	};
}
	
public static class MsgBoxHelpers
{
	public static DialogResult ShowMessage(string message, MessageBoxButtons buttons, MessageBoxType icon, string caption = "HATE-UML")
	{
		return Application.Instance.Invoke(delegate { return MessageBox.Show(message, caption, buttons, icon); });
	}
	public static DialogResult ShowMessage(string message, string caption = "HATE-UML")
	{
		return Application.Instance.Invoke(delegate { return MessageBox.Show(message, caption); });
	}
	public static DialogResult ShowWarning(string message, string caption = "HATE-UML")
	{
		return Application.Instance.Invoke(delegate { return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxType.Warning); });
	}
	public static DialogResult ShowError(string message, string caption = "HATE-UML")
	{
		return Application.Instance.Invoke(delegate { return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxType.Error); });
	}
}