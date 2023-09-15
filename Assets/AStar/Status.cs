namespace AK.AStar
{
    public enum Status
    {
        PathNotFound = -5,
        PathLengthOversize = -4,
        StartIsEnd = -3,
        InvalidPosition = -1,
        Success = 0,
    }
}