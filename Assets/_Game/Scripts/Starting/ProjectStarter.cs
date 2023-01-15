using System.Collections.Generic;
using Common.Loading;
using Common.Loading.Stages;
using Gameplay;
using Zenject;

namespace Starting
{
    public class ProjectStarter : IInitializable
    {
        private readonly AddressablesLoadingStage _addressablesLoadingStage;
        private readonly GameplayLoadingStage _gameplayLoadingStage;
        private readonly LoadingStagesLoader _loadingStagesLoader;


        public ProjectStarter(
            AddressablesLoadingStage addressablesLoadingStage,
            LoadingStagesLoader loadingStagesLoader,
            GameplayLoadingStage gameplayLoadingStage)
        {
            _addressablesLoadingStage = addressablesLoadingStage;
            _loadingStagesLoader = loadingStagesLoader;
            _gameplayLoadingStage = gameplayLoadingStage;
        }

        public void Initialize()
        {
            _loadingStagesLoader.Load(new List<ILoadingStage>
            {
                _addressablesLoadingStage,
                _gameplayLoadingStage
            });
        }
    }
}