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


        public ObjectResource(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
