using Nazar.Framework;
using StereoKit;

namespace NazAR
{
    public class Launcher : Node
    {

        private Mesh _mesh;
        private Material _material;

        public Launcher()
        {


        }

        public override bool Initialize()
        {

            _mesh = Mesh.GenerateCube(Vec3.One * 1f);
            _material = Material.Default;

            return base.Initialize();


        }

        public override void Step()
        {
            base.Step();

            _mesh.Draw(_material, base.Pose.ToMatrix());
        }

        public SKSettings Settings => new()
        {
            appName = "nazar.OS",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

    }
}