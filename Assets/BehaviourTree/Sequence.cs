using System.Collections.Generic;

namespace AK.BehaviourTree
{
    public class Sequence : Composite
    {
        public Sequence(ICollection<Node> nodes) : base(nodes)
        {
        }

        protected override Status Execute()
        {
            foreach (var node in Nodes)
            {
                switch (node.Traverse())
                {
                    case Status.Failure:
                        Status = Status.Failure;
                        return Status;
                    case Status.Success:
                        continue;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                }
            }

            return Status.Success;
        }
    }
}