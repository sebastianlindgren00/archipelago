using System.Threading.Tasks;

namespace BehaviourTree
{
  public class Inverter : Node
  {
    /* The constructor requires the child node that this inverter decorator 
     * wraps*/
    public Inverter(string name, int priority = 0) : base(name, priority) { }
    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public override async Task<NodeStatus> Evaluate()
    {
      switch (await m_children[0].Evaluate())
      {
        case NodeStatus.FAILURE:
          m_nodeStatus = NodeStatus.SUCCESS;
          return m_nodeStatus;
        case NodeStatus.RUNNING:
          m_nodeStatus = NodeStatus.RUNNING;
          return m_nodeStatus;
        default:
          m_nodeStatus = NodeStatus.FAILURE;
          return m_nodeStatus;
      }
    }
  }

  public class Repeat : Node
  {
    private int m_repeats;
    private int m_currentRepeats;

    public Repeat(string name, int repeats, int priority = 0) : base(name, priority)
    {
      m_repeats = repeats;
    }

    public override async Task<NodeStatus> Evaluate()
    {
      if (m_currentRepeats < m_repeats)
      {
        switch (await m_children[0].Evaluate())
        {
          case NodeStatus.FAILURE:
            m_nodeStatus = NodeStatus.FAILURE;
            return m_nodeStatus;
          case NodeStatus.RUNNING:
            m_nodeStatus = NodeStatus.RUNNING;
            return m_nodeStatus;
          default:
            m_currentRepeats++;
            return NodeStatus.RUNNING;
        }
      }
      m_nodeStatus = NodeStatus.SUCCESS;
      return m_nodeStatus;
    }

    public override void Reset()
    {
      m_currentRepeats = 0;
    }
  }
}