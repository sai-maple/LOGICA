using LOGICA.Common;
using LOGICA.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LOGICA.Controller
{
    public class GameScreenController : ScreenControllerBase
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly WindowController _windowController = default;
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
                    _windowController.OnStateChanged((int) ScreenState.Option);
                });
            _returnTitle.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Cancel);
                    _stateModel.SetGameState(GameState.Non);
                    _windowController.OnStateChanged((int) ScreenState.Back);
                });
        }

        private void SetEvents()
        {            
            _stateModel.OnSelectStageChanged
                .TakeUntilDestroy(this)
                .Subscribe(_ => _stageLabel.text = $"Stage {_ + 1}");
        }
        
        public override void Open()
        {
            base.Open();
            Bind();
            SetEvents();
        }
    }
}