using System;
using LOGICA.Common;
using LOGICA.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LOGICA.Controller
{
    public class TitleController : ScreenControllerBase
    {        
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly WindowController _windowController = default;
        [Inject] private readonly GameStateModel _stateModel = default;
        [SerializeField] private Button _startButton = default;
        [SerializeField] private Button _optionButton = default;
        [SerializeField] private Button _plusButton = default;
        [SerializeField] private Button _minusButton = default;
        [SerializeField] private TextMeshProUGUI _stageLabel = default;

        private void Bind()
        {
            _startButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _windowController.OnStateChanged((int) ScreenState.Game);
                    _stateModel.SetGameState(GameState.OnPlay);
                });
            
            _optionButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _windowController.OnStateChanged((int) ScreenState.Option);
                });
            
            _plusButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _stateModel.SetStage(_stateModel.GetStage() + 1);
                });
            _minusButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _stateModel.SetStage(_stateModel.GetStage() - 1);
                });
        }

        private void SetEvent()
        {
            _stateModel.OnSelectStageChanged
                .TakeUntilDestroy(this)
                .Subscribe(_ => _stageLabel.text = $"Stage {_ + 1}");
        }
        
        public override void Open()
        {
            base.Open();
            Bind();
            SetEvent();
        }
    }
}
