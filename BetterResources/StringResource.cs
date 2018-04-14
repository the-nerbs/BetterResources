using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterResources
{
    public sealed class StringResource
    {
        // Note: C# type names are non-regular (can be arbitrarily-nested generics), so we
        // just allow a general soup of identifier characters, angle brackets, and dots. If the
        // type is not valid, code compilation will fail.
        private static readonly Regex FormatPattern = new Regex(
            @"(?<type>(\w|[_.<>])+) \s+ (?<name>(\w|_)+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
        );

        private static readonly Regex FormatInsertPattern = new Regex(
            @"{ (?<index>\d+) (?<align>[,][+-]?\d+)? (?<fmt>[^}]+)? }",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
        );


        private readonly bool _isFormatString;
        private readonly string _comment;
        private readonly string _value;
        private readonly string _fmtParams;
        private readonly string _fmtCallParams;


        public string Name { get; }

        public string Comment
        {
            get { return _comment; }
        }

        public string Value
        {
            get { return _value; }
        }

        public bool IsFormatString
        {
            get { return _isFormatString; }
        }

        public string FormatParameters
        {
            get { return _fmtParams; }
        }

        public string FormatCallParameters
        {
            get { return _fmtCallParams; }
        }

        public bool HasComment
        {
            get { return !string.IsNullOrEmpty(Comment); }
        }


        public StringResource(string name, string value, string comment)
        {
            Name = name;
            _value = value;

            _comment = comment;
            _isFormatString = TryParseFormatString(value, ref _comment, out _fmtParams, out _fmtCallParams);
        }


        /// <summary>
        /// Attempts to parse a set of format arguments out of the resource comment or value.
        /// </summary>
        /// <param name="text">The resource text.</param>
        /// <param name="comment">[in,out] The resource's comment.</param>
        /// <param name="fmtParams">[out] The parameters to the resource function.</param>
        /// <param name="fmtCallArgs">[out] The arguments passed to <see cref="string.Format(string, object[])"/>.</param>
        /// <returns>True if format string information was found, or false if not.</returns>
        private static bool TryParseFormatString(string text, ref string comment, out string fmtParams, out string fmtCallArgs)
        {
            IReadOnlyDictionary<string, FormatArgument> args = null;

            comment = comment.Trim();
            if (!string.IsNullOrEmpty(comment)
                && comment[0] == '{')
            {
                int rbraceIndex = comment.IndexOf('}');
                if (rbraceIndex > 0)
                {
                    string parameterString = comment.Substring(1, rbraceIndex - 1);
                    MatchCollection formatArgMatches = FormatPattern.Matches(parameterString);

                    bool isFormatString = (formatArgMatches.Count > 0);

                    if (isFormatString)
                    {
                        args = ParseFormatArgs(text, formatArgMatches);

                        if (rbraceIndex < (comment.Length - 1))
                        {
                            comment = comment.Substring(rbraceIndex+1).TrimStart();
                        }
                        else
                        {
                            comment = string.Empty;
                        }
                    }
                }
            }
            else
            {
                // check the string itself to see if it's a format string
                MatchCollection matches = FormatInsertPattern.Matches(text);
                if (matches.Count > 0)
                {
                    args = ParseFormatStringInserts(matches);
                }
            }

            if (args != null)
            {
                fmtParams = BuildParametersString(args);
                fmtCallArgs = BuildFormatCallString(args);
                return true;
            }

            fmtParams = null;
            fmtCallArgs = null;
            return false;
        }

        /// <summary>
        /// Parses the format arguments from the resource's comment.
        /// </summary>
        /// <param name="text">The resource string value.</param>
        /// <param name="formatArgs">The collection of regex matches from the resource's comment.</param>
        /// <returns>A mapping of the resource names to <see cref="FormatArgument"/> objects.</returns>
        private static IReadOnlyDictionary<string,FormatArgument> ParseFormatArgs(string text, MatchCollection formatArgs)
        {
            var argsByName = new Dictionary<string, FormatArgument>();

            // parse out the format arguments from the Regex matches.
            for (int i = 0; i < formatArgs.Count; i++)
            {
                Match m = formatArgs[i];

                string name = m.Groups["name"].Value;
                string type = m.Groups["type"].Value;

                if (argsByName.TryGetValue(name, out FormatArgument existing))
                {
                    // if we already used this argument name, make sure the type is consistent.
                    if (string.IsNullOrEmpty(existing.TypeName)
                        && !string.IsNullOrEmpty(type))
                    {
                        existing.TypeName = type;
                    }
                }
                else
                {
                    // it's a new format argument name
                    var arg = new FormatArgument
                    {
                        Name = name,
                        TypeName = type,
                        Index = argsByName.Count,
                    };

                    argsByName.Add(name, arg);
                }
            }

            // if any arguments don't have a declared type, just make it 'object'.
            foreach (var arg in argsByName.Values)
            {
                if (string.IsNullOrEmpty(arg.TypeName))
                {
                    arg.TypeName = "object";
                }
            }

            // validate the number of arguments vs the highest index in the format string
            MatchCollection inserts = FormatInsertPattern.Matches(text);
            int maxIndex = inserts
                .OfType<Match>()
                .Select(m => m.Groups["index"].Value)
                .Select(int.Parse)
                .Max();

            if ((argsByName.Count - 1) < maxIndex)
            {
                throw new InvalidOperationException($"string expects at least {maxIndex+1} arguments, but only {argsByName.Count} were given.");
            }

            return argsByName;
        }

        /// <summary>
        /// Parses the format arguments from any format inserts in the string's value.
        /// </summary>
        /// <param name="matches">The collection of regex matches from the resource's value.</param>
        /// <returns>A mapping of the resource names to <see cref="FormatArgument"/> objects.</returns>
        private static IReadOnlyDictionary<string, FormatArgument> ParseFormatStringInserts(MatchCollection matches)
        {
            var args = new Dictionary<string, FormatArgument>();

            foreach (Match m in matches)
            {
                string name = m.Groups["index"].Value;

                if (!args.TryGetValue(name, out _))
                {
                    args.Add(name, new FormatArgument
                    {
                        Name = name,
                        TypeName = "object",
                        Index = args.Count,
                    });
                }
            }

            return args;
        }

        /// <summary>
        /// Builds the format method parameters string.
        /// </summary>
        /// <param name="args">The format arguments.</param>
        /// <returns>A string containing the parameters for the resource format method.</returns>
        private static string BuildParametersString(IReadOnlyDictionary<string, FormatArgument> args)
        {
            var builder = new StringBuilder();

            foreach (FormatArgument arg in args.Values.OrderBy(a => a.Index))
            {
                builder.Append(arg.TypeName);
                builder.Append(' ');
                builder.Append(arg.ParamName);
                builder.Append(',');
                builder.Append(' ');
            }

            if (args.Count > 0)
            {
                // trim the last ", " from the string.
                builder.Length -= 2;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Builds the parameters string to pass to <see cref="string.Format(string, object[])"/>
        /// when formatting the resource.
        /// </summary>
        /// <param name="args">The format arguments.</param>
        /// <returns>A string containing the parameters for calling <see cref="string.Format(string, object[])"/>.</returns>
        private static string BuildFormatCallString(IReadOnlyDictionary<string, FormatArgument> args)
        {
            var builder = new StringBuilder();

            foreach (FormatArgument arg in args.Values.OrderBy(a => a.Index))
            {
                builder.Append(arg.ParamName);
                builder.Append(',');
                builder.Append(' ');
            }

            if (args.Count > 0)
            {
                // trim the last ", " from the string.
                builder.Length -= 2;
            }

            return builder.ToString();
        }


        private sealed class FormatArgument
        {
            public string Name { get; set; }
            public string TypeName { get; set; }

            public int Index { get; set; }

            public string ParamName
            {
                get
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        char ch = Name[0];
                        if (('A' <= ch && ch <= 'Z')
                            || ('a' <= ch && ch <= 'z')
                            || (ch == '_'))
                        {
                            return Name;
                        }
                    }

                    return "arg" + Index;
                }
            }
        }
    }
}
