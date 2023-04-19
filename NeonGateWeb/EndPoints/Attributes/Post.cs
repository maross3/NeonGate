using System.Runtime.CompilerServices;

namespace NeonGateWeb.EndPoints.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal class Post : Attribute
{
    public string methodName;
    public string path;

    public Post(string path, [CallerMemberName] string name = "")
    {
        this.path = path;
        methodName = name;
    }
}