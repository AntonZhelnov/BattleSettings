using System;
using Gameplay.Players;
using Gameplay.UI;
using Zenject;

namespace Gameplay
{
    public class GameplayStarter : IInitializable, IDisposable
    {
        private readonly Player.Factory _playerFactory;
        private readonly PlayPanel _playPanel;
        private Player _player1;
        private Player _player2;


        public GameplayStarter(
            PlayPanel playPanel,
            Player.Factory playerFactory)
        {
            _playPanel = playPanel;
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            _playPanel.PlayRequested += Play;

            _player1 = _playerFactory.Create("1");
            _player2 = _playerFactory.Create("2");

            _player1.Target = _player2;
            _player2.Target = _player1;

            _player1.Initialize();
            _player2.Initialize();

            _player1.Defeated += OnPlayerDefeated;
            _player2.Defeated += OnPlayerDefeated;

            Play(true);
        }

        public void Dispose()
        {
            _playPanel.PlayRequested -= Play;
            _player1.Dispose();
            _player2.Dispose();
        }

        private void OnPlayerDefeated()
        {
            _player1.AllowAttack(false);
            _player2.AllowAttack(false);
        }

        private void Play(bool withBuffs)
        {
            _player1.Play(withBuffs);
            _player2.Play(withBuffs);
        }
    }
}