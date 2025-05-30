using System.Runtime.InteropServices;

namespace Igneous.Launcher.PInvoke;

using static Kernel32;

[StructLayout(default(LayoutKind), Size = sizeof(char) * Length)]
internal readonly struct Path
{
    internal Path(string path) => ExpandEnvironmentStrings(path, out this, Length);

    internal const int Length = 260;
}

[StructLayout(default(LayoutKind), Size = sizeof(char) * Length)]
readonly struct ApplicationUserModelId
{
    internal ApplicationUserModelId(string applicationUserModelId) => lstrcpy(this, applicationUserModelId);

    internal const int Length = 130;
}

[StructLayout(default(LayoutKind), Size = sizeof(char) * Length)]
readonly struct PackageFamilyName
{
    internal PackageFamilyName(string packageFamilyName) => lstrcpy(this, packageFamilyName);

    internal const int Length = 65;
}

[StructLayout(default(LayoutKind), Size = sizeof(char) * Length)]
readonly struct PackageFullName
{
    internal const int Length = 128;
}

readonly struct FILE_STANDARD_INFO
{
    readonly internal long AllocationSize, EndOfFile;

    readonly internal uint NumberOfLinks;

    [MarshalAs(UnmanagedType.I1)]
    readonly internal bool DeletePending, Directory;
}