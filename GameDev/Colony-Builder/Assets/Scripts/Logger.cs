using System;
using System.Text;
using UnityEngine;

public class Logger
{
    public static event Action<object> logs;
    public static event Action<object> warnings;
    public static event Action<object> errors;

    public static void Log(object msg, GameObject gameObject)
    {
        StringBuilder ss = new StringBuilder();
        ss.Append("LOG: ");
        if (gameObject != null)
        {
            ss.Append(gameObject.name);
            ss.Append(": ");
        }
        ss.Append(msg);

        logs?.Invoke(ss);
    }

    public static void LogError(object msg, GameObject gameObject)
    {
        StringBuilder ss = new StringBuilder();
        ss.Append("ERROR: ");
        if (gameObject != null)
        {
            ss.Append(gameObject.name);
            ss.Append(": ");
        }
        ss.Append(msg);

        errors?.Invoke(ss);
    }

    public static void LogWarning(object msg, GameObject gameObject)
    {
        StringBuilder ss = new StringBuilder();
        ss.Append("WARNING: ");
        if (gameObject != null)
        {
            ss.Append(gameObject.name);
            ss.Append(": ");
        }
        ss.Append(msg);

        warnings?.Invoke(ss);
    }

}
