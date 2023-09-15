namespace AK.SpaceMarine.Parts
{
    public interface IHero : ITransform, IPosition
    {
        bool Active { get; }
        
        string Loadout { get; }

        void Bind(IInput input);

        void Heal(float amount);
    }
}