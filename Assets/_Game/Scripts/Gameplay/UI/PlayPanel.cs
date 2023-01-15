using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PlayPanel : MonoBehaviour
    {
        public event Action<bool> PlayRequested;

        [SerializeField] private Button _playWithBuffsButton;
        [SerializeField] private Button _playWithoutBuffsButton;


        private void Start()
        {
            _playWithBuffsButton.OnClickAsObservable()
                .Subscribe(_ => PlayRequested?.Invoke(true))
                .AddTo(this);

            _playWithoutBuffsButton.OnClickAsObservable()
                .Subscribe(_ => PlayRequested?.Invoke(false))
                .AddTo(this);
        }
    }
}