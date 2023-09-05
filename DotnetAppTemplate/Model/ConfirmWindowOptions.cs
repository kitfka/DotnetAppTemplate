namespace DotnetAppTemplate.Model;

[Flags]
public enum ConfirmWindowOptions
{
    None = 0,
    CloseOnEnter = 1 << 0,
    Fullscreen = 1 << 1,
    SelfDestruct1s = 1 << 2,
}