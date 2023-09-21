namespace AK.BehaviourTree
{
    public class Leaf : INode
    {
        private Action _action;

        public Leaf(Action action)
        {
            _action = action;
        }
        
        public Status Execute()
        {
            switch (_action())
            {
                case Status.Success:
                    return Status.Success;
                case Status.Failure:
                default:
                    return Status.Failure;
            }
        }
    }
}