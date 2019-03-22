using DG.Tweening;
using LOGICA.Common;
using LOGICA.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LOGICA.Presenter
{
    public class GameScreenPresenter : ScreenPresenterBase
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly WindowPresenter _windowPresenter = default;
        [Inject] private readonly GameStateModel _stateModel = default;
        [SerializeField] private Button _optionButton = default;
        [SerializeField] private Button _returnTitle = default;
        [SerializeField] private TextMeshProUGUI _stageLabel = default;
        
        private void Bind()
        {
            _optionButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _windowPresenter.OnStateChanged((int) ScreenState.Option);
                });
            _returnTitle.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Cancel);
                    _stateModel.SetGameState(GameState.Non);
                    _windowPresenter.OnStateChanged((int) ScreenState.Back);
                });

            var stream = _stateModel.OnGameStateChanged
                .TakeUntilDestroy(this)
                .Publish()
                .RefCount();

            stream.Where(_ => _ == GameState.OnClear)
                .Subscribe(_ =>
                {
                    MoveOut();
                    Invoke(nameof(NextStage), 2f);
                });
            stream.Where(_ => _ == GameState.OnPlay)
                .Subscribe(_ =>
                {
                    base.Open();
                    var sequence = DOTween.Sequence();
                    sequence.AppendInterval(1f);
                    sequence.Append(_stageLabel.DOFade(1.0f, 1f));
                    sequence.Append(_stageLabel.DOFade(0.0f, 1f));
                    sequence.Play();
                });
        }

        private async void NextStage()
        {
            _stateModel.SetStage(_stateModel.GetStage() + 1);
            _stateModel.SetGameState(GameState.OnPlay);
        }

        private void SetEvents()
        {            
            _stateModel.OnSelectStageChanged
                .TakeUntilDestroy(this)
                .Subscribe(_ => _stageLabel.text = $"{_ + 1}");
        }
        
        public override void Open()
        {
            base.Open();
            Bind();
            SetEvents();
        }
    }
}