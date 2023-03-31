using Framework;

namespace Nazar.Framework
{
    public class Node : NodeLifecycle, INodeChildManager, INodeParentListener
    {
        private readonly List<Node> _children = new List<Node>();

        public override void Step()
        {
            if (!Enabled)
                return;

            foreach (var child in _children)
            {
                child.Step();
            }
        }

        public void AddChild(Node node)
        {
            _children.Add(node);
            if (node is INodeParentListener listener)
            {
                listener.OnAddedToParent(this);
            }
        }

        public void RemoveChild(Node node)
        {
            _children.Remove(node);
            if (node is INodeParentListener listener)
            {
                listener.OnRemovedFromParent(this);
            }
        }

        public Node GetChild(int index)
        {
            return _children[index];
        }

        public int GetChildCount()
        {
            return _children.Count;
        }

        public void OnAddedToParent(Node parent) { }

        public void OnRemovedFromParent(Node parent) { }

        // Utils and other methods can be added here
    }
}