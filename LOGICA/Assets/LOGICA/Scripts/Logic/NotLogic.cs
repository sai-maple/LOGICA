using System;
using LOGICA.Common;
using UniRx;
using UnityEngine;

namespace LOGICA.Logic
{
    public class NotLogic : LogicBase
    {
        [SerializeField] private LogicBase _conductor = default;

        private void Awake()
        {
            _conductor.OnConnect()
                .TakeUntilDestroy(this)
                .Subscribe(_ => SetConnect(!_));
        }
    }
}