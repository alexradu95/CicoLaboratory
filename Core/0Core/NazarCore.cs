using System.Collections.Generic;
using Nazar.Core.Passthrough;
using Nazar.Framework;

namespace Nazar.Core
{
    public class NazarCore : INode
    {

        public Dictionary<string, INode> Children { get; }

        public bool Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Step()
        {
            throw new System.NotImplementedException();
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        public bool Enabled { get; }
    }
}