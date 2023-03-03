using StereoKit;

namespace Nazar.Framework.Interface
{
    public interface IChildManager
    {
        Dictionary<string, INode> Children { get; }

        INode GetChild(string id)
        {
            return Children[id];
        }

        /// <summary>
        /// Adds a new node as child of the current node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id"></param>
        /// <exception cref="Exception">Throws when an already existing child node with the same id exists</exception>
        INode AddChild(Type node, string id = "")
        {
            INode stepperToBeAdded = (INode)SK.AddStepper(node);
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
        void DisableChildren(Type type)
        {
            var selectedChildrenIds = Children.Keys.Where(key => key.Contains(type.ToString()));
            selectedChildrenIds.ToList().ForEach(DisableChild);
        }

        /// <summary>
        /// Deletes a child by ID
        /// </summary>
        /// <param name="id"></param>
        void DisableChild(string id)
        {
            SK.RemoveStepper(Children[id]);
            Children.Remove(id);
        }
    }
}
