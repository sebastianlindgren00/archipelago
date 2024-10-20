using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
  public class Leaf : Node
  {
    private IStrategy action;

    public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
    {
      this.action = strategy;

    }

    public override Task<NodeStatus> Evaluate()
    {
      switch (action.Execute())
      {
        case NodeStatus.SUCCESS:
          Debug.Log("Leaf: " + Name + " " + " SUCCESS");
          m_nodeStatus = NodeStatus.SUCCESS;
          return Task.FromResult(m_nodeStatus);
        case NodeStatus.FAILURE:
          Debug.Log("Leaf: " + Name + " " + " FAILURE");
          m_nodeStatus = NodeStatus.FAILURE;
          return Task.FromResult(m_nodeStatus);
        default:
          m_nodeStatus = NodeStatus.RUNNING;
          return Task.FromResult(m_nodeStatus);
      }
    }

    public override void Reset()
    {
      action.Reset();
    }
  }
}