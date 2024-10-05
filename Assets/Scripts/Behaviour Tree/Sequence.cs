using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority) { }

        // If any child node returns a failure, the entire node fails.
        // Whence all nodes return a success, the node reports a success. 
        public override NodeStatus Evaluate()
        {
            Debug.Log("Sequence: " + Name);
            if (m_currentChild == m_children.Count)
            {
                m_nodeStatus = NodeStatus.SUCCESS;
                return m_nodeStatus;
            }

            Debug.Log("Evaluating: " + m_children[m_currentChild].Name);
            switch (m_children[m_currentChild].Evaluate())
            {
                case NodeStatus.FAILURE:
                    m_nodeStatus = NodeStatus.FAILURE;
                    return m_nodeStatus;
                case NodeStatus.RUNNING:
                    m_nodeStatus = NodeStatus.RUNNING;
                    return m_nodeStatus;
                default:
                    m_currentChild++;
                    return m_currentChild == m_children.Count ? NodeStatus.SUCCESS : NodeStatus.RUNNING;
            }
        }
    }
}