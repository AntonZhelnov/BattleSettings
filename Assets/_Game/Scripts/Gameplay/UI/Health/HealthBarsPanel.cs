using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Health
{
    public class HealthBarsPanel : MonoBehaviour
    {
        private HealthBar.Factory _healthBarFactory;


        [Inject]
        public void Construct(HealthBar.Factory healthBarFactory)
        {
            _healthBarFactory = healthBarFactory;
        }

        public HealthBar AddHealthBar(
            IReactiveProperty<float> reactiveProperty,
            Transform trackingTransform)
        {
            var healthBar = _healthBarFactory.Create(
                reactiveProperty,
                trackingTransform);

            healthBar.transform.SetParent(transform);

            return healthBar;
        }
    }
}