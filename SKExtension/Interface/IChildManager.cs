using StereoKit;

namespace Nazar.Framework.Interface
{
    public interface IChildManager
    {
        Dictionary<string, Node> Children { get; }

        Node GetChild(string id);

        /// <summary>
        /// Adds a new node as child of the current node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id"></param>
        /// <exception cref="Exception">Throws when an already existing child node with the same id exists</exception>
        Node AddChild(Type node, string id = "");

        /// <summary>
        /// Deletes all child nodes that have the requested Type
        /// </summary>
        /// <param name="type"></param>
        void DisableChildren(Type type);

        /// <summary>
        /// Deletes a child by ID
        /// </summary>
        /// <param name="id"></param>
        void DisableChild(string id);

    }
}
