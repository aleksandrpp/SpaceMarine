using System.Collections.Generic;

namespace AK.BehaviourTree
{
    public abstract class Composite : Node
    {
        protected ICollection<Node> Nodes;
        
        protected Composite(ICollection<Node> nodes)
        {
            Nodes = nodes;
        }
    }
}