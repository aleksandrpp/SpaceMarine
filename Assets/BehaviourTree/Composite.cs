namespace AK.BehaviourTree
{
    public abstract class Composite : INode
    {
        protected INode[] Nodes;

        protected Composite(INode[] nodes)
        {
            Nodes = nodes;
        }
        
        public abstract Status Execute();
    }
}