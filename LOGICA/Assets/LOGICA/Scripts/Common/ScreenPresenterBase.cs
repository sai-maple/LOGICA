using UnityEngine;

namespace LOGICA.Common
{
    public abstract class ScreenPresenterBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvas = default;
        [SerializeField] private Animator _animator = default;
        private readonly int[] _properties =
        {
            Animator.StringToHash("open"),
            Animator.StringToHash("close"),
            Animator.StringToHash("backIn"),
            Animator.StringToHash("moveOut")
        };
        
        public virtual void Open()
        {
            _canvas.blocksRaycasts = true;
            _animator.SetTrigger(_properties[0]);
        }

        public virtual void Close()
        {
            _canvas.blocksRaycasts = false;
            _animator.SetTrigger(_properties[1]);
        }
        
        public virtual void BackIn()
        {
            _canvas.blocksRaycasts = true;
            _animator.SetTrigger(_properties[2]);
        }
        
        public virtual void MoveOut()
        {
            _canvas.blocksRaycasts = false;
            _animator.SetTrigger(_properties[3]);
        }
    }
}
