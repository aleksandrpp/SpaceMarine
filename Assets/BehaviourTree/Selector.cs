namespace AK.BehaviourTree
{
    public class Selector : Composite
    {
        public Selector(INode[] nodes) : base(nodes)
        {
        }
        
        public override Status Execute()
        {
            foreach (INode node in Nodes)
            {
                switch (node.Execute())
                {
                    case Status.Failure:
                        continue;
                    case Status.Success:
                        return Status.Success;
                }
            }
            
            return Status.Failure;
        }
    }
}