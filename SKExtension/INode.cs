using Nazar.Framework.Interface;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Framework;

public abstract class Node : IChildManager, IStepper
{

    Dictionary<string, Node> children = new();
    public Dictionary<string, Node> Children => children;
    public bool Enabled => true;

    public Node GetChild(string id)
    {
        return Children[id];
    }

    /// <summary>
    /// Adds a new node as child of the current node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="id"></param>
    /// <exception cref="Exception">Throws when an already existing child node with the same id exists</exception>
    public Node AddChild(Type node, string id = "")
    {
        Node stepperToBeAdded = (Node)SK.AddStepper(node);
        string stepperId = string.IsNullOrEmpty(id) ? $"{node.Name}" : $"{node.Name}_{id}";
        if (Children.ContainsKey(stepperId))
        {
            throw new Exception("Could not add the requested child. Id already exists for this node child");
        }

        Children[stepperId] = stepperToBeAdded;

        return stepperToBeAdded;
    }

    /// <summary>
    /// Deletes all child nodes that have the requested Type
    /// </summary>
    /// <param name="type"></param>
    public void DisableChildren(Type type)
    {
        var selectedChildrenIds = Children.Keys.Where(key => key.Contains(type.ToString()));
        selectedChildrenIds.ToList().ForEach(DisableChild);
    }

    /// <summary>
    /// Deletes a child by ID
    /// </summary>
    /// <param name="id"></param>
    public void DisableChild(string id)
    {
        SK.RemoveStepper(Children[id]);
        Children.Remove(id);
    }

    public abstract bool Initialize();

    public abstract void Step();

    public abstract void Shutdown();
}


