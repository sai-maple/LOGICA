using System.Linq;
using LOGICA.Controller;
using LOGICA.Light;
using LOGICA.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace LOGICA.Stage
{
    public class StageObject : MonoBehaviour
    {
        [Inject] private readonly GameStateModel _gameStateModel = default;
        [Inject] private readonly WindowController _windowController = default;
        [SerializeField] private LightObject[] _lightObjects = default;

        public void OnStart()
        {
            _gameStateModel.SetGameState(GameState.OnPlay);
            foreach (var lightObject in _lightObjects)
            {
                lightObject.OnConnect()
                    .TakeUntilDestroy(this)
                    .Subscribe(_ => OnCheckClear());
            }
        }

        private void OnCheckClear()
        {
            if (_lightObjects.Any(lightObject => !lightObject.IsConnect()))
            {
                return;
            }

            _windowController.OnStateChanged((int) ScreenState.Clear);
            _gameStateModel.SetGameState(GameState.OnClear);
        }
    }
}
