using Nazar.Framework.Interface;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Framework;

/// <summary>
/// The node is the building block of Nazar
/// Each node cand contain other children nodes
/// </summary>
public abstract class ConfigurableNode : IChildManager, IConfigurable
{
    internal List<Type> allChildren = new();
    internal Dictionary<string, Node> activeChildren = new();

    private Pose menuPose;

    public ConfigurableNode()
    {
        menuPose.position = new Vec3(0, 0, -0.6f);
        menuPose.orientation = Quat.LookDir(-Vec3.Forward);
    }

    /// <summary>
    /// Adds a new node as child of the current node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="id"></param>
    /// <exception cref="Exception">Throws when an already existing child node with the same id exists</exception>
    public Node AddChild(Type node, string id = "")
    {
        allChildren.Add(node);
        var stepperToBeAdded = (Node) SK.AddStepper(node);
        var stepperId = string.IsNullOrEmpty(id) ? $"{node.Name}" : $"{node.Name}_{id}";
        if(activeChildren.ContainsKey(stepperId))
        {
            throw new Exception("Could not add the requested child. Id already exists for this node child");
        }
        activeChildren[stepperId] = stepperToBeAdded;

        return stepperToBeAdded;
    }

    /// <summary>
    /// Deletes all child nodes that have the requested Type
    /// </summary>
    /// <param name="type"></param>
    public void DisableChildren(Type type)
    {
        var selectedChildrenIds = activeChildren.Keys.Where(key => key.Contains(type.ToString()));
        selectedChildrenIds.ToList().ForEach(DisableChild);
    }

    /// <summary>
    /// Deletes a child by ID
    /// </summary>
    /// <param name="id"></param>
    public void DisableChild(string id)
    {
        SK.RemoveStepper(activeChildren[id]);
        activeChildren.Remove(id);
    }


    public void DrawNodeManager()
    {
        UI.WindowBegin(this.GetType().ToString(), ref menuPose, new Vec2(50 * U.cm, 0));
        foreach (string demoName in allChildren.Select(el => el.Name))
        {
            // If the button is pressed
            if (UI.Button(demoName))
            {
                if (!activeChildren.ContainsKey(demoName))
                {
                    EnableChild(demoName);
                }
                else
                {
                    DisableChild(demoName);
                }
            }

            UI.SameLine();
        }

        UI.NextLine();
        UI.HSeparator();
        UI.WindowEnd();
    }

    private void EnableChild(string demoName)
    {
        Type featureType = allChildren.FirstOrDefault(el => el.Name == demoName);
        activeChildren[demoName] = (Node)SK.AddStepper(featureType);
    }

    public Node GetChild(string id)
    {
        return activeChildren[id];
    }

    public abstract void DrawConfigurationUI();

}


