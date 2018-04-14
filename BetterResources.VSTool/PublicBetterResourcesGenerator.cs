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
        typeof(PublicBetterResourcesGenerator),
        "Nerbs Better Resources Generator (public class)",
        VSConstants.UICONTEXT.CSharpProject_string,
        GeneratorRegKeyName = nameof(PublicBetterResourcesGenerator),
        GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(PublicBetterResourcesGenerator), RegisterUsing = RegistrationMethod.CodeBase)]
    public sealed class PublicBetterResourcesGenerator : BetterResourcesCustomTool
    {
        public const string CLSID = "A632CAE9-8395-4F08-9E8C-66B9BA777C64";


        protected override bool IsPublic
        {
            get { return true; }
        }
    }
}
