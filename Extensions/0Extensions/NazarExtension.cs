using Nazar.Extensions.AIWorldGenerator;
using Nazar.Framework;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extensions
{
    public class NazarExtension : Node
    {

        public NazarExtension()
        {
            
        }

        public bool Enabled => true;

        public override bool Initialize()
        {
            AddChild(typeof(AiAssistant));
            return true;
        }

        public override void Shutdown()
        {

        }


        public override void Step()
        {

        }
    }
}