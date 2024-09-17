using GabrielBertasso.Debugger.Extensions;
using GabrielBertasso.Debugger.Helpers;
using GabrielBertasso.Debugger.Patterns;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GabrielBertasso.Debugger
{
    public class Debugger : PersistentSingleton<Debugger>
    {
        [Header("Actions")]
        [SerializeField] private Button _actionButtonPrefab;
        [SerializeField] private Transform _actionButtonsParent;

        [Header("Console")]
        [SerializeField] private TextMeshProUGUI _consoleTextPrefab;
        [SerializeField] private Transform _consoleTextsParent;
        [SerializeField] private Color _logColor = Color.black;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _errorColor = Color.red;

        private List<KeyValue<string, Button>> _buttonByName = new List<KeyValue<string, Button>>();

        private static Action<Debugger> Instantiated;
        

        protected override void Awake()
        {
            if (!SymbolsHelper.IsInDevelopment())
            {
                Destroy(gameObject);
                return;
            }

            base.Awake();
            if (Instance != this)
            {
                return;
            }

            Application.logMessageReceived += Application_logMessageReceived;

            try
            {
                Instantiated?.Invoke(this);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        public static void AddAction(Action action, string name)
        {
            if (HasInstance)
            {
                Instance.Instance_AddAction(action, name);
            }
            else
            {
                Instantiated += _ => Instance.Instance_AddAction(action, name);
            }
        }

        public static void RemoveAction(string name)
        {
            if (HasInstance)
            {
                Instance.Instance_RemoveAction(name);
            }
            else
            {
                Instantiated += _ => Instance.Instance_RemoveAction(name);
            }
        }

        private void Instance_AddAction(Action action, string name)
        {
            if (_actionButtonPrefab == null || ContainsAction(name))
            {
                return;
            }

            Transform parent = _actionButtonsParent != null ? _actionButtonsParent : transform;
            Button button = Instantiate(_actionButtonPrefab, parent);
            button.onClick.AddListener(action.Invoke);

            TextMeshProUGUI text = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            text.text = name;

            _buttonByName.Add(new KeyValue<string, Button>(name, button));
        }

        private void Instance_RemoveAction(string name)
        {
            List<KeyValue<string, Button>> buttons = _buttonByName.FindAll(x => x.Key == name);
            foreach (Button button in buttons.GetValues())
            {
                Destroy(button.gameObject);
            }

            _buttonByName.RemoveAll(x => x.Key == name);
        }

        private bool ContainsAction(string name)
        {
            return _buttonByName.Exists(x => x.Key == name);
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (_consoleTextPrefab == null)
            {
                return;
            }

            Transform parent = _consoleTextsParent != null ? _consoleTextsParent : transform;
            TextMeshProUGUI text = Instantiate(_consoleTextPrefab, parent);

            switch (type)
            {
                case LogType.Error or LogType.Assert or LogType.Exception:
                    text.color = _errorColor;
                    break;

                case LogType.Warning:
                    text.color = _warningColor;
                    break;

                case LogType.Log:
                    text.color = _logColor;
                    break;
            }

            text.text = condition + "\n" + stackTrace;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (!SymbolsHelper.IsInDevelopment())
            {
                return;
            }

            GameObject prefab = Resources.Load("Debugger", typeof(GameObject)) as GameObject;
            if (prefab == null)
            {
                Debug.LogWarning("Debugger prefab not found!");
                return;
            }

            Instantiate(prefab);
        }
    }
}