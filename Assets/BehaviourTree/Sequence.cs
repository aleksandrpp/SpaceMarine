namespace AK.BehaviourTree
{
    public class Sequence : Composite
    {
        public Sequence(INode[] nodes) : base(nodes)
        {
        }
        
        public override Status Execute()
        {
            foreach (INode node in Nodes)
            {
                switch (node.Execute())
                {
                    case Status.Failure:
                        return Status.Failure;
                    case Status.Success:
                        continue;
                }
            }
            
            return Status.Success;
        }
    }
}