using StereoKit;
using StereoKit.Framework;

namespace nazar.core.Features.Environment
{
    // Infinite floor feature from JumliVR repo
    internal class InfiniteFloor : IStepper
    {
        public bool Enabled => true;

        Matrix floorTransform = Matrix.TS(new Vec3(0, -1.5f, 0), new Vec3(30, 0.1f, 30));
        Material floorMaterial;

        public bool Initialize()
        {
            floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            Renderer.SkyTex = Tex.FromCubemapEquirectangular("old_depot.hdr", out SphericalHarmonics lighting);
            Renderer.SkyLight = lighting;

            return true;
        }


        public void Shutdown() { }

        public void Step()
        {
            if (SK.System.displayType == Display.Opaque)
                Mesh.Cube.Draw(floorMaterial, floorTransform);

            Text.Add("North", Matrix.TRS(Vec3.Forward * 5f, Quat.FromAngles(0, 180, 0), 3), Color.Black);
            Text.Add("South", Matrix.TRS(Vec3.Forward * -5f, Quat.FromAngles(0, 0, 0), 3), Color.Black);
            Text.Add("East", Matrix.TRS(Vec3.Right * 5f, Quat.FromAngles(0, 90, 0), 3), Color.Black);
            Text.Add("West", Matrix.TRS(Vec3.Right * -5f, Quat.FromAngles(0, 270, 0), 3), Color.Black);
            Text.Add("Home", Matrix.TRS(Vec3.Up * -1.6f, Quat.FromAngles(90, 180, 0), 3), Color.Black);

        }
    }
}
