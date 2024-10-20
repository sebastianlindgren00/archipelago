using UnityEngine;

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

    public void PrintRunningNode()
    {
      foreach (var child in m_children)
      {
        PrintNode(child, 0, true);
      }

    }

    public void Print()
    {
      Debug.Log("Behaviour Tree: " + Name);
      foreach (var child in m_children)
      {
        PrintNode(child, 1);
      }
    }

    private void PrintNode(Node node, int depth, bool runningNode = false)
    {
      if (runningNode)
      {
        if (nodeState == NodeStatus.RUNNING)
        {
          foreach (var child in m_children)
          {
            PrintNode(child, depth + 1, true);
          }
        }
        else
        {
          Debug.Log(new string('-', depth) + " " + node.Name);
        }

      }

      Debug.Log(new string('-', depth) + " " + node.Name);
      foreach (var child in node.GetChildren())
      {
        PrintNode(child, depth + 1);
      }
    }
  }
}