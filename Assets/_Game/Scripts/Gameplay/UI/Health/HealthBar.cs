using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _barImage;

        private Camera _camera;
        private IReactiveProperty<float> _health;
        private HealthNumber.Factory _healthNumberFactory;
        private float _initialHealth;
        private float _previousHealth;
        private Settings _settings;
        private Transform _trackingTransform;


        [Inject]
        public void Construct(
            Settings settings,
            IReactiveProperty<float> reactiveProperty,
            Transform trackingTransform,
            HealthNumber.Factory healthNumberFactory)
        {
            _settings = settings;
            _trackingTransform = trackingTransform;
            _health = reactiveProperty;
            _healthNumberFactory = healthNumberFactory;

            _camera = Camera.main;
        }

        public bool IsActive { get; set; }


        public void Reset()
        {
            _initialHealth = _health.Value;
            _previousHealth = _initialHealth;
            OnHealthChanged(_health.Value);
        }

        public void Start()
        {
            _health.Subscribe(OnHealthChanged).AddTo(this);
        }

        private void Update()
        {
            var position = _trackingTransform.position;

            transform.localPosition = _camera.WorldToScreenPoint(
                new Vector3(
                    position.x,
                    position.y + _settings.TrackingHeight,
                    position.z));
        }

        private void OnHealthChanged(float currentHealth)
        {
            if (!IsActive)
                return;

            var modifier = currentHealth - _previousHealth;
            _previousHealth = currentHealth;

            if (modifier != 0)
            {
                _healthNumberFactory.Create(
                    transform.position,
                    modifier);
            }

            _barImage.fillAmount = currentHealth / _initialHealth;
        }

        public class Factory : PlaceholderFactory<IReactiveProperty<float>, Transform, HealthBar>
        {
        }

        [Serializable]
        public class Settings
        {
            public float TrackingHeight;
        }
    }
}