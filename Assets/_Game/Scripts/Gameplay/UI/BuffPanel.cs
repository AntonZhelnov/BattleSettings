using Common.Loading;
using Gameplay.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class BuffPanel : MonoBehaviour, IPoolable<Buff, IMemoryPool>
    {
        [SerializeField] private Text _label;
        [SerializeField] private Image _icon;

        private AddressablesProvider _addressablesProvider;
        private Buff _buff;
        private IMemoryPool _pool;


        [Inject]
        public void Construct(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(
            Buff buff,
            IMemoryPool pool)
        {
            _buff = buff;
            _label.text = _buff.title;
            _icon.sprite = _addressablesProvider.Get<Sprite>(_buff.icon);
            _pool = pool;
        }

        public void Expire()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Buff, BuffPanel>
        {
        }
    }
}