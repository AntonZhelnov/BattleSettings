using System.Collections.Generic;
using Common.Loading;
using Common.Loading.Stages;

namespace Gameplay
{
    public class GameplayLoadingStage : LevelLoadingStage
    {
        public GameplayLoadingStage(AddressablesProvider addressablesProvider)
            : base(addressablesProvider)
        {
            SpriteAssetIds = new List<string>
            {
                Constants.Sprites.Apple,
                Constants.Sprites.Armor,
                Constants.Sprites.Axe,
                Constants.Sprites.Book,
                Constants.Sprites.HealthPotion,
                Constants.Sprites.Meat,
                Constants.Sprites.Shield
            };

            GameObjectAssetIds = new List<string>
            {
                Constants.GameObjects.StatPanel,
                Constants.GameObjects.BuffPanel,
                Constants.GameObjects.HealthBar,
                Constants.GameObjects.HealthNumber
            };

            DataAssetId = Constants.Configs.Settings;
            SceneAssetId = Constants.Scenes.Gameplay;
            Description = "Gameplay loading...";
        }
    }
}