using System;

namespace AK.BehaviourTree
{
    public class Node : INode
    {
        private event Func<Status> Action;
        
        public Status Status { get; protected set; }

        protected Node()
        {
            Status = Status.None;
        }

        public Node(Func<Status> action)
        {
            Status = Status.None;
            
            Action = action;
        }
        
        protected virtual Status Execute()
        {
            return Action!();
        }

        public Status Traverse()
        {
            Status = Execute();

            return Status;
        }
    }
}