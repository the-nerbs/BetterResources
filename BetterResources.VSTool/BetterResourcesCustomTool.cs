using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace BetterResources.VSTool
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public abstract class BetterResourcesCustomTool : IVsSingleFileGenerator
    {
        private static readonly UTF8Encoding Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);


        protected abstract bool IsPublic { get; }


        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = BetterResourceGenerator.CodeFileExtension;

            return VSConstants.S_OK;
        }

        public int Generate(
            string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace, IntPtr[] rgbOutputFileContents,
            out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            string result;

            var generator = new BetterResourceGenerator
            {
                Namespace = wszDefaultNamespace,
                IsPublic = IsPublic,
            };

            if (!string.IsNullOrEmpty(wszInputFilePath))
            {
                generator.ClassName = Path.GetFileNameWithoutExtension(wszInputFilePath);
            }
            else
            {
                generator.ClassName = "Resources";
            }

            result = generator.RunOnContent(
                bstrInputFileContents,
                new VsDiagnosticReporter(pGenerateProgress)
            );

            byte[] resultBytes = Encoding.GetBytes(result);

            IntPtr ptr = Marshal.AllocCoTaskMem(result.Length);
            Marshal.Copy(resultBytes, 0, ptr, resultBytes.Length);

            rgbOutputFileContents[0] = ptr;
            pcbOutput = unchecked((uint)resultBytes.Length);

            return VSConstants.S_OK;
        }
    }
}
