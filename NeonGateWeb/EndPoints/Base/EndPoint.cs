using System.Reflection;
using NeonGateWeb.EndPoints.Attributes;
using NeonGateWeb.Global;
using NeonGateWeb.Global.Extensions;
using NeonGateWeb.Server.TCP;
using NeonGateWeb.Utils;

namespace NeonGateWeb.EndPoints.Base;

public abstract class EndPoint
{
    protected EndPoint()
    {
        var type = GetType();
        var methods = type.GetMethods();
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                var delly = method.CreateDelegate(this);
                var pair = delly.CreateDelegatePair(attribute.ToPath(), attribute.ToRequestType());
                switch (attribute)
                {
                    case Get getAttribute:
                        HttpServer.MapGetRequest(getAttribute.path, pair);
                        break;
                    case Post postAttribute:
                        HttpServer.MapPostRequest(postAttribute.path, pair);
                        break;
                }
            }
        }
    }
}