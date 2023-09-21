namespace AK.BehaviourTree
{
    public delegate Status Action();

    public enum Status
    {
        Failure,
        Success
    }
}