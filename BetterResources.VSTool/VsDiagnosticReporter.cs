using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace BetterResources.VSTool
{
    internal sealed class VsDiagnosticReporter : IDiagnosticReporter
    {
        private readonly IVsGeneratorProgress _progress;


        public VsDiagnosticReporter(IVsGeneratorProgress progress)
        {
            _progress = progress;
        }


        public void Report(DiagnosticSeverity severity, string message)
        {
            if (_progress != null
                && severity != DiagnosticSeverity.Info)
            {
                int isWarning = (severity == DiagnosticSeverity.Warning) ? 1 : 0;

                _progress.GeneratorError(isWarning, 0, message, ~0u, ~0u);
            }
        }
    }
}
