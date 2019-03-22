using System;
using LOGICA.Common;
using LOGICA.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
//using naichilab;

namespace LOGICA.Presenter
{
    // クリア画面廃止
    public class ClearScreenPresenter : ScreenPresenterBase
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly WindowPresenter _windowPresenter = default;
        [Inject] private readonly GameStateModel _stateModel = default;

        [SerializeField] private Button _nextStageButton = default;
        [SerializeField] private Button _tweetButton = default;
        [SerializeField] private Button _titleButton = default;

        [SerializeField] private TextMeshProUGUI _stageLabel = default;

        private void Initialize()
        {
            _audioManager.Play(Clip.Clear);
            _stageLabel.text = $"Stage {_stateModel.GetStage() + 1}";
        }
        
        private void Bind()
        {
            _nextStageButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
                    _stateModel.SetStage(_stateModel.GetStage() + 1);
                    _stateModel.SetGameState(GameState.OnPlay);
                    _windowPresenter.OnStateChanged((int) ScreenState.Back);
                });
            _titleButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Cancel);
                    _stateModel.SetGameState(GameState.Non);
                    _windowPresenter.OnStateChanged((int) ScreenState.Reset);
                });
            _tweetButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Apply);
//                    UnityRoomTweet.Tweet ("libertywall_logica", $"ステージ{_stateModel.GetStage() + 1}を{_stateModel.ClickCount}手でクリア。", "unityroom", "unity1week");
                });
        }

        public override void Open()
        {
            base.Open();
            Initialize();
            Bind();
        }
    }
}
