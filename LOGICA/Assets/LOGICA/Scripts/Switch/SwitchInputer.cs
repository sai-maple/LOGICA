using LOGICA.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LOGICA.Switch
{
    public class SwitchInputer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Inject] private readonly GameStateModel _gameStateModel = default;
        [SerializeField] private SwitchObject _switchObject = default;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_gameStateModel.GetGameState() != GameState.OnPlay) return;
            _switchObject.SetClick(InputState.Click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _switchObject.SetClick(InputState.Enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _switchObject.SetClick(InputState.Exit);
        }
    }
}