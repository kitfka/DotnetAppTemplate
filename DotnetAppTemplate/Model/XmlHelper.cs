using System.Xml;
using System.Xml.Serialization;

namespace DotnetAppTemplate.Model;

public static class XmlHelper
{
    /// <summary>
    /// Default readable XmlWritterSettings.
    /// Is used for all XML files in this application.
    /// </summary>
    /// <returns></returns>
    public static XmlWriterSettings DefaultXmlWriterSettings()
    {
        return new()
        {
            Indent = true,
            IndentChars = ("\t"),
            OmitXmlDeclaration = true
        };
    }

    // https://stackoverflow.com/questions/1138414/can-i-serialize-xml-straight-to-a-string-instead-of-a-stream-with-c
    public static string ToXmlString<T>(this T input)
    {
        using StringWriter writer = new();
        input.ToXml(writer);
        return writer.ToString();
    }
    public static void ToXml<T>(this T objectToSerialize,
                                Stream stream)
    {
        new XmlSerializer(typeof(T)).Serialize(stream, objectToSerialize);
    }

    public static void ToXml<T>(this T objectToSerialize,
                                StringWriter writer,
                                bool EmptyNamespace = false)
    {
        if (EmptyNamespace)
        {
            XmlSerializerNamespaces ns = new();
            ns.Add("", "");
            new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize, ns);
        }
        else
        {
            new XmlSerializer(typeof(T)).Serialize(writer, objectToSerialize);
        }
    }

    // https://stackoverflow.com/questions/14645964/deserializing-xml-from-string
    public static T FromXml<T>(this string ObjectToDeserialize)
    {
        XmlSerializer serializer = new(typeof(T));
        using TextReader reader = new StringReader(ObjectToDeserialize);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        return (T)serializer.Deserialize(reader);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    }
}