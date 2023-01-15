using System;
using Common.Loading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class StatPanel : MonoBehaviour, IPoolable<IReactiveProperty<float>, string, IMemoryPool>
    {
        [SerializeField] private Text _label;
        [SerializeField] private Image _icon;

        private AddressablesProvider _addressablesProvider;
        private IMemoryPool _pool;
        private IDisposable _subscription;


        [Inject]
        public void Construct(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
        }

        public void OnDespawned()
        {
            _subscription.Dispose();
            _pool = null;
        }

        public void OnSpawned(
            IReactiveProperty<float> reactiveProperty,
            string iconAssetId,
            IMemoryPool pool)
        {
            _subscription = reactiveProperty.Subscribe(UpdateLabel);
            _icon.sprite = _addressablesProvider.Get<Sprite>(iconAssetId);
            _pool = pool;
        }

        public void Expire()
        {
            _pool.Despawn(this);
        }

        private void UpdateLabel(float value)
        {
            _label.text = value.ToString("0.##");
        }

        public class Factory : PlaceholderFactory<IReactiveProperty<float>, string, StatPanel>
        {
        }
    }
}