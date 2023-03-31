using Nazar.Framework;

namespace Framework
{
    public interface INodeParentListener
    {
        void OnAddedToParent(Node parent);
        void OnRemovedFromParent(Node parent);
    }
}
