namespace BehaviourTree
{
  public class Tree : Node
  {
    public Tree(string name) : base(name) { }

    public override NodeStatus Evaluate()
    {
      while (m_currentChild < m_children.Count)
      {
        m_nodeStatus = m_children[m_currentChild].Evaluate();
        // If the child fails or is running, then we stop the loop
        if (m_nodeStatus != NodeStatus.SUCCESS)
        {
          return m_nodeStatus;
        }
        // If the child succeeds, then we continue to the next one
        m_currentChild++;
      }
      // If we get to the end of the list, then the behaviour tree has finished running
      return NodeStatus.SUCCESS;
    }
  }
}