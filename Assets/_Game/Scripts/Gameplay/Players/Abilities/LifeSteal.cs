using Gameplay.Data;

namespace Gameplay.Players.Abilities
{
    public class LifeSteal : IAbility
    {
        public void Use(Character character)
        {
            var healingAmount =
                character.Target.ReceivingDamage
                * character.Stats[StatsId.LifeSteal].value / 100;

            character.ModifyValue(
                StatsId.Life,
                healingAmount);
        }

        public AbilityType Type => AbilityType.PostAttack;
    }
}