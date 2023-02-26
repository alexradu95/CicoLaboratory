using Nazar.Extension.AIWorldGenerator;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension
{
    public class NazarExtension : IStepper
    {

        public NazarExtension()
        {


        }

        public bool Enabled => true;
        public bool Initialize() => true;
        public void Shutdown() { }
        public void Step() { }

    }
}