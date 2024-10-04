namespace BehaviourTree
{
  public class Inverter : Node
  {
    /* Child node to evaluate */
    private Node m_node;

    public Node node
    {
      get { return m_node; }
    }

    /* The constructor requires the child node that this inverter decorator 
     * wraps*/
    public Inverter(Node node)
    {
      m_node = node;
    }

    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public override NodeStatus Evaluate()
    {
      switch (m_node.Evaluate())
      {
        case NodeStatus.FAILURE:
          m_nodeStatus = NodeStatus.SUCCESS;
          return m_nodeStatus;
        case NodeStatus.SUCCESS:
          m_nodeStatus = NodeStatus.FAILURE;
          return m_nodeStatus;
        case NodeStatus.RUNNING:
          m_nodeStatus = NodeStatus.RUNNING;
          return m_nodeStatus;
      }
      m_nodeStatus = NodeStatus.SUCCESS;
      return m_nodeStatus;
    }
  }
}