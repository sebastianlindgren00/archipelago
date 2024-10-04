using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        // Children nodes that belong to this sequence
        private List<Node> m_nodes = new List<Node>();

        // Must provide an initial set of children nodes to work
        public Sequence(List<Node> nodes)
        {
            m_nodes = nodes;
        }

        // If any child node returns a failure, the entire node fails.
        // Whence all nodes return a success, the node reports a success. 
        public override NodeStatus Evaluate()
        {
            bool anyChildRunning = false;

            foreach (Node node in m_nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeStatus.FAILURE:
                        m_nodeStatus = NodeStatus.FAILURE;
                        return m_nodeStatus;
                    case NodeStatus.SUCCESS:
                        continue;
                    case NodeStatus.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        m_nodeStatus = NodeStatus.SUCCESS;
                        return m_nodeStatus;
                }
            }
            m_nodeStatus = anyChildRunning ? NodeStatus.RUNNING : NodeStatus.SUCCESS;
            return m_nodeStatus;
        }
    }
}