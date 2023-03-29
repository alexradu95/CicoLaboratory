using StereoKit;
using StereoKit.Framework;
using System.Text;

namespace Nazar.Framework
{
    /// <summary>
    /// This Node class defines a basic interface for a node in your scene, 
    /// and provides basic functionality for adding and removing child nodes, 
    /// as well as initializing, stepping, and shutting down the node and its children. 
    /// To use this class, you can simply create new nodes that inherit from Node and override the necessary methods:
    /// </summary>
    public class Node : IStepper
    {
        private readonly List<Node> _children = new List<Node>();
        private bool _isInitialized = false;

        #region Properties

        /// <summary>
        /// Returns true if this node is enabled, false otherwise.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the pose (position and orientation) of this node.
        /// </summary>
        public Pose Pose { get; set; } = Pose.Identity;

        #endregion

        #region Interface methods

        /// <summary>
        /// Initializes this node and all of its child nodes. This method is called once before the first call to Step().
        /// </summary>
        public virtual bool Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                // Trigger _enter_tree callback
                OnEnterTree();

                // Initialize child nodes
                foreach (var child in _children)
                {
                    child.Initialize();
                }

                // Trigger _ready callback
                OnReady();
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


            foreach (var child in _children)
            {
                child.Step();
            }
        }

        /// <summary>
        /// Shuts down this node and all of its child nodes. This method is called once after the last call to Step().
        /// </summary>
        public virtual void Shutdown()
        {
            foreach (var child in _children)
            {
                child.Shutdown();
            }
        }

        #endregion

        #region Child management methods

        /// <summary>
        /// Adds a child node to this node. The child will be initialized, stepped, and shut down along with this node.
        /// </summary>
        public void AddChild(Node node)
        {
            _children.Add(node);
            node.OnAddedToParent(this);
        }

        /// <summary>
        /// Removes a child node from this node.
        /// </summary>
        public void RemoveChild(Node node)
        {
            _children.Remove(node);
            node.OnRemovedFromParent(this);
        }

        /// <summary>
        /// Gets the child node at the specified index.
        /// </summary>
        public Node GetChild(int index)
        {
            return _children[index];
        }

        /// <summary>
        /// Gets the number of child nodes of this node.
        /// </summary>
        public int GetChildCount()
        {
            return _children.Count;
        }

        #endregion

        #region Callback methods

        /// <summary>
        /// Called when this node is added to the scene tree.
        /// </summary>
        protected virtual void OnEnterTree()
        {
        }

        /// <summary>
        /// Called after this node and all of its child nodes are initialized.
        /// </summary>
        protected virtual void OnReady()
        {
        }

        /// <summary>
        /// Called when this node is added as a child to another node.
        /// </summary>
        protected virtual void OnAddedToParent(Node parent)
        {
        }

        /// <summary>
        /// Called when this node is added as a child to another node.
        /// </summary>
        protected virtual void OnRemovedFromParent(Node parent)
        {
        }

        #endregion


        #region Utils

        public string GetHierarchy(int level = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(' ', level * 2));
            sb.AppendLine($"{GetType().Name} ({GetHashCode()})");

            foreach (var child in _children)
            {
                sb.Append(child.GetHierarchy(level + 1));
            }

            return sb.ToString();
        }

        #endregion
    }
}
