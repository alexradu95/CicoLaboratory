using Nazar.Core.Passthrough;
using Nazar.Framework;

namespace Nazar.Core
{
    public class NazarCore : Node
    {
        public bool Enabled => true;

        public override bool Initialize()
        {
            AddChild(typeof(PassthroughCore));
            return true;
        }

        public override void Step()
        {

        }

        public override void Shutdown()
        {

        }


    }
}