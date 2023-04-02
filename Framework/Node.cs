using StereoKit;
using StereoKit.Framework;

namespace Framework
{
    // The abstract Node class serves as the base for other classes in the application.
    public abstract class Node : INodeChildManager<Node>, IStepper
    {
        // Enabled property determines if the node is active or not.
        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        // Unique identifier for the node.
        public Guid Id { get; } = Guid.NewGuid();

        // List of child nodes.
        private readonly List<Node> children = new();
        public List<Node> Children => children;

        #region IStepper

        // Initializes the node and sets it as enabled.
        public virtual bool Initialize()
        {
            if (!enabled)
            {
                enabled = true;
            }
            return true;
        }

        /// <summary>
        /// Updates this node and all of its child nodes. This method is called once per frame.
        /// </summary>
        public virtual void Step()
        {
            if (!Enabled)
                return;

            foreach (var child in Children)
            {
                child.Step();
            }
        }

        // Performs any necessary cleanup when shutting down the node.
        public virtual void Shutdown() { }

        #endregion

        #region INodeChildManager

        // Adds a child node to the list.
        public void AddChild(Node node) => children.Add(node);

        // Retrieves a child node at a specific index.
        public Node GetChild(int index) => children[index];

        // Returns the number of child nodes.
        public int GetChildCount() => children.Count;

        // Removes a child node at a specific index.
        public void RemoveChildAt(uint index)
        {
            if (index < Children.Count)
            {
                Children.RemoveAt((int)index);
            }
        }

        // Removes a child node with a specific ID.
        public void RemoveChildById(Guid id)
        {
            var childToRemove = children.FirstOrDefault(child => child.Id == id);
            if (childToRemove != null)
            {
                children.Remove(childToRemove);
            }
        }

        // Removes all child nodes of a specific type.
        public void RemoveChildren<TChild>() where TChild : Node
        {
            Children.RemoveAll(child => child is TChild);
        }

        #endregion
    }
}