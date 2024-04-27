using Board;
using Board.BoardCreation;
using GameManagement;
using Services.InputService;
using UnityEngine;
using VContainer;

namespace GamePlay
{
    public class GamePlayPresenter : IInitializable
    {
        [Inject] private IInputService _inputService;
        [Inject] private BoardPresenter _boardPresenter;
        [Inject] private LevelCreationPresenter _levelCreationPresenter;
        
        public void Initialize()
        {
            _inputService.OnItemPicked += OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated += _boardPresenter.GroupCells;
        }

        private void OnBlockSelected(GameObject cellGameObject)
        {
            if(cellGameObject.layer != LayerMask.NameToLayer("Cell")) return;
            
            _boardPresenter.OnBlockSelected(cellGameObject);
        }

        private void OnLevelEnd()
        {
            _inputService.OnItemPicked -= OnBlockSelected;
            _levelCreationPresenter.OnLevelCreated -= _boardPresenter.GroupCells;
        }
    }
}
