using StereoKit.Framework;

namespace Nazar.Extensions
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