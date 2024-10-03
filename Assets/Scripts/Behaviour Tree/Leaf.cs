using UnityEngine;

namespace BehaviourTree
{
  public interface IAction
  {
    NodeStatus Execute();
    void Reset();
  }

  public class Leaf : Node
  {
    private IAction action;

    public Leaf(string name, IAction action) : base(name)
    {
      this.action = action;
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