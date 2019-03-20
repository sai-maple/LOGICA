using LOGICA.Common;
using UniRx;
using UnityEngine;
using Zenject;

namespace LOGICA.Light
{
    public class LightObject : LogicBase
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [SerializeField] private LogicBase _conductor = default;
        [SerializeField] private ParticleSystem _light = default;

        private void Start()
        {
            _conductor.OnConnect()
                .TakeUntilDestroy(this)
                .Subscribe(Decision);
        }

        private void Decision(bool isConnect)
        {
            if (isConnect)
            {
                _light.Play();
                _audioManager.Play(Clip.LightOn);
            }
            else
            {
                _light.Stop();
                _audioManager.Play(Clip.LightOff);
            }
            SetConnect(isConnect);
        }
    }
}