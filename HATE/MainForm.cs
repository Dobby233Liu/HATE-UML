using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using UndertaleModLib;
using System.Runtime;
using System.Threading.Tasks;
using UndertaleModLib.Models;
using System.Reflection;

namespace HATE
{
    public partial class MainForm : Form
    {
        private static class Style
        {
            private static readonly Color _btnRestoreColor = Color.LimeGreen;
            private static readonly Color _btnCorruptColor = Color.Coral;
            private static readonly string _btnRestoreLabel = " -RESTORE- ";
            private static readonly string _btnCorruptLabel = " -CORRUPT- ";
            private static readonly Color _optionSet = Color.Yellow;
            private static readonly Color _optionUnset = Color.White;
            
            public static Color GetOptionColor(bool b) { return b ? _optionSet : _optionUnset; }
            public static Color GetCorruptColor(bool b) { return b ? _btnCorruptColor : _btnRestoreColor; }
            public static string GetCorruptLabel(bool b) { return b ? _btnCorruptLabel : _btnRestoreLabel; }
        }

        private StreamWriter _logWriter;
        
        private bool _shuffleGFX = false;
        private bool _shuffleText = false;
        private bool _hitboxFix = false;
        private bool _shuffleFont = false;
        private bool _shuffleBG = false;
        private bool _shuffleAudio = false;
        private bool _corrupt = false;
        private bool _friskMode = false;
        private float _truePower = 0;
        private readonly string _defaultPowerText = "0 - 255";
        private readonly string _dataWin = "data.win";

