using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    sealed class ObjectResource
    {
        public string Name { get; }
        public string Type { get; }
        public string Comment { get; }

        public bool HasComment
        {
            get { return !string.IsNullOrEmpty(Comment); }
        }


        public ObjectResource(string name, string type, string comment)
        {
            Name = name;
            Type = type;
            Comment = comment;
        }
    }
}
