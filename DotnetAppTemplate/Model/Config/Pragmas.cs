namespace DotnetAppTemplate.Model.Config;

public static class Pragmas
{
    public static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DotnetAppTemplate");
}
