using Eto.Forms;
using Eto.Drawing;

namespace HATE;

public class SmallSpacer : Label
{
	public SmallSpacer()
	{
		this.Handler.Height = 10;
	}
	public SmallSpacer(int height)
	{
		this.Handler.Height = height;
	}
}
	
public partial class MainForm : Form
{
	public MainForm()
	{
		Title = "HATE-UML";
		MinimumSize = new Size(200, 485);
		BackgroundColor = Colors.Black;
		
		// GTK currently looks weird af with background colors
		if (Eto.Platform.Instance.IsWinForms)
		{
			btnCorrupt.BackgroundColor = Colors.Black;
			btnLaunch.BackgroundColor = Colors.Black;
		}

		DynamicLayout mainLayout = new DynamicLayout();
		mainLayout.BeginCentered();
		mainLayout.AddRange(
			new SmallSpacer(),
			label3,
			new SmallSpacer(5),
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

		// Events
		PreLoad += MainForm_PreLoad;
		Load += MainForm_Load;
		LoadComplete += MainForm_LoadComplete;
		Closing += MainForm_FormClosing;
		btnCorrupt.Click += button_Corrupt_Clicked;
		chbShuffleText.CheckedChanged += chbShuffleText_CheckedChanged;
		chbShuffleGFX.CheckedChanged += chbShuffleGFX_CheckedChanged;
		chbHitboxFix.CheckedChanged += chbHitboxFix_CheckedChanged;
		chbShuffleFont.CheckedChanged += chbShuffleFont_CheckedChanged;
		chbShuffleBG.CheckedChanged += chbShuffleBG_CheckedChanged;
		chbShuffleAudio.CheckedChanged += chbShuffleAudio_CheckedChanged;
		txtSeed.GotFocus += txtPower_Enter;
		txtSeed.LostFocus += txtPower_Leave;
		txtPower.GotFocus += txtPower_Enter;
		txtPower.LostFocus += txtPower_Leave;
		btnLaunch.Click += btnLaunch_Clicked;
	}

	private Button btnCorrupt = new Button
	{
		Text = "-CORRUPT-",
		TextColor = Colors.Coral,
		Font = new Font(SystemFont.Bold, 8.25F)
	};
	private CheckBox chbShuffleText = new CheckBox
	{
		Text = "Shuffle Text",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private CheckBox chbShuffleGFX = new CheckBox
	{
		Text = "Shuffle Sprites",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private CheckBox chbHitboxFix = new CheckBox
	{
		Text = "Hitbox Fix",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private CheckBox chbShuffleFont = new CheckBox
	{
		Text = "Shuffle Fonts",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private CheckBox chbShuffleBG = new CheckBox
	{
		Text = "Shuffle GFX/BG",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private CheckBox chbShuffleAudio = new CheckBox
	{
		Text = "Shuffle Audio",
		TextColor = Colors.White,
		Height = 40,
		Font = new Font(SystemFont.Bold, 12F)
	};
	private TextBox txtSeed = new TextBox
	{
		BackgroundColor = Colors.White,
	};
	private TextBox txtPower = new TextBox
	{
		BackgroundColor = Colors.White,
		Text = "0 - 255",
	};
	private Label label1 = new Label
	{
		Font = new Font(SystemFont.Bold, 10F),
		Text = "Seed:",
		TextColor = Colors.White,
	};
	private Label label2 = new Label
	{
		Font = new Font(SystemFont.Bold, 10F),
		Text = "Power:",
		TextColor = Colors.White,
	};
	private Button btnLaunch = new Button
	{
		Font = new Font(SystemFont.Bold, 8.25F),
		TextColor = Colors.Fuchsia,
		Text = "-LAUNCH-"
	};
	private Label label3 = new Label
	{
		Font = new Font(SystemFont.Bold, 10F),
		TextColor = Colors.White,
		Text = "Current Game:"
	};
	private Label lblGameName = new Label()
	{
		BackgroundColor = Colors.Black,
		TextColor = Colors.White,
		Text = "Loading game..."
	};
}