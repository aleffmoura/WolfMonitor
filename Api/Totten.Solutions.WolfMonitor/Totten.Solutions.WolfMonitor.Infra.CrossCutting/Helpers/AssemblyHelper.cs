using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers
{
    public static class AssemblyHelper
    {
        public static string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
        public static string AssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
    }
}
