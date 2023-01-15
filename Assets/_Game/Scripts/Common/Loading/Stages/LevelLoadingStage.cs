using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Loading.Stages
{
    public abstract class LevelLoadingStage : ILoadingStage
    {
        public event Action<float> Loaded;
        private readonly AddressablesProvider _addressablesProvider;
        private int _loadedElementsCount;
        private int _loadingElementsCount;
        private float _progress;
        protected string DataAssetId;
        protected List<string> GameObjectAssetIds;
        protected string SceneAssetId;
        protected List<string> SpriteAssetIds;


        protected LevelLoadingStage(AddressablesProvider addressablesProvider)
        {
            _addressablesProvider = addressablesProvider;
        }

        public string Description { get; protected set; }

        public async UniTask Load()
        {
            Loaded?.Invoke(.1f);

            _loadingElementsCount = SpriteAssetIds.Count + GameObjectAssetIds.Count + 2;

            foreach (var assetId in SpriteAssetIds)
            {
                await _addressablesProvider.Load<Sprite>(assetId);
                UpdateProgress();
            }

            foreach (var assetId in GameObjectAssetIds)
            {
                await _addressablesProvider.Load<GameObject>(assetId);
                UpdateProgress();
            }

            await _addressablesProvider.Load<TextAsset>(DataAssetId);
            UpdateProgress();

            var operation = SceneManager.LoadSceneAsync(
                SceneAssetId,
                LoadSceneMode.Single);

            await operation.ToUniTask();
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            _loadedElementsCount++;
            _progress = (float)_loadedElementsCount / _loadingElementsCount;
            Loaded?.Invoke(_progress);
        }
    }
}