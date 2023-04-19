using System.Linq.Expressions;
using System.Reflection;

namespace NeonGateWeb.Global.Extensions;

public static class EndPointExtensions
{
    public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
    {
        var parmTypes = methodInfo.GetParameters().Select(parm => parm.ParameterType);
        var parmAndReturnTypes = parmTypes.Append(methodInfo.ReturnType).ToArray();
        var delegateType = Expression.GetDelegateType(parmAndReturnTypes);

        if (methodInfo.IsStatic)
            return methodInfo.CreateDelegate(delegateType);
        
        return methodInfo.CreateDelegate(delegateType, target);
    }
    public static DelegatePair CreateDelegatePair(this Delegate del, string path, RequestType requestType)
    {
        return new DelegatePair(path, requestType, del);
    }

}
