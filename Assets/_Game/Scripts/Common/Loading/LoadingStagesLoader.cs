using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Common.Loading
{
    public class LoadingStagesLoader
    {
        private readonly LocalPrefabSpawner _localPrefabSpawner;


        public LoadingStagesLoader(
            LocalPrefabSpawner localPrefabSpawner)
        {
            _localPrefabSpawner = localPrefabSpawner;
        }

        public async UniTask Load(List<ILoadingStage> loadingStages)
        {
            var loadingScreen = await _localPrefabSpawner.Spawn<LoadingScreen>();

            foreach (var loadingStage in loadingStages)
            {
                loadingScreen.TrackLoadingStage(loadingStage);
                await loadingStage.Load();
            }

            await loadingScreen.WaitForProgressBarFill();
            _localPrefabSpawner.Release();
        }
    }
}