        private readonly string[] _friskSpriteHandles = {
            // UNDERTALE
            "spr_maincharal", "spr_maincharau", "spr_maincharar", "spr_maincharad",
            "spr_maincharau_stark", "spr_maincharar_stark", "spr_maincharal_stark",
            "spr_maincharad_pranked", "spr_maincharal_pranked",
            "spr_maincharad_umbrellafall", "spr_maincharau_umbrellafall", "spr_maincharar_umbrellafall", "spr_maincharal_umbrellafall",
            "spr_maincharad_umbrella", "spr_maincharau_umbrella", "spr_maincharar_umbrella", "spr_maincharal_umbrella",
            "spr_charad", "spr_charad_fall", "spr_charar", "spr_charar_fall", "spr_charal", "spr_charal_fall", "spr_charau", "spr_charau_fall",
            "spr_maincharar_shadow", "spr_maincharal_shadow", "spr_maincharau_shadow", "spr_maincharad_shadow",
            "spr_maincharal_tomato", "spr_maincharal_burnt", "spr_maincharal_water",
            "spr_maincharar_water", "spr_maincharau_water", "spr_maincharad_water", "spr_mainchara_pourwater",
            "spr_maincharad_b", "spr_maincharau_b", "spr_maincharar_b", "spr_maincharal_b",
            "spr_doorA", "spr_doorB", "spr_doorC", "spr_doorD", "spr_doorX",
            // DELTARUNE
            "spr_krisr", "spr_krisl", "spr_krisd", "spr_krisu", "spr_kris_fall", "spr_krisr_sit",
            "spr_krisd_dark", "spr_krisr_dark", "spr_krisu_dark", "spr_krisl_dark",
            "spr_krisd_slide", "spr_krisd_slide_light",
            "spr_krisd_heart", "spr_krisd_slide_heart", "spr_krisu_heart", "spr_krisl_heart", "spr_krisr_heart",
            "spr_kris_fallen_dark", "spr_krisu_run", "spr_kris_fall_d_white", "spr_kris_fall_turnaround",
            "spr_kris_fall_d_lw", "spr_kris_fall_d_dw", "spr_kris_fall_smear", "spr_kris_dw_landed",
            "spr_kris_fall_ball", "spr_kris_jump_ball", "spr_kris_dw_land_example_dark", "spr_kris_fall_example_dark",
            "spr_krisu_fall_lw", "spr_kris_pose", "spr_kris_dance",
            "spr_kris_sword_jump", "spr_kris_sword_jump_down", "spr_kris_sword_jump_settle", "spr_kris_sword_jump_up",
            "spr_kris_coaster", "spr_kris_coaster_hurt_front", "spr_kris_coaster_hurt_back",
            "spr_kris_coaster_front", "spr_kris_coaster_empty", "spr_kris_coaster_back",
            "spr_kris_hug_left", "spr_kris_peace", "spr_kris_rude_gesture",
            "spr_kris_sit_wind", "spr_kris_hug", "spr_krisb_pirouette", "spr_krisb_bow",
            "spr_krisb_victory", "spr_krisb_defeat", "spr_krisb_attackready",
            "spr_krisb_act", "spr_krisb_actready", "spr_krisb_itemready", "spr_krisb_item",
            "spr_krisb_attack", "spr_krisb_hurt", "spr_krisb_intro", "spr_krisb_idle", "spr_krisb_defend",
            "spr_krisb_virokun", "spr_krisb_virokun_doctor", "spr_krisb_virokun_nurse", "spr_krisb_wan",
            "spr_krisb_wan_tail", "spr_krisb_wiggle",
            "spr_krisb_ready_throw_torso", "spr_krisb_ready_throw_full", "spr_krisb_throw",
            "spr_krisd_bright", "spr_krisl_bright", "spr_krisr_bright", "spr_krisu_bright",
            "spr_kris_fell",
            "spr_teacup_kris", "spr_teacup_kris_tea", "spr_teacup_kris_tea2", "spr_kris_tea",
            "spr_kris_hug_ch1",
            "spr_krisb_pirouette_ch1", "spr_krisb_bow_ch1", "spr_krisb_victory_ch1",
            "spr_krisb_defeat_ch1", "spr_krisb_attackready_ch1", "spr_krisb_act_ch1",
            "spr_krisb_actready_ch1", "spr_krisb_itemready_ch1", "spr_krisb_item_ch1",
            "spr_krisb_attack_ch1", "spr_krisb_attack_old_ch1", "spr_krisb_hurt_ch1",
            "spr_krisb_intro_ch1", "spr_krisb_idle_ch1", "spr_krisb_defend_ch1",
            "spr_kris_drop_ch1", "spr_kris_fell_ch1",
            "spr_krisr_kneel_ch1", "spr_krisd_bright_ch1", "spr_krisl_bright_ch1",
            "spr_krisr_bright_ch1", "spr_krisu_bright_ch1", "spr_krisd_heart_ch1",
            "spr_krisd_slide_heart_ch1", "spr_krisu_heart_ch1", "spr_krisl_heart_ch1",
            "spr_krisr_heart_ch1", "spr_kris_fallen_dark_ch1",
            "spr_krisd_dark_ch1", "spr_krisr_dark_ch1", "spr_krisu_dark_ch1", "spr_krisl_dark_ch1",
            "spr_krisd_slide_ch1", "spr_krisd_slide_light_ch1",
            "spr_krisr_ch1", "spr_krisl_ch1", "spr_krisd_ch1", "spr_krisu_ch1",
            "spr_krisr_sit_ch1", "spr_kris_fall_ch1",
            "spr_doorAny", "spr_doorE", "spr_doorF", "spr_doorW",
            "spr_doorE_ch1", "spr_doorF_ch1", "spr_doorW_ch1"
        };

        private static readonly DateTime _unixTimeZero = new DateTime(1970, 1, 1);
        private Random _random;

        private WindowsPrincipal windowsPrincipal;

        private UndertaleData Data;

