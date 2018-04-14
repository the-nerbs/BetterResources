using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    /// <summary>
    /// The diagnostic severity.
    /// </summary>
    public enum DiagnosticSeverity
    {
        Error,
        Warning,
        Info,
    }

    /// <summary>
    /// Provides an interface for reporting diagnostics to clients calling into this tool.
    /// </summary>
    public interface IDiagnosticReporter
    {
        /// <summary>
        /// Reports a diagnostic.
        /// </summary>
        /// <param name="severity">The diagnostic severity.</param>
        /// <param name="message">The diagnostic message.</param>
        void Report(DiagnosticSeverity severity, string message);
    }
}
