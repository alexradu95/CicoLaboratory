using Nazar.Framework;
using StereoKit;

public class HierarchyExample : Node
{
    private readonly CubeNode _rootCubeNode = new CubeNode();
    private readonly CubeNode _childCubeNode1 = new CubeNode();
    private readonly CubeNode _childCubeNode2 = new CubeNode();
    private readonly CubeNode _grandchildCubeNode = new CubeNode();

    public HierarchyExample()
    {
        // Set up the hierarchy of nodes
        AddChild(_rootCubeNode);
        _rootCubeNode.AddChild(_childCubeNode1);
        _rootCubeNode.AddChild(_childCubeNode2);
        _childCubeNode2.AddChild(_grandchildCubeNode);

        // Set the positions of the nodes in local space
        _childCubeNode1.Pose = new Pose(new Vec3(-2f, 0, 0));
        _childCubeNode2.Pose = new Pose(new Vec3(3f, 0, 0));
        _grandchildCubeNode.Pose = new Pose(new Vec3(44, 0.5f, 0));
    }
}