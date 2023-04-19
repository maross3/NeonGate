using System.Reflection;
using NeonGateWeb.EndPoints.Attributes;
using NeonGateWeb.Global;
using NeonGateWeb.Global.Extensions;
using NeonGateWeb.Server.TCP;

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
                if (attribute is Get getAttribute)
                {
                    var pair = delly.CreateDelegatePair(getAttribute.path, RequestType.Get);
                    HttpServer.MapGetRequest(getAttribute.path, pair);
                }
                else if (attribute is Post postAttribute)
                {
                    var pair = delly.CreateDelegatePair(postAttribute.path, RequestType.Post);
                    HttpServer.MapPostRequest(postAttribute.path, pair);
                }
            }
        }
    }
}