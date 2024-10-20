using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviourTree
{
  public class Selector : Node
  {
    // The constructor requires a list of child nodes to be passed in
    public Selector(string name, int priority = 0) : base(name, priority) { }

    // If any of the children reports a success, the selector will immediately report a success upwards.
    // If all children fail, it will report a failure instead.
    public async override Task<NodeStatus> Evaluate()
    {
      foreach (Node node in m_children)
      {
        switch (await node.Evaluate())
        {
          case NodeStatus.SUCCESS:
            return NodeStatus.SUCCESS;
          case NodeStatus.RUNNING:
            return NodeStatus.RUNNING;
          default:
            continue;
        }
      }
      return NodeStatus.FAILURE;
    }
  }

  public class PrioritySelector : Selector
  {
    List<Node> sortedChildren;
    List<Node> priorityChildren => sortedChildren ??= (sortedChildren = m_children.OrderByDescending(c => c.Priority).ToList());


    // The constructor requires a list of child nodes to be passed in
    public PrioritySelector(string name) : base(name) { }

    public override void Reset()
    {
      base.Reset();
      sortedChildren = null;

    }

    // If any of the children reports a success, the selector will immediately report a success upwards.
    // If all children fail, it will report a failure instead.
    public async override Task<NodeStatus> Evaluate()
    {
      foreach (Node node in priorityChildren)
      {
        switch (await node.Evaluate())
        {
          case NodeStatus.SUCCESS:
            return NodeStatus.SUCCESS;
          case NodeStatus.RUNNING:
            return NodeStatus.RUNNING;
          default:
            continue;
        }
      }
      return NodeStatus.FAILURE;
    }
  }

  public class RandomSelector : Selector
  {
    // The constructor requires a list of child nodes to be passed in
    public RandomSelector(string name, int priority = 0) : base(name, priority) { }

    // If any of the children reports a success, the selector will immediately report a success upwards.
    // If all children fail, it will report a failure instead.
    public async override Task<NodeStatus> Evaluate()
    {
      List<Node> shuffledChildren = m_children.OrderBy(x => Random.value).ToList();
      foreach (Node node in shuffledChildren)
      {
        switch (await node.Evaluate())
        {
          case NodeStatus.SUCCESS:
            return NodeStatus.SUCCESS;
          case NodeStatus.RUNNING:
            return NodeStatus.RUNNING;
          default:
            continue;
        }
      }
      return NodeStatus.FAILURE;
    }
  }
}
