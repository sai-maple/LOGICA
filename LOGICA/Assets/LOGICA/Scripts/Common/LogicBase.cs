using System;
using UniRx;
using UnityEngine;

namespace LOGICA.Common
{
    public class LogicBase : MonoBehaviour , IConductor
    {
        private readonly Subject<bool> _isConnecting = new Subject<bool>();
        private bool _lastValue = false;
        
        public IObservable<bool> OnConnect()
        {
            return _isConnecting;
        }

        protected void SetConnect(bool isConnect)
        {
            _lastValue = isConnect;
            _isConnecting.OnNext(isConnect);
        }

        public virtual bool IsConnect()
        {
            return _lastValue;
        }
    }
}