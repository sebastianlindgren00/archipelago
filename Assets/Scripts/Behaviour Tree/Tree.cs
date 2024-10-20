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

    public Node GetRunningNode(Node node)
    {
      foreach (var child in node.GetChildren())
      {
        // Debug.Log("Checking: " + child.Name + " " + child.nodeState);
        if (child.nodeState == NodeStatus.SUCCESS)
        {
          if (child.GetChildren().Length > 0)
            return GetRunningNode(child);
        }
        else if (child.nodeState == NodeStatus.RUNNING)
        {
          return GetRunningNode(child);
        }
      }

      if (node.GetChildren().Length > 0)
      {
        Node lastChild = node.GetChildren()[node.GetChildren().Length - 1];
        if (lastChild.nodeState == NodeStatus.RUNNING)
          return GetRunningNode(node.GetChildren()[node.GetChildren().Length - 1]);
      }
      return node;
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