using System.Diagnostics;

namespace NeonGateWeb.Utils;

public static class NrLogger
{
    // todo:
    // add different logging methods
    // add different logging levels

    public static void Log(string message)
    {
        // this was from winforms.
        // todo; create configs for different trace logging
        // Form.LogBox.Text += $@"[{DateTime.Now.TimeOfDay.ToString()}]: " + message + "\r";
        Trace.WriteLine(message, $"[{DateTime.Now.TimeOfDay.ToString()}]");
    }
}