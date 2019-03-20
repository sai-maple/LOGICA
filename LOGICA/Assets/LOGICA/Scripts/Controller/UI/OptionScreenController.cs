﻿using LOGICA.Common;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LOGICA.Controller
{
    public class OptionScreenController : ScreenControllerBase
    {
        
        [Inject] private readonly WindowController _windowController = default;
        [Inject] private readonly AudioManager _audioManager = default;
        [SerializeField] private Slider _seSlider = default;
        [SerializeField] private Button _backButton = default;

        private void Initialize()
        {
            _seSlider.value = _audioManager.GetVolume();
        }
        
        private void Bind()
        {
            _seSlider.OnValueChangedAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_audioManager.SetVolume);
            _backButton.OnClickAsObservable()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    _audioManager.Play(Clip.Cancel);
                    _windowController.OnStateChanged((int) ScreenState.Back);
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