        public MainForm()
        {
            InitializeComponent();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

                //This is so it doesn't keep starting the program over and over in case something messes up
                if (Process.GetProcessesByName("HATE").Length == 1)
                {
                    if (Directory.GetCurrentDirectory().Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) || Directory.GetCurrentDirectory().Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) && !IsElevated)
                    {
                        string message = $"The game is in a system protected folder and we need elevated permissions in order to mess with {_dataWin}.\nDo you allow us to get elevated permissions (if you press no this will just close the program as we can't do anything)";
                        if (ShowMessage(message, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // Restart program and run as admin
                            var exeName = Process.GetCurrentProcess().MainModule.FileName;
                            ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                            startInfo.Arguments = "true";
                            startInfo.Verb = "runas";
                            Process.Start(startInfo);
                            Close();
                            return;
                        }
                        else
                        {
                            Close();
                            return;
                        }
                    }
                }
            }

            _random = new Random();

            EnableControls(false);

            if (!File.Exists(_dataWin))
            {
                if (File.Exists("game.ios"))
                    _dataWin = "game.ios";
                else if (File.Exists("assets/game.unx"))
                    _dataWin = "assets/game.unx";
                else if (File.Exists("game.unx"))
                    _dataWin = "game.unx";
                else
                {
                    ShowMessage("We couldn't find any game in this folder, check that this is in the right folder.");
                    Close();
                    return;
                }
            }
        }
        private async void MainForm_Load(object sender, EventArgs e)
        {
            EnableControls(false);
            await LoadFile(_dataWin);
            this.Invoke(delegate
            {
                EnableControls(true);
                lblGameName.Text = Data.GeneralInfo.DisplayName.Content;
                UpdateCorrupt();
            });
        }

        private async Task LoadFile(string filename)
        {
            Task t = Task.Run(() =>
            {
                UndertaleData data = null;
                try
                {
                    using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        data = UndertaleIO.Read(stream, warning =>
                        {
                            this.ShowWarning(warning, "Loading warning");
                        });
                    }

                    UndertaleEmbeddedTexture.TexData.ClearSharedStream();
                }
                catch (Exception e)
                {
                    this.ShowError("An error occured while trying to load:\n" + e.Message, "Load error");
                }

                if (data != null)
                {
                    if (data.UnsupportedBytecodeVersion)
                    {
                        this.ShowWarning("Only bytecode versions 13 to 17 are supported for now, you are trying to load " + data.GeneralInfo.BytecodeVersion + ". A lot of code is disabled and something will likely break.", "Unsupported bytecode version");
                    }

                    Data = data;

                    Data.ToolInfo.ProfileMode = false;
                    Data.ToolInfo.AppDataProfiles = "";
                }
            });
            await t;

            // Clear "GC holes" left in the memory in process of data unserializing
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.gcsettings.largeobjectheapcompactionmode?view=net-6.0
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }

        public bool IsElevated
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ?
                    windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator)
                    : false;
            }
        }

        private DialogResult ShowMessage(string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, "HATE-UML", buttons, icon);
        }
        private DialogResult ShowMessage(string message)
        {
            return MessageBox.Show(message, "HATE-UML");
        }
        private DialogResult ShowWarning(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private DialogResult ShowError(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public string WindowsExecPrefix
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) /* unix platforms */
                    ? "wine " : "";
        }
        public string GetGame()
        {
            if (File.Exists("runner")) { return "runner"; }
            else if (File.Exists(Data.GeneralInfo.FileName + ".exe")) { return $"{WindowsExecPrefix}{Data.GeneralInfo.FileName}.exe"; }
            else
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                foreach (string file in Directory.EnumerateFiles(dir))
                {
                    if (Path.GetFileNameWithoutExtension(file) != "HATE" && (file.Contains(".exe") || file.Contains(".app")))
                    {
                        var executable = file.Remove(0, file.LastIndexOf("\\") + 1);
                        if (executable.EndsWith(".exe")) return $"{WindowsExecPrefix}{executable}";
                        return executable;
                    }
                }
                if (dir.IndexOf(".app") >= 0)
                {
                    return dir.Substring(0, dir.IndexOf(".app") + ".app".Length);
                }
            }
            return "";
        }

        private void btnLaunch_Clicked(object sender, EventArgs e)
        {
            EnableControls(false);

            ProcessStartInfo processStartInfo = new ProcessStartInfo(GetGame())
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Process.Start(processStartInfo);
            EnableControls(true);
        }

        private void button_Corrupt_Clicked(object sender, EventArgs e)
        {
            EnableControls(false);

            try { _logWriter = new StreamWriter("HATE.log", true); }
            catch (Exception) { MessageBox.Show("Could not set up the log file."); }

            if (!Setup()) { goto End; };
            if (_hitboxFix && !HitboxFix_Func(_random, _truePower, _logWriter)) { goto End; }
            if (_shuffleGFX && !ShuffleGFX_Func(_random, _truePower, _logWriter)) { goto End; }
            if (_shuffleText && !ShuffleText_Func(_random, _truePower, _logWriter)) { goto End; }
            if (_shuffleFont && !ShuffleFont_Func(_random, _truePower, _logWriter)) { goto End; }
            if (_shuffleBG && !ShuffleBG_Func(_random, _truePower, _logWriter)) { goto End; }
            if (_shuffleAudio && !ShuffleAudio_Func(_random, _truePower, _logWriter)) { goto End; }

            End:
            _logWriter.Close();
            EnableControls(true);
        }

        public void EnableControls(bool state)
        {
            btnCorrupt.Enabled = state;
            btnLaunch.Enabled = state;
            chbShuffleText.Enabled = state;
            chbShuffleGFX.Enabled = state;
            chbHitboxFix.Enabled = state;
            chbShuffleFont.Enabled = state;
            chbShuffleBG.Enabled = state;
            chbShuffleAudio.Enabled = state;
            label1.Enabled = state;
            label2.Enabled = state;
            txtPower.Enabled = state;
            txtSeed.Enabled = state;
        }

        public bool Setup()
        {
            _logWriter.WriteLine("-------------- Session at: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\n");

            /** SEED PARSING AND RNG SETUP **/
            _friskMode = false;
            _random = new Random();

            byte power;
            if (!byte.TryParse(txtPower.Text, out power) && _corrupt)
            {
                MessageBox.Show("Please set Power to a number between 0 and 255 and try again.");
                return false;
            }
            _truePower = (float)power / 255;

            int timeSeed = 0;
            string seed = txtSeed.Text.Trim();
            bool textSeed = false;

            if (seed.ToUpper() == "FRISK" || seed.ToUpper() == "KRIS")
                _friskMode = true;

            /* checking # for: Now it will change the corruption every time you press the button */
            else if (seed == "" || txtSeed.Text[0] == '#')
            {
                timeSeed = (int)DateTime.Now.Subtract(_unixTimeZero).TotalSeconds;

                txtSeed.Text = $"#{timeSeed}";
            }
            else if (!int.TryParse(seed, out timeSeed))
            {
                _logWriter.WriteLine($"Text seed - {txtSeed.Text.GetHashCode()}");
                _random = new Random(txtSeed.Text.GetHashCode());
                textSeed = true;
            }

            if (!textSeed)
            {
                _logWriter.WriteLine($"Numeric seed - {timeSeed}");
            }
            _logWriter.WriteLine($"Power - {power}");
            _logWriter.WriteLine($"TruePower - {_truePower}");

            /** ENVIRONMENTAL CHECKS **/
            if (File.Exists("UNDERTALE.exe") && !File.Exists("./mus_barrier.ogg"))
            {
                MessageBox.Show("ERROR:\nIt seems you've either placed HATE.exe in the wrong location or are using an old version of Undertale. Solutions to both problems are given in the README.txt file included in the download.");
                return false;
            }

            if (!File.Exists(_dataWin))
            {
                MessageBox.Show($"You seem to be missing your resource file, {_dataWin}. Make sure you've placed HATE.exe in the proper location.");
                return false;
            }
            else if (!Directory.Exists("Data"))
            {
                if (!SafeMethods.CreateDirectory("Data")) { return false; }
                if (!SafeMethods.CopyFile(_dataWin, $"Data/{_dataWin}")) { return false; }
                if (Directory.Exists("./lang"))
                {
                    if (!SafeMethods.CopyFile("./lang/lang_en.json", "./Data/lang_en.json")) { return false; };
                    if (!SafeMethods.CopyFile("./lang/lang_ja.json", "./Data/lang_ja.json")) { return false; };
                }
                _logWriter.WriteLine($"Finished setting up the Data folder.");
            }

            if (!SafeMethods.DeleteFile(_dataWin)) { return false; }
            _logWriter.WriteLine($"Deleted {_dataWin}.");
            if (Directory.Exists("./lang"))
            {
                if (!SafeMethods.DeleteFile("./lang/lang_en.json")) { return false; }
                _logWriter.WriteLine($"Deleted ./lang/lang_en.json.");
                if (!SafeMethods.DeleteFile("./lang/lang_ja.json")) { return false; }
                _logWriter.WriteLine($"Deleted ./lang/lang_ja.json.");
            }

            if (!SafeMethods.CopyFile($"Data/{_dataWin}", _dataWin)) { return false; }
            _logWriter.WriteLine($"Copied {_dataWin}.");
            if (Directory.Exists("./lang"))
            {
                if (!SafeMethods.CopyFile("./Data/lang_en.json", "./lang/lang_en.json")) { return false; }
                _logWriter.WriteLine($"Copied ./lang/lang_en.json.");
                if (!SafeMethods.CopyFile("./Data/lang_ja.json", "./lang/lang_ja.json")) { return false; }
                _logWriter.WriteLine($"Copied ./lang/lang_ja.json.");
            }

            return true;
        }
        
        public void UpdateCorrupt()
        {
            _corrupt = _shuffleGFX || _shuffleText || _hitboxFix || _shuffleFont || _shuffleAudio || _shuffleBG;
            btnCorrupt.Text = Style.GetCorruptLabel(_corrupt);
            btnCorrupt.ForeColor = Style.GetCorruptColor(_corrupt);
        }

        private void chbShuffleText_CheckedChanged(object sender, EventArgs e)
        {
            _shuffleText = chbShuffleText.Checked;
            chbShuffleText.ForeColor = Style.GetOptionColor(_shuffleText);
            UpdateCorrupt();
        }

        private void chbShuffleGFX_CheckedChanged(object sender, EventArgs e)
        {
            _shuffleGFX = chbShuffleGFX.Checked;
            chbShuffleGFX.ForeColor = Style.GetOptionColor(_shuffleGFX);
            UpdateCorrupt();
        }

        private void chbHitboxFix_CheckedChanged(object sender, EventArgs e)
        {
            _hitboxFix = chbHitboxFix.Checked;
            chbHitboxFix.ForeColor = Style.GetOptionColor(_hitboxFix);
            UpdateCorrupt();
        }

        private void chbShuffleFont_CheckedChanged(object sender, EventArgs e)
        {
            _shuffleFont = chbShuffleFont.Checked;
            chbShuffleFont.ForeColor = Style.GetOptionColor(_shuffleFont);
            UpdateCorrupt();
        }

        private void chbShuffleBG_CheckedChanged(object sender, EventArgs e)
        {
            _shuffleBG = chbShuffleBG.Checked;
            chbShuffleBG.ForeColor = Style.GetOptionColor(_shuffleBG);
            UpdateCorrupt();
        }

        private void chbShuffleAudio_CheckedChanged(object sender, EventArgs e)
        {
            _shuffleAudio = chbShuffleAudio.Checked;
            chbShuffleAudio.ForeColor = Style.GetOptionColor(_shuffleAudio);
            UpdateCorrupt();
        }

        private void txtPower_Enter(object sender, EventArgs e)
        {
            txtPower.Text = (txtPower.Text == _defaultPowerText) ? "" : txtPower.Text;
        }

        private void txtPower_Leave(object sender, EventArgs e)
        {
            txtPower.Text = string.IsNullOrWhiteSpace(txtPower.Text) ? _defaultPowerText : txtPower.Text;
        }
    }
}
