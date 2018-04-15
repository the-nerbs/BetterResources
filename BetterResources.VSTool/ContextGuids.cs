using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace BetterResources.VSTool
{
    static class ContextGuids
    {
        /// <summary>
        /// Context GUID for C# projects targeting a .NET desktop framework.
        /// </summary>
        public const string CSharpDesktopProjectGuid = VSConstants.UICONTEXT.CSharpProject_string;

        /// <summary>
        /// Context GUID for C# projects targeting a .NET Standard framework.
        /// </summary>
        public const string CSharpNetStandardProjectGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
    }
}
