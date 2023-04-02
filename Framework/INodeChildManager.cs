using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// Defines the interface for managing child nodes of a hierarchical structure.
    /// </summary>
    /// <typeparam name="T">The type of child nodes managed by the implementing class.</typeparam>
    public interface INodeChildManager<T> where T : INodeChildManager<T>
    {
        /// <summary>
        /// Gets the list of child nodes.
        /// </summary>
        List<T> Children { get; }

        /// <summary>
        /// Adds a child node to the list.
        /// </summary>
        /// <param name="node">The child node to add.</param>
        void AddChild(T node);

        /// <summary>
        /// Removes a child node at the specified index.
        /// </summary>
        /// <param name="index">The index of the child node to remove.</param>
        void RemoveChildAt(uint index);

        /// <summary>
        /// Removes a child node with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the child node to remove.</param>
        void RemoveChildById(Guid id);

        /// <summary>
        /// Removes all occurrences of a specific child node from the list.
        /// </summary>
        /// <param name="node">The child node to remove.</param>
        void RemoveChildren<TChild>() where TChild : T;

        /// <summary>
        /// Gets the child node at the specified index.
        /// </summary>
        /// <param name="index">The index of the child node to get.</param>
        /// <returns>The child node at the specified index.</returns>
        T GetChild(int index);

        /// <summary>
        /// Gets the number of child nodes.
        /// </summary>
        /// <returns>The number of child nodes.</returns>
        int GetChildCount();

        /// <summary>
        /// Generates a string representation of the node hierarchy.
        /// </summary>
        /// <param name="level">The level of indentation for the current node (default: 0).</param>
        /// <returns>A string representation of the node hierarchy.</returns>
        public string GetHierarchy(int level = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(new string(' ', level * 2));
            sb.AppendLine($"{GetType().Name} ({GetHashCode()})");

            foreach (var child in Children)
            {
                sb.Append(child.GetHierarchy(level + 1));
            }

            return sb.ToString();
        }
    }
}
