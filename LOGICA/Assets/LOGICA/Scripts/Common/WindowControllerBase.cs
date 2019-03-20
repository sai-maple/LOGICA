using System;
using UnityEngine;

namespace LOGICA.Common
{
    public class WindowControllerBase : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas = default;

        public virtual void OnStateChanged(int screenState)
        {

        }

        public virtual void OnStateChanged<T>(int screenState, Action<T> beforeOpenAction = null)
        {

        }

        public virtual void Open()
        {
            _canvas.enabled = true;
        }

        public virtual void Close()
        {
            _canvas.enabled = false;
        }

        public virtual void BackIn()
        {
            _canvas.enabled = true;
        }

        public virtual void MoveOut()
        {
            _canvas.enabled = false;
        }
    }
}