using Common.Animating;
using Common.Loading;
using Gameplay.Data;
using Gameplay.Players;
using Gameplay.Players.Abilities;
using Gameplay.UI;
using Gameplay.UI.Health;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [CreateAssetMenu(
        fileName = "New Gameplay",
        menuName = "Installers/Levels/Gameplay")]
    public class GameplayInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Player.Settings _playerSettings;
        [SerializeField] private HealthBar.Settings _healthBarSettings;
        [SerializeField] private HealthNumber.Settings _healthNumberSettings;

        [Inject] private readonly AddressablesProvider _addressablesProvider;


        public override void InstallBindings()
        {
            InstallData();
            InstallCharacters();
            InstallPlayers();
            InstallUI();

            Container.BindInterfacesTo<GameplayStarter>().AsSingle();
        }

        private void InstallCharacters()
        {
            Container.BindFactory<Character, Character.Factory>();
            Container.BindInterfacesAndSelfTo<CharacterBuffer>().AsSingle();
            Container.Bind<IAbility>().To<LifeSteal>().AsTransient();
            Container.Bind<IAbility>().To<Protect>().AsTransient();
        }

        private void InstallData()
        {
            var dataJson = _addressablesProvider.Get<TextAsset>(Constants.Configs.Settings);
            var data = JsonUtility.FromJson<Settings>(dataJson.ToString());
            Container.BindInstance(data).AsSingle();
        }

        private void InstallPlayer(
            DiContainer subContainer,
            string id)
        {
            subContainer.Bind<Player>().AsSingle()
                .WithArguments(_playerSettings);

            subContainer.BindInstance(Container.ResolveId<Animator>(id)).AsSingle();
            subContainer.BindInstance(Container.ResolveId<PlayerPanel>(id)).AsSingle();

            subContainer.Bind<AnimationPlayer>().AsSingle();
        }

        private void InstallPlayers()
        {
            Container.BindFactory<string, Player, Player.Factory>()
                .FromSubContainerResolve()
                .ByMethod(InstallPlayer);
        }

        private void InstallUI()
        {
            Container.BindFactory<IReactiveProperty<float>, string, StatPanel, StatPanel.Factory>()
                .FromPoolableMemoryPool<IReactiveProperty<float>, string, StatPanel, StatPanelPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(
                            _addressablesProvider.Get<GameObject>(Constants.GameObjects.StatPanel))
                        .UnderTransformGroup("StatPanels"));

            Container.BindFactory<Buff, BuffPanel, BuffPanel.Factory>()
                .FromPoolableMemoryPool<Buff, BuffPanel, BuffPanelPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(
                            _addressablesProvider.Get<GameObject>(Constants.GameObjects.BuffPanel))
                        .UnderTransformGroup("BuffPanels"));

            Container.BindFactory<IReactiveProperty<float>, Transform, HealthBar, HealthBar.Factory>()
                .FromComponentInNewPrefab(_addressablesProvider.Get<GameObject>(Constants.GameObjects.HealthBar));
            Container.BindInstance(_healthBarSettings).WhenInjectedInto<HealthBar>();

            Container.BindFactory<Vector3, float, HealthNumber, HealthNumber.Factory>()
                .FromPoolableMemoryPool<Vector3, float, HealthNumber, HealthNumberPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(
                            _addressablesProvider.Get<GameObject>(Constants.GameObjects.HealthNumber))
                        .UnderTransformGroup("HealthNumbers"));
            Container.BindInstance(_healthNumberSettings).WhenInjectedInto<HealthNumber>();
        }

        private class StatPanelPool : MonoPoolableMemoryPool<IReactiveProperty<float>, string, IMemoryPool, StatPanel>
        {
        }

        private class BuffPanelPool : MonoPoolableMemoryPool<Buff, IMemoryPool, BuffPanel>
        {
        }

        private class HealthNumberPool : MonoPoolableMemoryPool<Vector3, float, IMemoryPool, HealthNumber>
        {
        }
    }
}