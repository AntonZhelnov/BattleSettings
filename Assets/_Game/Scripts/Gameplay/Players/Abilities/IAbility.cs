namespace Gameplay.Players.Abilities
{
    public enum AbilityType
    {
        PostAttack,
        PreDamage
    }

    public interface IAbility
    {
        public AbilityType Type { get; }

        public void Use(Character character);
    }
}