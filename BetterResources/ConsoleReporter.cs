using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    /// <summary>
    /// An implementation of <see cref="IDiagnosticReporter"/> which writes messages to the console.
    /// </summary>
    public sealed class ConsoleReporter : IDiagnosticReporter
    {
        private const string DefaultFormat = "{0} : {1}";


        private string _format = DefaultFormat;


        /// <summary>
        /// The format string used to format diagnostic messages.
        /// </summary>
        /// <remarks>
        /// The format string uses 2 arguments:
        /// Arg 0: The severity as a <see cref="DiagnosticSeverity"/>.
        /// Arg 1: The diagnostic message as a <see cref="string"/>.
        /// </remarks>
        public string DiagnosticFormat
        {
            get { return _format; }
            set
            {
                _format = string.IsNullOrEmpty(value)
                    ? DefaultFormat
                    : value;
            }
        }


        /// <inheritdoc cref="IDiagnosticReporter.Report(DiagnosticSeverity, string)" />
        public void Report(DiagnosticSeverity severity, string message)
        {
            Console.WriteLine(DiagnosticFormat, severity, message);
        }
    }
}
