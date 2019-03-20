using LOGICA.Common;
using LOGICA.Switch;
using UniRx;
using UnityEngine;

namespace LOGICA.Logic
{
    public class ConductorLogic : LogicBase
    {
        [SerializeField] private SwitchObject _switch = default;
        [SerializeField] private Direction _trueDirection = default;

        private void Start()
        {
            _switch.OnConnect()
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
                {
                    SetConnect(_.IsConnect && _.CurrentDirection == _trueDirection);
                });
        }
    }
}