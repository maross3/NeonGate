﻿using System.Diagnostics;

namespace NeonGateWeb.Global;

public class LogTraceListener : TraceListener
{
    private readonly string _prefix;

    public LogTraceListener(string prefix) =>
        _prefix = prefix;


    public override void Write(string? message)
    {
        if (message != null && message.StartsWith(_prefix))
            Console.Write(message);
    }

    public override void WriteLine(string? message)
    {
        if (message != null && message.StartsWith(_prefix))
            Console.WriteLine(message);
    }
}