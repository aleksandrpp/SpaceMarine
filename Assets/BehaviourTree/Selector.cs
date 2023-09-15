using System.Collections.Generic;

namespace AK.BehaviourTree
{
    public class Selector : Composite
    {
        public Selector(ICollection<Node> nodes) : base(nodes)
        {
        }

        protected override Status Execute()
        {
            foreach (var node in Nodes)
            {
                switch (node.Traverse())
                {
                    case Status.Failure:
                        continue;
                    case Status.Success:
                        Status = Status.Success;
                        return Status;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                }
            }
            
            Status = Status.Failure;
            return Status;
        }
    }
}