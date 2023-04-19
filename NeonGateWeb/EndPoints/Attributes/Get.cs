using System.Runtime.CompilerServices;

namespace NeonGateWeb.EndPoints.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class Get : Attribute
{
    public string methodName;
    public string path;

    public Get(string path, [CallerMemberName] string name = "")
    {
        this.path = path;
        methodName = name;
    }
}