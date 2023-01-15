using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Health
{
    public class HealthNumber : MonoBehaviour, IPoolable<Vector3, float, IMemoryPool>
    {
        [SerializeField] private TextMeshProUGUI _label;

        private Transform _parentTransform;
        private IMemoryPool _pool;
        private Settings _settings;


        [Inject]
        public void Construct(
            Settings settings,
            HealthBarsPanel healthBarsPanel)
        {
            _settings = settings;
            _parentTransform = healthBarsPanel.transform;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(
            Vector3 position,
            float value,
            IMemoryPool pool)
        {
            _label.SetText(value.ToString("0"));

            _label.color = value > 0
                ? _settings.PositiveColor
                : _settings.NegativeColor;

            transform.SetParent(_parentTransform);
            transform.localPosition = position;
            transform.localScale = Vector3.zero;

            var tween = DOTween.Sequence();
            tween
                .Append(transform.DOScale(Vector3.one, _settings.PunchDuration))
                .Join(transform.DOMoveY(transform.position.y + _settings.MoveDistance, _settings.MoveDuration))
                .Append(transform.DOScale(Vector3.zero, _settings.PunchDuration / 2f))
                .OnComplete(Expire);

            _pool = pool;
        }

        private void Expire()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Vector3, float, HealthNumber>
        {
        }

        [Serializable]
        public class Settings
        {
            public Color PositiveColor;
            public Color NegativeColor;
            public float MoveDistance;
            public float MoveDuration;
            public float PunchDuration;
        }
    }
}