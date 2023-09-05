namespace DotnetAppTemplate.Model.Config;

public partial class Config
{
    #region OtherWriters
    /// <summary>
    /// Read settings for bool, <see cref="ReadSetting(string, string)"/>
    /// </summary>
    /// <param name="Key">Key of the setting that is used. use nameof(setting)</param>
    /// <param name="Default">The Default value of the setting</param>
    /// <returns></returns>
    public static bool ReadSetting(string Key, bool Default = false)
    {
        return bool.TryParse(ReadSetting(Key, Default.ToString()), out bool value) ? value : Default;
    }

    /// <summary>
    /// Write settings for bool, <see cref="ReadSetting(string, string)"/>
    /// </summary>
    /// <param name="Key">Key of the setting that is used. use nameof(setting)</param>
    /// <param name="Default">The Default value of the setting</param>
    /// <returns></returns>
    public static void WriteSetting(string Key, bool Value)
    {
        WriteSetting(Key, Value.ToString());
    }

    // Read Write settings for int
    public static int ReadSetting(string Key, int Default = 0)
    {
        return int.TryParse(ReadSetting(Key, Default.ToString()), out int value) ? value : Default;
    }

    public static void WriteSetting(string Key, int Value)
    {
        WriteSetting(Key, Value.ToString());
    }



    // This should work for every other type. Making life a little bit nicer for use pore programmers.
    /// <summary>
    /// A Fallback ReadSetting method that should work for every type. However, it is not really readable when you see the XML file.
    /// Should only be used on custom objects. Or a list.
    /// </summary>
    /// <typeparam name="T">Type of the data that should be stored, could be implied by <paramref name="Default"/></typeparam>
    /// <param name="Key">The Key in the config file that should be used, use nameof(setting)</param>
    /// <param name="Default">The default value of setting that should be returned when reading the setting fails.</param>
    /// <returns></returns>
    public static T ReadSetting<T>(string Key, T Default)
    {
        try
        {
            return ReadSetting(Key, Default.ToXmlString<T>()).FromXml<T>();
        }
        catch (Exception)
        {
            //RollingLogger.Warning(Key, ex.Message);
            return Default;
        }
    }

    /// <summary>
    /// A Fallback WriteSetting method that should work for every type. See <seealso cref="ReadSetting{T}(string, T)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void WriteSetting<T>(string Key, T Value)
    {
        if (Key is null)
        {
            throw new ArgumentNullException(nameof(Key));
        }
        WriteSetting(Key, Value.ToXmlString());
    }
    #endregion
}
