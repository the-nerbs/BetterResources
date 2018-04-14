using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    partial class ResourceTemplate
    {
        public string Namespace { get; set; }
        public bool IsPublic { get; set; }
        public string ClassName { get; set; }
        public string ResourceName { get; set; }

        public List<StringResource> StringResources { get; } = new List<StringResource>();
        public List<TextFileResource> TextFileResources { get; } = new List<TextFileResource>();
        public List<ObjectResource> ObjectResources { get; } = new List<ObjectResource>();
        public List<StreamResource> StreamResources { get; } = new List<StreamResource>();

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
