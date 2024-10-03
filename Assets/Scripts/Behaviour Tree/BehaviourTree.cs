using System.Collections.Generic;

namespace BehaviourTree
{
  public class BehaviourTree : Node
  {
    public BehaviourTree(string name) : base(name) { }

    public override NodeStatus Evaluate()
    {
      while (m_currentChild < m_children.Count)
      {
        m_nodeStatus = m_children[m_currentChild].Evaluate();

        if (m_nodeStatus != NodeStatus.SUCCESS)
        {
          return m_nodeStatus;
        }

        m_currentChild++;
      }
      return NodeStatus.SUCCESS;
    }
  }
}