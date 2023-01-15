using System;
using Common.Animating;
using Gameplay.Data;
using Gameplay.UI;
using Gameplay.UI.Health;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class Player : IInitializable, IDisposable
    {
        public event Action Defeated;

        private readonly AnimationPlayer _animationPlayer;
        private readonly Transform _characterAnimatorTransform;
        private readonly CharacterBuffer _characterBuffer;
        private readonly Character.Factory _characterFactory;
        private readonly HealthBarsPanel _healthBarsPanel;
        private readonly PlayerPanel _playerPanel;
        private readonly Settings _settings;
        private HealthBar _healthBar;


        public Player(
            Settings settings,
            PlayerPanel playerPlayerPanel,
            AnimationPlayer animationPlayer,
            Character.Factory characterFactory,
            CharacterBuffer characterBuffer,
            HealthBarsPanel healthBarsPanel,
            Animator animator)
        {
            _settings = settings;
            _playerPanel = playerPlayerPanel;
            _animationPlayer = animationPlayer;
            _characterFactory = characterFactory;
            _characterBuffer = characterBuffer;
            _healthBarsPanel = healthBarsPanel;

            _characterAnimatorTransform = animator.transform;
        }

        public void Initialize()
        {
            _playerPanel.AttackRequested += Attack;

            Character = _characterFactory.Create();
            Character.Reset();

            _healthBar = _healthBarsPanel.AddHealthBar(
                Character.ReactiveProperties[StatsId.Life],
                _characterAnimatorTransform);

            Character.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ => OnCharacterDied());
        }

        public void Dispose()
        {
            _playerPanel.AttackRequested -= Attack;
        }

        public Player Target { get; set; }
        public Character Character { get; private set; }


        public void AllowAttack(bool value)
        {
            _playerPanel.ActivateAttackButton(value);
        }

        public void Play(bool withBuffs)
        {
            if (Character.IsDead.Value)
                _animationPlayer.Play(_settings.ReviveAnimationTriggerName);

            _healthBar.IsActive = false;
            Character.Reset();
            _playerPanel.ClearPanels();

            foreach (var stat in Character.Stats)
                _playerPanel.AddStatPanel(
                    Character.ReactiveProperties[stat.id],
                    stat.icon);

            if (withBuffs)
            {
                _characterBuffer.ApplyBuffsToCharacter(Character);

                foreach (var buff in Character.Buffs)
                    _playerPanel.AddBuffPanel(buff);
            }

            _healthBar.IsActive = true;
            _healthBar.Reset();

            AllowAttack(true);
        }

        private void Attack()
        {
            Character.Attack(Target.Character);
            _animationPlayer.Play(_settings.AttackAnimationTriggerName);
        }

        private void OnCharacterDied()
        {
            _animationPlayer.Play(_settings.DeadAnimationTriggerName);
            Defeated?.Invoke();
        }

        public class Factory : PlaceholderFactory<string, Player>
        {
        }

        [Serializable]
        public class Settings
        {
            public string AttackAnimationTriggerName;
            public string DeadAnimationTriggerName;
            public string ReviveAnimationTriggerName;
        }
    }
}