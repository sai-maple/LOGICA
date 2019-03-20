using System;
using LOGICA.Stage;
using UniRx;

namespace LOGICA.Model
{
    public class GameStateModel
    {
        private int _stageNum = 2;
        private readonly ReactiveProperty<int> _selectStage = new ReactiveProperty<int>(0);
        public IObservable<int> OnSelectStageChanged => _selectStage;

        private readonly ReactiveProperty<GameState> _gameState  = new ReactiveProperty<GameState>();
        public IObservable<GameState> OnGameStateChanged => _gameState;

        public bool IsAnimated { get; private set; } = false;

        public int ClickCount { get; private set; } = 0;
        public readonly int[] BestClickCount =
        {
            1,1,1,1,1,1
        };

        public void SetStageNum(int num)
        {
            _stageNum = num;
        }

        public void SetStage(int index)
        {
            index = index >= _stageNum ? _stageNum - 1 : index;
            _selectStage.Value = index < 0 ? 0 : index;
        }
        
        public int GetStage()
        {
            return _selectStage.Value;
        }
        
        public void SetGameState(GameState gameState)
        {
            _gameState.Value = gameState;
        }

        public GameState GetGameState()
        {
            return _gameState.Value;
        }
        
        public void OnClick()
        {
            ClickCount++;
        }

        public void ResetClickCount()
        {
            ClickCount = 0;
        }

        public void SetAnimated(bool isAnimated)
        {
            IsAnimated = isAnimated;
        }
    }
    
    public enum GameState
    {
        Non,
        OnPlay,
        OnClear,
    }
}
