namespace NeonGateWeb.Global;

public class DelegatePair
{
    public string path;
    public RequestType type;
    public readonly Delegate del;
    public DelegatePair(string path, RequestType type, Delegate del)
    {
        this.del = del;
        this.path = path;
        this.type = type;
    }

    public async Task<string> Invoke(params object[] args) =>
        await (Task<string>) del.DynamicInvoke(args)!;

    public async Task<string> Invoke() => 
        await (Task<string>) del.DynamicInvoke()!;

    public async Task<string> Invoke(object arg) => 
        await (Task<string>) del.DynamicInvoke(arg)!;
}
