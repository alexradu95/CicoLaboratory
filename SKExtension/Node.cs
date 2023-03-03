using Nazar.Framework.Interface;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Framework;

/// <summary>
/// The node is the building block of Nazar
/// Each node can contain other Children nodes
/// </summary>
public abstract class Node : IChildManager, IStepper
{
    internal Dictionary<string, Node> Children = new();

    /// <summary>
    /// Adds a new node as child of the current node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="id"></param>
    /// <exception cref="Exception">Throws when an already existing child node with the same id exists</exception>
    public Node AddChild(Type node, string id = "")
    {
        Node stepperToBeAdded = (Node) SK.AddStepper(node);
        string stepperId = string.IsNullOrEmpty(id) ? $"{node.Name}" : $"{node.Name}_{id}";
        if(Children.ContainsKey(stepperId))
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

    #region IStepper methods

    public abstract bool Enabled { get; }

    public abstract bool Initialize();

    public void Shutdown()
    {
        Children.Keys.ToList().ForEach(DisableChild);
    }

    public virtual void Step()
    {
        DrawNodeManager();
    }

    public Node GetChild(string id)
    {
        return activeChildren[id];
    }

    #endregion



    private void DrawNodeManager()
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
}


