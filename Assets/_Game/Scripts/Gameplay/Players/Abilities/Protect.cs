using Gameplay.Data;

namespace Gameplay.Players.Abilities
{
    public class Protect : IAbility
    {
        public void Use(Character character)
        {
            character.ReceivingDamage -=
                character.ReceivingDamage
                * character.Stats[StatsId.Armor].value / 100;
        }

        public AbilityType Type => AbilityType.PreDamage;
    }
}