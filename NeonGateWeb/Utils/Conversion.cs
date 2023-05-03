using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeonGateWeb.EndPoints.Attributes;
using NeonGateWeb.Global;

namespace NeonGateWeb.Utils
{
    internal static class Conversion
    {
        // todo, make an abstract base to get these from instead of conversion extensions
        // they all have a path and a type...
        public static RequestType ToRequestType(this Attribute attribute) =>
            attribute switch
            {
                Post => RequestType.Post,
                Get => RequestType.Get,
                _ => RequestType.Get
            };

        public static string ToPath(this Attribute attribute) =>
            attribute switch
            {
                Post post => post.path,
                Get get => get.path,
                _ => ""
            };

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] ToByteArray(this string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }

        public static string ToHexString(this string str)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.UTF8.GetBytes(str);
            foreach (var t in bytes)
                sb.Append(t.ToString("X2"));
            return sb.ToString();
        }

        public static string ToHexString(this int i)
        {
            return i.ToString("X2");
        }

        public static string ToHexString(this byte b)
        {
            return b.ToString("X2");
        }
    }
}