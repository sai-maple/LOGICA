using System.Linq;
using LOGICA.Common;
using UniRx;
using UnityEngine;

namespace LOGICA.Logic
{
    public class OrLogic : LogicBase
    {
        [SerializeField] private LogicBase[] _conductors = default;

        private void Start()
        {
            foreach (var conductor in _conductors)
            {
                conductor.OnConnect()
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => Decision());
            }
        }

        private void Decision()
        {
            var isConnect = _conductors.Any(conductor => conductor.IsConnect());
            SetConnect(isConnect);
        }
    }
}