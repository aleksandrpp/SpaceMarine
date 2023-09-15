namespace AK.SpaceMarine.Parts
{
    public interface IPickable : IRange
    {
        void Pickup(IPosition position);
    }
}