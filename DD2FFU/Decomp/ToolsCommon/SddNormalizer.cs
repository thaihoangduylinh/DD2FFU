﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.WindowsPhone.ImageUpdate.Tools.Common.SddlNormalizer
// Assembly: ToolsCommon, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b3f029d4c9c2ec30
// MVID: 8A4E8FCA-4522-42C3-A670-4E93952F2307
// Assembly location: C:\Users\gus33000\source\repos\DD2FFU\DD2FFU\libraries\ToolsCommon.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Decomp.Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
    internal static class SddlNormalizer
    {
        private static readonly HashSet<string> s_knownSids = new HashSet<string>
        {
            "AN",
            "AO",
            "AU",
            "BA",
            "BG",
            "BO",
            "BU",
            "CA",
            "CD",
            "CG",
            "CO",
            "CY",
            "EA",
            "ED",
            "ER",
            "IS",
            "IU",
            "LA",
            "LG",
            "LS",
            "LU",
            "MU",
            "NO",
            "NS",
            "NU",
            "OW",
            "PA",
            "PO",
            "PS",
            "PU",
            "RC",
            "RD",
            "RE",
            "RO",
            "RS",
            "RU",
            "SA",
            "SO",
            "SU",
            "SY",
            "WD",
            "WR"
        };

        private static readonly ConcurrentDictionary<string, string> s_map = new ConcurrentDictionary<string, string>();

        private static string ToFullSddl(string sid)
        {
            if (string.IsNullOrEmpty(sid) || sid.StartsWith("S-", StringComparison.Ordinal) ||
                s_knownSids.Contains(sid))
                return sid;
            var str = (string) null;
            if (!s_map.TryGetValue(sid, out str))
            {
                str = new SecurityIdentifier(sid).ToString();
                s_map.TryAdd(sid, str);
            }

            return str;
        }

        private static string FormatFullAccountSid(string matchGroupIndex, Match match)
        {
            var str = match.Value;
            var sid = match.Groups[matchGroupIndex].Value;
            var ch = str[str.Length - 1];
            return str.Remove(str.Length - (sid.Length + 1)) + ToFullSddl(sid) + ch;
        }

        public static string FixAceSddl(string sddl)
        {
            if (string.IsNullOrEmpty(sddl))
                return sddl;
            return Regex.Replace(sddl, "((;[^;]*){4};)(?<sid>[^;\\)]+)([;\\)])", x => FormatFullAccountSid("sid", x));
        }

        public static string FixOwnerSddl(string sddl)
        {
            if (string.IsNullOrEmpty(sddl))
                return sddl;
            return Regex.Replace(sddl, "O:(?<oid>.*?)G:(?<gid>.*)",
                x => string.Format("O:{0}G:{1}", ToFullSddl(x.Groups["oid"].Value), ToFullSddl(x.Groups["gid"].Value)));
        }
    }
}