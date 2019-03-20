using System;
using JetBrains.Annotations;
using LOGICA.Common;
using UniRx;
using UnityEngine;

namespace LOGICA.Switch
{    
    public class SwitchObject : MonoBehaviour
    {
        [CanBeNull] [SerializeField] private LogicBase _verticalConductor = default;
        [CanBeNull] [SerializeField] private LogicBase _horizontalConductor = default;
        [SerializeField] private Direction _direction = default;
        private readonly ReactiveProperty<InputState> _inputState = new ReactiveProperty<InputState>();
        private readonly Subject<Unit> _onCollide = new Subject<Unit>();
        private readonly Subject<SwitchConnection> _isConnecting = new Subject<SwitchConnection>();

        private void Start()
        {            
            if (_verticalConductor != null)
            {
                _verticalConductor.OnConnect()
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => Decision(GetDirection()));
            }

            if (_horizontalConductor != null)
            {
                _horizontalConductor.OnConnect()
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => Decision(GetDirection()));
            }
        }
        
        public IObservable<SwitchConnection> OnConnect()
        {
            return _isConnecting;
        }

        private void Decision(Direction direction)
        {
            if (direction == Direction.Vertical)
            {
                _isConnecting.OnNext(new SwitchConnection
                {
                    IsConnect = _verticalConductor != null && _verticalConductor.IsConnect(),
                    CurrentDirection = GetDirection()
                });
            }
            else
            {
                _isConnecting.OnNext(new SwitchConnection
                {
                    IsConnect = _horizontalConductor != null && _horizontalConductor.IsConnect(),
                    CurrentDirection = GetDirection()
                });
            }
        }

        public void SetClick(InputState state)
        {
            _inputState.Value = state;
        }

        public IObservable<InputState> OnInputStateChanged()
        {
            return _inputState;
        }

        public void OnCollision()
        {
            _onCollide.OnNext(default);
        }

        public IObservable<Unit> OnLineCollided()
        {
            return _onCollide;
        }

        public void SetDirection(Direction direction)
        {
            _direction = direction;
            Decision(direction);
        }

        public Direction GetDirection()
        {
            return _direction;
        }
    }

    public class SwitchConnection
    {
        public Direction CurrentDirection;
        public bool IsConnect;
    }
}