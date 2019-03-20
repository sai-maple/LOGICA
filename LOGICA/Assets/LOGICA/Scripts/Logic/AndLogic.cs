using LOGICA.Common;
using UniRx;
using UnityEngine;

namespace LOGICA.Logic
{
    public class AndLogic : LogicBase
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
            var isConnect = false;
            foreach (var conductor in _conductors)
            {
                if (!conductor.IsConnect())
                {
                    return;
                }
                isConnect = true;
            }
            if(isConnect == IsConnect()) return;
            SetConnect(isConnect);
        }
    }
}