using System;
using System.Collections.Generic;
using Gameplay.Data;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Players
{
    public class CharacterBuffer : IInitializable
    {
        private readonly Settings _settings;
        private int _buffCountMax;
        private int _buffCountMin;
        private Buff[] _buffs;
        private int _buffsCount;
        private int _randomBuffsCount;


        public CharacterBuffer(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            _buffs = _settings.buffs;
            _buffsCount = _buffs.Length;
            _buffCountMin = _settings.gameSettings.buffCountMin;
            _buffCountMax = _settings.gameSettings.buffCountMax;
        }

        public void ApplyBuffsToCharacter(Character character)
        {
            _randomBuffsCount = Random.Range(_buffCountMin, _buffCountMax);

            var selectedBuffs =
                _settings.gameSettings.allowDuplicateBuffs
                    ? GetBuffs(_randomBuffsCount)
                    : GetUniqueBuffs(_randomBuffsCount);

            foreach (var buff in selectedBuffs)
                character.ApplyBuff(buff);
        }

        private List<Buff> GetBuffs(int count)
        {
            var buffs = new List<Buff>();

            for (var i = 0; i < count; i++)
                buffs.Add(_buffs[Random.Range(0, _buffsCount)]);

            return buffs;
        }

        private List<Buff> GetUniqueBuffs(int count)
        {
            if (count > _buffsCount)
                throw new ArgumentOutOfRangeException("Available data buffs isn't enough!");

            var buffAvailableIndexes = new List<int>();

            for (var i = 0; i < _buffsCount; i++)
                buffAvailableIndexes.Add(i);

            var buffs = new List<Buff>();

            for (var i = 0; i < count; i++)
            {
                var randomBuffIndex =
                    buffAvailableIndexes[Random.Range(0, buffAvailableIndexes.Count)];
                buffs.Add(_buffs[randomBuffIndex]);
                buffAvailableIndexes.Remove(randomBuffIndex);
            }

            return buffs;
        }
    }
}