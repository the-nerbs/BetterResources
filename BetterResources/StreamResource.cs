using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    sealed class StreamResource
    {
        public string Name { get; }
        public string Comment { get; }

        public bool HasComment
        {
            get { return !string.IsNullOrEmpty(Comment); }
        }


        public StreamResource(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }
    }
}
