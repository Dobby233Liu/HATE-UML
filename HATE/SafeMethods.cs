using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HATE;

static class SafeMethods
{
    private static bool IsValidPath(string path)
    {
        try { Path.GetFullPath(path); } catch (Exception) { return false; } return true;
    }

    public static bool CreateDirectory(string dirname)
    {
        if (!IsValidPath(dirname)) { return false; }

        try
        {
            Directory.CreateDirectory(dirname);
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                MsgBoxHelpers.ShowError($"UnauthorizedAccessException has occured while attempting to create {dirname}. Creation of the specified directory requires permissions which this application does not have. Please remove permission requirement from the parent directory and try again.");
            else
                MsgBoxHelpers.ShowError($"{ex} has occured while attempting to create {dirname}.");
            return false;
        }
        return true;
    }

    public static bool CopyFile(string from, string to)
    {
        if (!IsValidPath(from) || !IsValidPath(to) || !File.Exists(from)) { return false; }

        try
        {
            File.Copy(from, to, true);
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                MsgBoxHelpers.ShowError($"UnauthorizedAccessException has occured while attempting to copy {from} to {to}. Please ensure that the source file doesn't require permissions to access and that destination file is not read-only.");
            else if (ex is IOException)
                MsgBoxHelpers.ShowError($"IOException has occured while attempting to copy {from} to {to}. Please ensure that the files are not in use and try again.");
            else
                MsgBoxHelpers.ShowError($"Exception {ex} has occured while attempting to copy {from} to {to}.");
            return false;
        }
        return true;
    }

    public static List<string> GetFiles(string dirname, bool alldirs = true, string format = "*.*")
    {
        if (!IsValidPath(dirname) || !Directory.Exists(dirname)) { return new List<string> (); }
            
        List<string> output = new List<string>();
        try
        {
            output = Directory.GetFiles(dirname, format, alldirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                MsgBoxHelpers.ShowError($"UnauthorizedAccessException has occured while attempting to get the list of files in {dirname}. The directory requires permissions which this application does not have to access. Please remove permission requirement from the directory and try again.");
            else if (ex is IOException)
                MsgBoxHelpers.ShowError($"IOException has occured while attempting to get the list of files in {dirname}. Please ensure that the directory is not in use and try again.");
            else
                MsgBoxHelpers.ShowError($"{ex} has occured while attempting to get the list of files in {dirname}.");
            return new List<string>();
        }
        return output;
    }

    public static bool DeleteFile(string filename)
    {
        if (!IsValidPath(filename) || !File.Exists(filename)) { return false; }

        try
        {
            File.Delete(filename);
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                MsgBoxHelpers.ShowError($"UnauthorizedAccessException has occured while attempting to delete {filename}. Please ensure that the file is neither read-only, requiring permissions to access, actually a directory all along, or a executable currently in use.");
            else if (ex is IOException)
                MsgBoxHelpers.ShowError($"IOException has occured while attempting to delete {filename}. Please ensure that the file is not in use and try again.");
            else
                MsgBoxHelpers.ShowError($"{ex} has occured while attempting to delete {filename}.");
            return false;
        }
        return true;
    }
}