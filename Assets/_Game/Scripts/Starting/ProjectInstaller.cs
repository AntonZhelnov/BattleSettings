using Common.Loading;
using Common.Loading.Stages;
using Gameplay;
using Zenject;

namespace Starting
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressablesProvider>().AsSingle();

            Container.Bind<LocalPrefabSpawner>().AsSingle();
            Container.Bind<LoadingStagesLoader>().AsSingle();
            Container.Bind<AddressablesLoadingStage>().AsSingle();
            Container.Bind<GameplayLoadingStage>().AsSingle();

            Container.BindInterfacesTo<ProjectStarter>().AsSingle().NonLazy();
        }
    }
}