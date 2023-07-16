using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using UndertaleModLib;
using UndertaleModLib.Models;
using UndertaleModLib.Util;

namespace HATE;

public partial class MainForm : Form
{
    private static class OurStyle
    {
        private static readonly Color _btnRestoreColor = Colors.LimeGreen;
        private static readonly Color _btnCorruptColor = Colors.Coral;
        private static readonly string _btnRestoreLabel = " -RESTORE- ";
        private static readonly string _btnCorruptLabel = " -CORRUPT- ";
        private static readonly Color _optionSet = Colors.Yellow;
        private static readonly Color _optionUnset = Colors.White;

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
    private string _dataWin = "data.win";

    private static readonly DateTime _unixTimeZero = new DateTime(1970, 1, 1);
    private Random _random;

    private WindowsPrincipal windowsPrincipal;

    private UndertaleData Data;
    private string PossibleFileName;

    private bool _controlEnabled = true;

    private void MainForm_PreLoad(object sender, EventArgs e)
    {
        _random = new Random();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
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
                MsgBoxHelpers.ShowMessage("We couldn't find any game data in this folder, check if HATE is in the right folder.");
                QuitEarly();
                return;
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            //This is so it doesn't keep starting the program over and over in case something messes up
            if (Process.GetProcessesByName("HATE").Length == 1)
            {
                if (!IsElevated &&
                    (Directory.GetCurrentDirectory().StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
                     || Directory.GetCurrentDirectory().StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86))))
                {
                    string message = $"The game is in a protected folder, and we need elevated permissions in order to mess with {_dataWin}.\nDo you allow us to get elevated permissions? If you choose not to, the application will close.";
                    if (MsgBoxHelpers.ShowMessage(message, MessageBoxButtons.YesNo, MessageBoxType.Information) == DialogResult.Yes)
                    {
                        // Restart program and run as admin
                        var exeName = Process.GetCurrentProcess().MainModule.FileName;
                        ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                        startInfo.Verb = "runas";
                        Process.Start(startInfo);
                    }
                    QuitEarly();
                    return;
                }
            }
        }
    }

    private void QuitEarly()
    {
        // BUG: In WPF mode, if you do either Close() or this
        // during initialization Eto will still attempt to show this form
        // In WinForms mode doing Close() will cause a exception and
        // doing this will cause the window to show for a moment then close
        Application.Instance.Quit();
    }

    private async void MainForm_LoadComplete(object sender, EventArgs e)
    {
        EnableControls(false);
        await LoadFile(_dataWin, true);
        AfterDataLoad();
        if (Data is not null)
        {
            var displayName = Data.GeneralInfo.DisplayName.Content.ToLower();
            if (!(displayName.StartsWith("undertale") || displayName.StartsWith("deltarune")))
                MsgBoxHelpers.ShowWarning("HATE-UML's support for this game may be limited. Unless this is a mod of a Toby Fox game, DO NOT report any problems you might experience.", "Not a Toby Fox game");
            PossibleFileName = Data.GeneralInfo.FileName?.Content;
            DisposeGameData();
        }
    }
    private void AfterDataLoad()
    {
        Application.Instance.Invoke(delegate
        {      
            if (Data is not null)
            {
                EnableControls(true);
                SetGameName(Data.GeneralInfo.DisplayName.Content);
            }
            UpdateCorrupt();
        });
    }

    private void SetGameName(string content)
    {
        if (Eto.Platform.Instance.IsWinForms)
            content = content.Replace("&", "&&");
        lblGameName.Text = content;
    }

    private void DisposeGameData()
    {
        if (Data is not null)
        {
            Data.Dispose();
            Data = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
        }
    }

    private async Task LoadFile(string filename, bool firstLoad)
    {
        DisposeGameData();
        Task t = Task.Run(() =>
        {
            UndertaleData data = null;
            try
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    data = UndertaleIO.Read(stream, warning =>
                    {
                        if (firstLoad)
                            MsgBoxHelpers.ShowWarning(warning, "Loading warning");
                        if (_logWriter != null)
                            _logWriter.WriteLine(warning);
                    }, message =>
                    {
                        Application.Instance.Invoke(delegate
                        {
                            SetGameName(message);
                        });
                        if (_logWriter != null)
                            _logWriter.WriteLine(message);
                    });
                }

                UndertaleEmbeddedTexture.TexData.ClearSharedStream();
            }
            catch (Exception e)
            {
                MsgBoxHelpers.ShowError("An error occured while trying to load:\n" + e.Message, "Load error");
                if (_logWriter != null)
                    _logWriter.WriteLine("An error occured while trying to load:\n" + e.Message);
            }

            if (data != null)
            {
                if (data.UnsupportedBytecodeVersion && firstLoad)
                {
                    MsgBoxHelpers.ShowWarning("Only bytecode versions 13 to 17 are supported for now, you are trying to load " + data.GeneralInfo.BytecodeVersion + ". A lot of code is disabled and something will likely break.", "Unsupported bytecode version");
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
    private async Task SaveFile(string filename)
    {
        if (Data == null)
            return;

        Task t = Task.Run(() =>
        {
            bool SaveSucceeded = true;

            try
            {
                using (var stream = new FileStream(filename + "temp", FileMode.Create, FileAccess.Write))
                {
                    UndertaleIO.Write(stream, Data, message =>
                    {
                        Application.Instance.Invoke(delegate
                        {
                            SetGameName(message);
                        });
                        if (_logWriter != null)
                            _logWriter.WriteLine(message);
                    });
                }

                UndertaleEmbeddedTexture.TexData.ClearSharedStream();
                QoiConverter.ClearSharedBuffer();
            }
            catch (Exception e)
            {
                if (!UndertaleIO.IsDictionaryCleared)
                {
                    try
                    {
                        var listChunks = Data.FORM.Chunks.Values.Select(x => x as IUndertaleListChunk);
                        Parallel.ForEach(listChunks.Where(x => x is not null), (chunk) =>
                        {
                            chunk.ClearIndexDict();
                        });

                        UndertaleIO.IsDictionaryCleared = true;
                    }
                    catch { }
                }

                MsgBoxHelpers.ShowError("An error occured while trying to save:\n" + e.Message, "Save error");
                if (_logWriter != null)
                    _logWriter.WriteLine("An error occured while trying to save:\n" + e.Message);

                SaveSucceeded = false;
            }
            // Don't make any changes unless the save succeeds.
            try
            {
                if (SaveSucceeded)
                {
                    // It saved successfully!
                    // If we're overwriting a previously existing data file, we're going to delete it now.
                    // Then, we're renaming it back to the proper (non-temp) file name.
                    if (File.Exists(filename))
                        File.Delete(filename);
                    File.Move(filename + "temp", filename);
                }
                else
                {
                    // It failed, but since we made a temp file for saving, no data was overwritten or destroyed (hopefully)
                    // We need to delete the temp file though (if it exists).
                    if (File.Exists(filename + "temp"))
                        File.Delete(filename + "temp");
                }
            }
            catch (Exception exc)
            {
                MsgBoxHelpers.ShowError("An error occured while trying to save:\n" + exc.Message, "Save error");

                SaveSucceeded = false;
            }

            return Task.CompletedTask;
        });
        await t;

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

    public string WindowsExecPrefix
    {
        get => RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
               || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
               || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) /* unix platforms */
            ? "wine " : "";
    }
    public string GetGame()
    {
        try
        {
            if (File.Exists("runner")) { return "runner"; }
            else if (PossibleFileName is not null && File.Exists(PossibleFileName + ".exe")) { return $"{PossibleFileName}.exe"; }
            else
            {
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                foreach (string file in SafeMethods.GetFiles(dir))
                {
                    if (Path.GetFileNameWithoutExtension(file) != "HATE" && file.EndsWith(".exe"))
                    {
                        var executable = Path.GetFileName(file);
                        return executable;
                    }
                }
                if (dir.IndexOf(".app") >= 0)
                {
                    return dir.Substring(0, dir.IndexOf(".app") + ".app".Length);
                }
            }
        }
        catch { }
        return "";
    }

    private void btnLaunch_Clicked(object sender, EventArgs e)
    {
        EnableControls(false);

        string game = GetGame();
        if (game is not null && game != "")
        {
            bool wine = game.EndsWith(".exe") && WindowsExecPrefix.Length > 0;
            ProcessStartInfo processStartInfo = new ProcessStartInfo(wine ? WindowsExecPrefix : game)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = Path.GetDirectoryName(game)
            };
            if (wine)
                processStartInfo.ArgumentList.Add(game);
            var process = Process.Start(processStartInfo);
            if (process is null || process.HasExited)
                MsgBoxHelpers.ShowError("Failed to start the game.");
        }
        else
            MsgBoxHelpers.ShowError("I don't know how to run this game (can't find a executable).\nPlease run the game manually.");

        EnableControls(true);
    }

    enum CorruptErrorType : int { Success, Failure, Exception }

    private async void button_Corrupt_Clicked(object sender, EventArgs e)
    {
        EnableControls(false);

        try { _logWriter = new StreamWriter("HATE.log", true); }
        catch (Exception) { MsgBoxHelpers.ShowError("Could not set up the log file."); }

        Task<CorruptErrorType> t = Task.Run(async () =>
        {
            try
            {
                if (!await Setup()) return CorruptErrorType.Failure;
                if (_shuffleGFX && !Functionality.ShuffleGFX_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
                if (_hitboxFix && !Functionality.HitboxFix_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
                if (_shuffleText && !Functionality.ShuffleText_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
                if (_shuffleFont && !Functionality.ShuffleFont_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
                if (_shuffleBG && !Functionality.ShuffleBG_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
                if (_shuffleAudio && !Functionality.ShuffleAudio_Func(Data, _random, _truePower, _logWriter, _friskMode))
                    return CorruptErrorType.Failure;
            } catch (Exception ex)
            {
                MsgBoxHelpers.ShowError(ex.ToString());
                return CorruptErrorType.Exception;
            }
            return CorruptErrorType.Success;
        });

        CorruptErrorType result = await t;
        _logWriter.Close();
        _logWriter = null;
        if (result == CorruptErrorType.Success)
        {
            await SaveFile(_dataWin);
            AfterDataLoad();
            DisposeGameData();
        }
        /*else if (result == CorruptErrorType.Failure)
        {
            _logWriter.WriteLine("An error ocurred.");
            MsgBoxHelpers.ShowError("HATE.log may have described this error in detail.", "An error ocurred");
        }*/
        EnableControls(true);
    }

    public void EnableControls(bool state)
    {
        _controlEnabled = state;
        btnCorrupt.Enabled = state;
        btnLaunch.Enabled = state;
        chbShuffleText.Enabled = state;
        chbShuffleGFX.Enabled = state;
        chbHitboxFix.Enabled = state;
        chbShuffleFont.Enabled = state;
        chbShuffleBG.Enabled = state;
        chbShuffleAudio.Enabled = state;
        txtPower.Enabled = state;
        txtSeed.Enabled = state;
    }

    public async Task<bool> Setup()
    {
        _logWriter.WriteLine("-------------- Session at: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\n");

        byte power;
        if (!byte.TryParse(Application.Instance.Invoke(delegate { return txtPower.Text; }), out power) && _corrupt)
        {
            MsgBoxHelpers.ShowError("Please set Power to a number between 0 and 255 and try again.");
            return false;
        }
        _truePower = (float)power / 255;

        /** SEED PARSING AND RNG SETUP **/
        _friskMode = false;

        int timeSeed = 0;
        string seed = Application.Instance.Invoke(delegate { return txtSeed.Text; }).Trim();
        _logWriter.WriteLine($"Specified seed - {seed}");

        if (seed.ToUpper() == "FRISK" || seed.ToUpper() == "KRIS")
        {
            _friskMode = true;
            _logWriter.WriteLine($"Frisk mode");
        }

        /* checking # for: Now it will change the corruption every time you press the button */
        if (seed == "" || seed.StartsWith('#'))
        {
            timeSeed = (int)DateTime.Now.Subtract(_unixTimeZero).TotalSeconds;
            Application.Instance.Invoke(delegate { txtSeed.Text = $"#{timeSeed}"; });
        }
        else if (!int.TryParse(seed, out timeSeed))
        {
            timeSeed = seed.GetHashCode();
        }

        _logWriter.WriteLine($"Using seed - {timeSeed}");

        _random = new Random(timeSeed);

        _logWriter.WriteLine($"Power - {power}");
        _logWriter.WriteLine($"TruePower - {_truePower}");

        /** ENVIRONMENTAL CHECKS **/
        if (!File.Exists(_dataWin))
        {
            MsgBoxHelpers.ShowError($"You seem to be missing your resource file, {_dataWin}. Make sure you've placed HATE.exe in the proper location.");
            return false;
        }
        else if (!Directory.Exists("HATE_backup") || !File.Exists($"HATE_backup/{_dataWin}"))
        {
            if (!SafeMethods.CreateDirectory("HATE_backup")) { return false; }
            if (!SafeMethods.CopyFile(_dataWin, $"HATE_backup/{_dataWin}")) { return false; }
            if (Directory.Exists("lang"))
            {
                foreach (string file in SafeMethods.GetFiles("lang", true, "*.json"))
                {
                    if (!SafeMethods.CopyFile(file, $"HATE_backup/{Path.GetFileName(file)}")) { return false; }
                }
            }
            _logWriter.WriteLine($"Finished setting up the Data folder.");
        }
        else
        {
            /* restore backup */
            if (!SafeMethods.DeleteFile(_dataWin)) { return false; }
            _logWriter.WriteLine($"Deleted {_dataWin}.");
            if (Directory.Exists("lang"))
            {
                foreach (string file in SafeMethods.GetFiles("lang", true, "*.json"))
                {
                    if (!SafeMethods.DeleteFile(file)) { return false; }
                    _logWriter.WriteLine($"Deleted {file}.");
                }
            }

            if (!SafeMethods.CopyFile($"HATE_backup/{_dataWin}", _dataWin)) { return false; }
            _logWriter.WriteLine($"Copied {_dataWin}.");
            if (Directory.Exists("lang"))
            {
                foreach (string file in SafeMethods.GetFiles("HATE_backup", true, "*.json"))
                {
                    if (!SafeMethods.CopyFile(file, $"lang/{Path.GetFileName(file)}")) { return false; }
                    _logWriter.WriteLine($"Copied {file}.");
                }
            }
        }

        await LoadFile(_dataWin, false);
        Application.Instance.Invoke(delegate
        {
            if (Data is not null)
            {
                SetGameName(Data.GeneralInfo.DisplayName.Content);
            }
        });

        return Data is not null;
    }

    public void UpdateCorrupt()
    {
        _corrupt = _shuffleGFX || _shuffleText || _hitboxFix || _shuffleFont || _shuffleAudio || _shuffleBG;
        btnCorrupt.Text = OurStyle.GetCorruptLabel(_corrupt);
        btnCorrupt.TextColor = OurStyle.GetCorruptColor(_corrupt);
    }

    private void chbShuffleText_CheckedChanged(object sender, EventArgs e)
    {
        _shuffleText = chbShuffleText.Checked.Value;
        chbShuffleText.TextColor = OurStyle.GetOptionColor(_shuffleText);
        UpdateCorrupt();
    }

    private void chbShuffleGFX_CheckedChanged(object sender, EventArgs e)
    {
        _shuffleGFX = chbShuffleGFX.Checked.Value;
        chbShuffleGFX.TextColor = OurStyle.GetOptionColor(_shuffleGFX);
        UpdateCorrupt();
    }

    private void chbHitboxFix_CheckedChanged(object sender, EventArgs e)
    {
        _hitboxFix = chbHitboxFix.Checked.Value;
        chbHitboxFix.TextColor = OurStyle.GetOptionColor(_hitboxFix);
        UpdateCorrupt();
    }

    private void chbShuffleFont_CheckedChanged(object sender, EventArgs e)
    {
        _shuffleFont = chbShuffleFont.Checked.Value;
        chbShuffleFont.TextColor = OurStyle.GetOptionColor(_shuffleFont);
        UpdateCorrupt();
    }

    private void chbShuffleBG_CheckedChanged(object sender, EventArgs e)
    {
        _shuffleBG = chbShuffleBG.Checked.Value;
        chbShuffleBG.TextColor = OurStyle.GetOptionColor(_shuffleBG);
        UpdateCorrupt();
    }

    private void chbShuffleAudio_CheckedChanged(object sender, EventArgs e)
    {
        _shuffleAudio = chbShuffleAudio.Checked.Value;
        chbShuffleAudio.TextColor = OurStyle.GetOptionColor(_shuffleAudio);
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

    private void MainForm_FormClosing(object sender, CancelEventArgs e)
    {
        if (!_controlEnabled)
            e.Cancel = true;
    }
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
