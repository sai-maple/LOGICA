using DG.Tweening;
using LOGICA.Common;
using LOGICA.Model;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace LOGICA.Stage
{
    public class StageController : MonoBehaviour
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly DiContainer _container = default;
        [Inject] private readonly GameStateModel _gameStateModel = default;
        [SerializeField] private StageObject[] _stageObjects = default;

        private StageObject _currentStage;

        private void Start()
        {
            _gameStateModel.SetStageNum(_stageObjects.Length);
            var stream = _gameStateModel.OnGameStateChanged
                .TakeUntilDestroy(this)
                .Publish()
                .RefCount();
            
            stream.Where(s => s == GameState.OnPlay)
                .Subscribe(_ => CreateStage(_gameStateModel.GetStage()));
            
            stream.Where(s => s == GameState.Non)
                .Subscribe(_ => ReturnTitle());
                
        }

        private async void CreateStage(int index)
        {
            if (_currentStage != null)
            {
                _currentStage.transform.DOMove(new Vector3(-20,0,0), 1f).SetEase(Ease.OutQuint);
                Destroy(_currentStage.gameObject, 1f);
            }

            if(index >= _stageObjects.Length) return;
//            _gameStateModel.ResetClickCount();

            await UniTask.Delay(3000);
            
            _audioManager.Play(Clip.StageSelect);
            var stage = _container.InstantiatePrefab(_stageObjects[index]).GetComponent<StageObject>();
            stage.transform.localPosition = new Vector3(20,0,0);
            stage.transform.SetParent(transform, false);
            stage.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.OutQuint);
            stage.OnStart();
            _currentStage = stage;
        }

        private void ReturnTitle()
        {
            if (_currentStage != null)
            {
                Destroy(_currentStage.gameObject);
            }
        }
    }
}