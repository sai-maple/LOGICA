using System.Collections;
using DG.Tweening;
using LOGICA.Common;
using LOGICA.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace LOGICA.Switch
{
    public class SwitchAnimator : MonoBehaviour
    {
        [Inject] private readonly AudioManager _audioManager = default;
        [Inject] private readonly GameStateModel _stateModel = default;
        [SerializeField] private SwitchObject _switchObject = default;
        [SerializeField] private Transform _point = default;
        [SerializeField] private Transform _line = default;

        private readonly Vector2 _animatedScale = new Vector2(0, 1);
        private readonly Vector2 _defaultScale = new Vector2(1, 1);
        private readonly Vector2 _selectedScale = new Vector2(1.1f, 1.1f);
        private readonly Vector3 _vertical = new Vector3(0, 0, 90);
        private readonly Vector3 _horizontal = new Vector3(0,0,0);

        private Sequence _sequence = default;

        private void Start()
        {
            var stream = _switchObject.OnInputStateChanged()
                .TakeUntilDestroy(this)
                .Publish()
                .RefCount();
            
            stream
                .Where(s => s == InputState.Click)
                .Where(_ => !_stateModel.IsAnimated)
                .Subscribe(_ => ClickAnimationOnPlay());
            stream
                .Where(s => s == InputState.Enter)
                .Subscribe(s => PointerEnterAnimationOnPlay());
            stream
                .Where(s => s == InputState.Exit)
                .Subscribe(s => PointerExitAnimationOnPlay());
            
            _switchObject.OnLineCollided()
                .TakeUntilDestroy(this)
                .Where(_ => _stateModel.IsAnimated)
                .Subscribe(_ => StartCoroutine(CancelAnimationOnPlay()));
        }

        private void PointerEnterAnimationOnPlay()
        {
            _point.DOScale(_selectedScale, 0.2f);
        }

        private void PointerExitAnimationOnPlay()
        {
            _point.DOScale(_defaultScale, 0.2f);
        }

        private void ClickAnimationOnPlay()
        {
            _stateModel.SetAnimated(true);
            _audioManager.Play(Clip.Swich);
//            _stateModel.OnClick();
            _sequence = DOTween.Sequence();

            _sequence.Append(_line.DOScale(_animatedScale, 0.3f)).SetEase(Ease.InOutCubic);
            _sequence.Append(_switchObject.GetDirection() == Direction.Vertical
                ? transform.DORotate(_horizontal, 0.3f)
                : transform.DORotate(_vertical, 0.3f));
            _sequence.Append(_line.DOScale(_defaultScale, 0.3f)).SetEase(Ease.InOutCubic);
            _sequence.OnComplete(() =>
            {
                _switchObject.SetDirection(_switchObject.GetDirection() == Direction.Vertical
                    ? Direction.Horizontal
                    : Direction.Vertical);
                _switchObject.SetClick(InputState.Non);
                _stateModel.SetAnimated(false);
            }).SetEase(Ease.InOutCubic);
            _sequence.Play();
        }

        private IEnumerator CancelAnimationOnPlay()
        {
            _sequence.Pause();

            yield return new WaitForSeconds(0.3f);

            _sequence.PlayBackwards();

            _switchObject.SetClick(InputState.Non);
            _stateModel.SetAnimated(false);
        }
    }
}