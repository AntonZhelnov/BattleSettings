using System.Collections.Generic;
using Common.Extensions;
using Gameplay.Data;
using Gameplay.Players.Abilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class Character : IInitializable
    {
        private readonly List<IAbility> _abilities;
        private readonly Settings _settings;


        public Character(
            Settings settings,
            List<IAbility> abilities)
        {
            _settings = settings;
            _abilities = abilities;
        }

        public void Initialize()
        {
            ReactiveProperties = new IReactiveProperty<float>[_settings.stats.Length];
            for (var i = 0; i < ReactiveProperties.Length; i++)
                ReactiveProperties[i] = new ReactiveProperty<float>();

            Buffs = new List<Buff>();

            IsDead = ReactiveProperties[StatsId.Life]
                .Select(value => value <= 0)
                .ToReactiveProperty();
        }

        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }
        public float ReceivingDamage { get; set; }
        public List<Buff> Buffs { get; private set; }
        public Stat[] Stats { get; private set; }
        public Character Target { get; private set; }
        public IReactiveProperty<float>[] ReactiveProperties { get; private set; }


        public void ApplyBuff(Buff buff)
        {
            Buffs.Add(buff);
            var buffStats = buff.stats;

            foreach (var buffStat in buffStats)
            foreach (var stat in Stats)
                if (buffStat.statId == stat.id)
                {
                    ModifyValue(
                        stat.id,
                        buffStat.value);
                }
        }

        public void Attack(Character target)
        {
            Target = target;
            target.ReceiveDamage(Stats[StatsId.Damage].value);
            UseAbilities(AbilityType.PostAttack);
        }

        public void ModifyValue(
            int id,
            float amount)
        {
            Stats[id].value = Mathf.Max(0f, Stats[id].value + amount);
            ReactiveProperties[id].Value = Stats[id].value;
        }

        public void Reset()
        {
            Stats = (Stat[])_settings.stats.Copy();

            foreach (var stat in Stats)
                ReactiveProperties[stat.id].Value = stat.value;

            if (Buffs.Count > 0)
                Buffs.Clear();
        }

        private void ReceiveDamage(float damage)
        {
            ReceivingDamage = damage;
            UseAbilities(AbilityType.PreDamage);
            ModifyValue(
                StatsId.Life,
                -ReceivingDamage);
        }

        private void UseAbilities(AbilityType abilityType)
        {
            foreach (var ability in _abilities)
                if (ability.Type == abilityType)
                    ability.Use(this);
        }

        public class Factory : PlaceholderFactory<Character>
        {
            public override Character Create()
            {
                var character = base.Create();
                character.Initialize();
                return character;
            }
        }
    }
}