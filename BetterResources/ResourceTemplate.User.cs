using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    partial class ResourceTemplate
    {
        internal string Namespace { get; set; }
        internal bool IsPublic { get; set; }
        internal string ClassName { get; set; }
        internal string ResourceName { get; set; }

        internal List<StringResource> StringResources { get; } = new List<StringResource>();
        internal List<TextFileResource> TextFileResources { get; } = new List<TextFileResource>();
        internal List<ObjectResource> ObjectResources { get; } = new List<ObjectResource>();
        internal List<StreamResource> StreamResources { get; } = new List<StreamResource>();

        private string ToolName
        {
            get
            {
                return typeof(ResourceTemplate).Assembly.GetName().Name;
            }
        }

        public string ToolVersion
        {
            get
            {
                return typeof(ResourceTemplate).Assembly.GetName().Version.ToString();
            }
        }

        private string AccessModifer
        {
            get
            {
                return IsPublic ? "public" : "internal";
            }
        }
    }
}
