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

        public StreamResource(string name)
        {
            Name = name;
        }
    }
}
