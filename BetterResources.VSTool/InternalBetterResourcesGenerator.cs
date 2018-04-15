using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace BetterResources.VSTool
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid(CLSID)]
    [CodeGeneratorRegistration(
        typeof(InternalBetterResourcesGenerator),
        "Nerbs Better Resources Generator",
        ContextGuids.CSharpDesktopProjectGuid,
        GeneratorRegKeyName = nameof(InternalBetterResourcesGenerator),
        GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(
        typeof(InternalBetterResourcesGenerator),
        "Nerbs Better Resources Generator",
        ContextGuids.CSharpNetStandardProjectGuid,
        GeneratorRegKeyName = nameof(InternalBetterResourcesGenerator),
        GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(InternalBetterResourcesGenerator))]
    public sealed class InternalBetterResourcesGenerator : BetterResourcesCustomTool
    {
        public const string CLSID = "0741EF37-2F9C-4887-9BB6-4D34E5F32000";


        protected override bool IsPublic
        {
            get { return false; }
        }
    }
}
