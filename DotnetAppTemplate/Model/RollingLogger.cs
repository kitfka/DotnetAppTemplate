//#define LOG_TRACE
#define LOG_INFO
using DotnetAppTemplate.Model.Config;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace DotnetAppTemplate.Model;

// https://stackoverflow.com/questions/16949771/using-streamwriter-to-implement-a-rolling-log-and-deleting-from-top
public class RollingLogger
{
    static readonly string LOG_FILE = Path.Combine(Pragmas.FolderPath, "Logs/logfile.log");
    static readonly int MaxRolledLogCount = 9;
    static readonly int MaxLogSize = 1 * 1024 * 1024; // 1MB

    public static void LogMessage(string msg)
    {
        System.Diagnostics.Trace.WriteLine(msg); // this is only useful when debugging in the editor.
        lock (LOG_FILE) // lock is optional, but.. should this ever be called by multiple threads, it is safer
        {
            RollLogFile(LOG_FILE);
            File.AppendAllText(LOG_FILE, msg + Environment.NewLine, Encoding.UTF8);
        }
    }

    #region Write line methods
    #region Write if variants
    public static void TraceIf(bool condition, string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        if (condition)
        {
            Trace(msg, section, memberName);
        }
    }
    public static void InfoIf(bool condition, string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        if (condition)
        {
            Info(msg, section, memberName);
        }
    }

    public static void WarningIf(bool condition, string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        if (condition)
        {
            Warning(msg, section, memberName);
        }
    }

    public static void ErrorIf(bool condition, string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        if (condition)
        {
            Error(msg, section, memberName);
        }
    }
    #endregion


    public static void Trace(string msg, string section = "", [CallerMemberName] string memberName = "")
    {
#if LOG_TRACE
        LevelLogging("Trace", msg, section, memberName);
#endif
    }

    public static void Info(string msg, string section = "", [CallerMemberName] string memberName = "")
    {
#if LOG_INFO
        LevelLogging("Info", msg, section, memberName);
#endif
    }

    public static void Warning(string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        LevelLogging("Warning", msg, section, memberName);
    }

    public static void Error(string msg, string section = "", [CallerMemberName] string memberName = "")
    {
        LevelLogging("Error", msg, section, memberName);
    }

    public static void Except(Exception ex)
    {
        Error(ex.Message, ex?.StackTrace ?? "N/A", ex?.Source ?? "N/A");
    }
    #endregion

    private static void LevelLogging(string level, string msg, string section = "", string memberName = "")
    {
        if (section != "")
        {
            LogMessage($"{DateTime.Now} - {level} - {section}:{memberName}:\t {msg}");
        }
        else
        {
            LogMessage($"{DateTime.Now} - {level} - {memberName}:\t {msg}");
        }
    }

    public static void EnsureLogFolderExists()
    {
        if (!Directory.Exists(Path.Combine(Pragmas.FolderPath, "Logs")))
        {
            _ = Directory.CreateDirectory(Path.Combine(Pragmas.FolderPath, "Logs"));
        }
    }

    private static void RollLogFile(string logFilePath)
    {
        try
        {
            var length = new FileInfo(logFilePath).Length;

            if (length > MaxLogSize)
            {
                var path = Path.GetDirectoryName(logFilePath) ?? throw new Exception("LogFolderName not found");
                var wildLogName = Path.GetFileNameWithoutExtension(logFilePath) + "*" + Path.GetExtension(logFilePath);
                var bareLogFilePath = Path.Combine(path, Path.GetFileNameWithoutExtension(logFilePath));
                string[] logFileList = Directory.GetFiles(path, wildLogName, SearchOption.TopDirectoryOnly);
                if (logFileList.Length > 0)
                {
                    // only take files like logfilename.log and logfilename.0.log, so there also can be a maximum of 10 additional rolled files (0..9)
                    var rolledLogFileList = logFileList.Where(fileName => fileName.Length == (logFilePath.Length + 2)).ToArray();
                    Array.Sort(rolledLogFileList, 0, rolledLogFileList.Length);
                    if (rolledLogFileList.Length >= MaxRolledLogCount)
                    {
                        File.Delete(rolledLogFileList[MaxRolledLogCount - 1]);
                        var list = rolledLogFileList.ToList();
                        list.RemoveAt(MaxRolledLogCount - 1);
                        rolledLogFileList = list.ToArray();
                    }
                    // move remaining rolled files
                    for (int i = rolledLogFileList.Length; i > 0; --i)
                        File.Move(rolledLogFileList[i - 1], bareLogFilePath + "." + i + Path.GetExtension(logFilePath));
                    var targetPath = bareLogFilePath + ".0" + Path.GetExtension(logFilePath);
                    // move original file
                    File.Move(logFilePath, targetPath);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
}
