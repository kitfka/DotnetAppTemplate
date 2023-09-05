using System.Text;
using System.Xml;
using static DotnetAppTemplate.Model.Config.Pragmas;

namespace DotnetAppTemplate.Model.Config;

// TODO I don't want a static class anymore!
public partial class Config
{
    public static bool IsEnabled { get; set; } = false;

    /// <summary>
    /// Call this method at the start of you application.
    /// </summary>
    public static void Init()
    {
        IsEnabled = true;
        EnsureConfigDirectoryExists();
    }



    public static readonly string ConfigFile = "Config.xml";
    public static readonly string FilePath = Path.Combine(FolderPath, ConfigFile);
    public static readonly string TestConfigFile = "TestConfig.xml";
    public static readonly string TestFilePath = Path.Combine(FolderPath, TestConfigFile);
    private static bool _initialized = false;
    private static bool _test_mode = false;
    private static readonly bool UseCashe = true;

    private static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

    public static readonly Encoding DefaultEncoding = Encoding.UTF8;


    /// <summary>
    /// The default Settings.xml content. 
    /// The weird Doctype root thing is needed to enable the GetElementById call we use for reading and writing settings.
    /// </summary>
    public static readonly string DefaultXML = "<?xml version=\"1.0\"?> \n" +
            "<!DOCTYPE root [ \n" +
            "  <!ELEMENT root ANY>   \n" +
            "  <!ELEMENT Setting ANY>  \n" +
            "  <!ATTLIST Setting Key ID #REQUIRED>]>  \n" +
            "<root>\n" +
            " 	<Setting Key=\"DefaultFilePath\">c:\\temp</Setting> \n" +
            "</root>";



    public static void EnsureConfigDirectoryExists()
    {
        if (!Directory.Exists(FolderPath))
        {
            _ = Directory.CreateDirectory(FolderPath);
        }
    }

    public static void ClearCash() => cache.Clear();

    public static void ClearConfig()
    {
        ClearCash();
        File.Delete(FilePath);
        GetXmlDocument();
    }

    private static XmlDocument GetXmlDocument()
    {
        XmlDocument doc = new()
        {
            PreserveWhitespace = true
        };

        try { doc.Load(FilePath); }
        catch (FileNotFoundException)
        { 
            doc.LoadXml(DefaultXML);
            doc.Save(FilePath);
        }
        return doc;
    }


    #region Default string read/writer
    /// <summary>
    /// Base method to retrieve an setting.
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Default">Default value that will be returned if the key does not exist</param>
    /// <returns>Value of the element related to the key</returns>
    public static string ReadSetting(string Key, string Default = "")
    {
        if (UseCashe)
        {
            if (cache.ContainsKey(Key))
            {
                return cache[Key];
            }
        }
        if (_test_mode) { return Default; }
        lock (FilePath)
        {
            if (!_initialized)
            {
                _initialized = true;
                EnsureConfigDirectoryExists();
            }
            XmlDocument doc = GetXmlDocument();
            string res = doc?.GetElementById(Key)?.InnerText ?? Default;
            if (UseCashe)
            {
                cache.Add(Key, res);
            }

            return res;
        }
    }

    /// <summary>
    /// Write to existing setting, or create a new entry.
    /// </summary>
    /// <param name="Key">Name of the setting</param>
    /// <param name="Value">New value of the setting</param>
    public static void WriteSetting(string Key, string Value)
    {
        if (UseCashe)
        {
            cache[Key] = Value;
        }
        if (_test_mode) { return; }

        lock (FilePath)
        {
            if (!_initialized)
            {
                _initialized = true;
                EnsureConfigDirectoryExists();
            }
            XmlDocument doc = GetXmlDocument();
            if (doc == null)
            {
                return;
            }
            XmlElement? a = doc.GetElementById(Key);
            if (a != null)
            {
                a.InnerText = Value;
            }
            else
            {
                XmlElement NewSetting = doc.CreateElement("Setting");
                NewSetting.InnerText = Value;
                XmlAttribute attribute = doc.CreateAttribute("Key");
                attribute.Value = Key;
                NewSetting.Attributes.Append(attribute);

                if (doc.LastChild != null)
                {
                    doc.LastChild.AppendChild(NewSetting);
                    doc.LastChild.InnerXml = doc.LastChild.InnerXml.Replace(NewSetting.OuterXml, "	" + NewSetting.OuterXml + "\n");
                }
                else
                {
                    throw new Exception("LastChild is null" + nameof(WriteSetting));
                }
            }
            doc.Save(FilePath);
        }
    }
    #endregion
}
