using Nazar.Framework;
using StereoKit;

public class CubeNode : Node
{
    private Mesh _mesh;
    private Material _material;

    public override bool Initialize()
    {
        base.Initialize();

        _mesh = Mesh.GenerateCube(Vec3.One * 0.5f);
        _material = Material.Default;

        return true;
    }

    public override void Step()
    {
        base.Step();

        _mesh.Draw(_material, base.Pose.ToMatrix());
    }
}