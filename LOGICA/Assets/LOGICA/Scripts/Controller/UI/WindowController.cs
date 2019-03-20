using System;
using System.Collections.Generic;
using LOGICA.Common;
using LOGICA.Stage;
using UnityEngine;
using Zenject;

namespace LOGICA.Controller
{
    public class WindowController : WindowControllerBase
    {        
        [Inject] private readonly DiContainer _container = default;
        [SerializeField] private ScreenControllerBase[] _screenPrefabs = default;
        private readonly Stack<ScreenControllerBase> _screenStack = new Stack<ScreenControllerBase>();

        private void Start()
        {
            OnStateChanged((int)ScreenState.Title);
        }

        public override void OnStateChanged(int screenState)
        {
            OnStateChanged<WindowController>(screenState);
        }

        public override void OnStateChanged<T>(int screenState, Action<T> beforeOpenAction = null)
        {
            var status = (ScreenState) Enum.ToObject(typeof(ScreenState), screenState);
            switch (status)
            {
                case ScreenState.Reset:
                    ResetScreen();
                    break;
                case ScreenState.Back:
                    BackScreen();
                    break;
                default:
                    MoveScreen(status, beforeOpenAction);
                    break;
            }
        }

        private void MoveScreen<T>(ScreenState screenState, Action<T> beforeOpenAction = null)
        {
            if (_screenStack.Count != 0)
            {
                var screen = _screenStack.Peek();
                screen.MoveOut();
            }

            var nextScreen = _container.InstantiatePrefab(_screenPrefabs[(int) screenState]).GetComponent<ScreenControllerBase>();
            nextScreen.transform.SetParent(transform, false);
            _screenStack.Push(nextScreen);
            beforeOpenAction?.Invoke(nextScreen.GetComponent<T>());
            nextScreen.Open();
        }

        private void BackScreen()
        {
            var screen = _screenStack.Pop();
            screen.Close();
            Destroy(screen.gameObject, 1f);
            
            _screenStack.Peek().BackIn();
        }
        
        private void ResetScreen()
        {
            var currentScreen = _screenStack.Pop();
            currentScreen.Close();
            Destroy(currentScreen.gameObject, 1f);
            while (_screenStack.Count > 1)
            {
                var screen = _screenStack.Pop();
                screen.Close();
                Destroy(screen.gameObject);
            }
            _screenStack.Peek().BackIn();
        }
    }
    
    public enum ScreenState
    {
        Title,
        Game,
        Option,
        Clear,
        Back,
        Reset,
    }
}
