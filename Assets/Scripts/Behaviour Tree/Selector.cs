using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : Node
{
  // The child nodes for this selector
  protected List<Node> m_nodes = new List<Node>();

  // The constructor requires a list of child nodes to be passed in
  public Selector(List<Node> nodes)
  {
    m_nodes = nodes;
  }

  // If any of the children reports a success, the selector will immediately report a success upwards.
  // If all children fail, it will report a failure instead.
  public override NodeStatus Evaluate()
  {
    foreach (Node node in m_nodes)
    {
      switch (node.Evaluate())
      {
        case NodeStatus.FAILURE:
          continue;
        case NodeStatus.SUCCESS:
          m_nodeStatus = NodeStatus.SUCCESS;
          return m_nodeStatus;
        case NodeStatus.RUNNING:
          m_nodeStatus = NodeStatus.RUNNING;
          return m_nodeStatus;
        default:
          continue;
      }
    }
    m_nodeStatus = NodeStatus.FAILURE;
    return m_nodeStatus;
  }
}
