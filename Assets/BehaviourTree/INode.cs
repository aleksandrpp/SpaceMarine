namespace AK.BehaviourTree
{
    public interface INode
    {
        Status Status { get; }

        Status Traverse();
    }
}