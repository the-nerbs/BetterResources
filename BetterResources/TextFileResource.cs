using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterResources
{
    sealed class TextFileResource
    {
        public string Name { get; }


        public TextFileResource(string name)
        {
            Name = name;
        }
    }
}
