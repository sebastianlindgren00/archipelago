using System.Collections.Generic;
using System.Threading.Tasks;

namespace BehaviourTree
{
    [System.Serializable]
    public abstract class Node
    {
        public readonly string Name;
        public readonly int Priority;
        protected List<Node> m_children = new List<Node>();
        protected int m_currentChild = 0;

        // The current state of the node
        protected NodeStatus m_nodeStatus;

        public NodeStatus nodeState
        {
            get { return m_nodeStatus; }
        }

        // The constructor for the node
        public Node(string name = "Node", int priority = 0)
        {
            this.Name = name;
            this.Priority = priority;
        }

        public void AddChild(Node child) => m_children.Add(child);
        public Node[] GetChildren() => m_children.ToArray();

        // Implementing classes use this method to evaluate the desired set of conditions
        public async virtual Task<NodeStatus> Evaluate() => await m_children[m_currentChild].Evaluate();

        public virtual void Reset()
        {
            m_currentChild = 0;
            m_nodeStatus = NodeStatus.RUNNING;
        }

    }

    // Enumeration for the possible node states
    public enum NodeStatus
    {
        SUCCESS, FAILURE, RUNNING, ERROR
    }
}
