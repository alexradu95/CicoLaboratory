using Nazar.Core.Passthrough;
using Nazar.Framework;

namespace Nazar.Core
{
    public class NazarCore : Node
    {
        public NazarCore()
        {
            AddChild(typeof(PassthroughExtension), "");
        }

        public override bool Enabled => true;


        public override bool Initialize() => true;

        public override void Step()
        {

        }
    }
}