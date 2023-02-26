using StereoKit.Framework;

namespace Nazar.Framework.Interface
{
    interface INode
    {

        Node GetChild(string id);

        Node AddChild(Type node, string id);

        /// <summary>
        /// Deletes the node with the corresponding ID
        /// </summary>
        /// <param name="id"></param>
        void DisableChild(string id);

        /// <summary>
        /// Deletes all nodes of the provided Type
        /// </summary>
        /// <param name="node"></param>
        void DisableChildren(Type node);

        void DrawNodeManager();
    }
}
