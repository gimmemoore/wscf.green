using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Thinktecture.Tools.Web.Services.Wscf.Environment
{
    public static class ValidationHelper
    {
        public static bool IsWindowsFileName(string fileName)
        {
            const string windowsFileNameFormat = "^(?!^(PRN|AUX|CLOCK\\$|NUL|CON|COM\\d|LPT\\d|\\..*)(\\..+)?$)[^\\x00-\\x1f\\\\?*:\\\";|/]+$";
            Regex regex = new Regex(windowsFileNameFormat);
            return regex.IsMatch(fileName);
        }

        public static bool IsDotNetNamespace(string namespaceName)
        {
            const string dotNetNamespace = @"^(?:(?:((?![0-9_])[a-zA-Z0-9_]+)\.?)+)(?<!\.)$";
            Regex regex = new Regex(dotNetNamespace);
            return regex.IsMatch(namespaceName);
        }
    }
}
