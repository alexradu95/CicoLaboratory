using Nazar.Framework;

namespace Framework
{
    public interface INodeChildManager
    {
        void AddChild(Node node);
        void RemoveChild(Node node);
        Node GetChild(int index);
        int GetChildCount();
    }
}
