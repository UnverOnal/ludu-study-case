using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GamePlay;
using GamePlay.CellManagement;
using GamePlay.CellManagement.Creators;
using Level.LevelCounter;
using Level.LevelCreation;
using PowerUpManagement.PowerUpTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PowerUpManagement
{
    public class PowerUpView
    {
        public event Action<CellCreationData> OnPowerUpCreated;

        private readonly CellCreator _cellCreator;
        private readonly CellPrefabCreator _cellPrefabCreator;
        private readonly LevelPresenter _levelPresenter;

        private readonly GameObject _powerUpParent;
        private readonly IEnumerable<CellAssetData> _powerUpData;

        public PowerUpView(CellCreator cellCreator, CellPrefabCreator cellPrefabCreator, LevelPresenter levelPresenter,
            BoardCreationData boardCreationData)
        {
            _cellCreator = cellCreator;
            _cellPrefabCreator = cellPrefabCreator;
            _levelPresenter = levelPresenter;

            _powerUpParent = new GameObject("PowerUps");
            
            _powerUpData =
                boardCreationData.blockCreationData.Where(data => data.type is CellType.Bomb or CellType.Rocket);
        }

        public void CreatePowerUp(CellType cellType, BoardLocation location)
        {
            var prefab = _cellPrefabCreator.Get(cellType);
            var creationData = new CellCreationData(location, prefab, GetLevelPowerUpData(cellType));
            OnPowerUpCreated?.Invoke(creationData);

            SpawnPowerUp(prefab, new Vector3(location.x, location.y));
        }

        private LevelPowerUpData GetLevelPowerUpData(CellType type)
        {
            var levelPowerUpDatas = _levelPresenter.GetCurrentLevelData().powerUpData;
            for (var i = 0; i < levelPowerUpDatas.Length; i++)
            {
                var data = levelPowerUpDatas[i];
                if (data.cellType == type)
                    return data;
            }

            return null;
        }

        private void SpawnPowerUp(GameObject prefab, Vector3 position)
        {
            var transform = prefab.transform;
            transform.position = position;
            
            var originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack, 2f);
        }
    }
}