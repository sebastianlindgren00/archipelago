namespace BehaviourTree
{
  public class Leaf : Node
  {
    private IStrategy action;

    public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
    {
      this.action = strategy;

    }

    public override NodeStatus Evaluate()
    {
      switch (action.Execute())
      {
        case NodeStatus.SUCCESS:
          m_nodeStatus = NodeStatus.SUCCESS;
          return m_nodeStatus;
        case NodeStatus.FAILURE:
          m_nodeStatus = NodeStatus.FAILURE;
          return m_nodeStatus;
        case NodeStatus.RUNNING:
          m_nodeStatus = NodeStatus.RUNNING;
          return m_nodeStatus;
        default:
          m_nodeStatus = NodeStatus.FAILURE;
          return m_nodeStatus;
      }
    }

    public override void Reset()
    {
      action.Reset();
    }
  }
}