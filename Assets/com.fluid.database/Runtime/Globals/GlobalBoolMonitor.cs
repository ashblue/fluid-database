using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using CleverCrow.Fluid.Utilities.UnityEvents;

namespace CleverCrow.Fluid.Databases {
    /// <summary>
    /// Triggers the proper event on `Start()` and triggers the opposite event if the variable changes value
    /// </summary>
    public class GlobalBoolMonitor : MonoBehaviour {
        private GlobalBoolMonitorInternal _internal;

        [SerializeField]
        private KeyValueDefinitionBool[] _booleans = new KeyValueDefinitionBool[1];

        [SerializeField]
        private UnityEvent _eventTrue = new UnityEvent();

        [SerializeField]
        private UnityEvent _eventFalse = new UnityEvent();

        private void Start () {
            var copies = _booleans.Select(Instantiate).ToArray<IKeyValueDefinition<bool>>();
            _internal = new GlobalBoolMonitorInternal(
                GlobalDatabaseManager.Instance.Database, copies);

            _internal.EventTrue.AddListener(_eventTrue.Invoke);
            _internal.EventFalse.AddListener(_eventFalse.Invoke);

            _internal.UpdateEvent();
            _internal.BindChangeMonitor();
        }

        private void OnDestroy () {
            if (!GlobalDatabaseManager.IsInstance) return;
            _internal?.UnbindChangeMonitor();
        }
    }

    public class GlobalBoolMonitorInternal {
        private readonly IDatabaseInstance _database;
        private readonly IKeyValueDefinition<bool>[] _definitions;

        public IUnityEvent EventTrue { get; } = new UnityEventPlus();
        public IUnityEvent EventFalse { get; } = new UnityEventPlus();

        public GlobalBoolMonitorInternal (IDatabaseInstance database, IKeyValueDefinition<bool>[] definitions) {
            _database = database;
            _definitions = definitions;
        }

        public void UpdateEvent () {
            var isTrue = true;
            foreach (var definition in _definitions) {
                if (!_database.Bools.Get(definition.Key, definition.DefaultValue)) {
                    isTrue = false;
                }
            }

            if (isTrue) {
                EventTrue.Invoke();
            } else {
                EventFalse.Invoke();
            }
        }

        public void BindChangeMonitor () {
            foreach (var definition in _definitions) {
                _database.Bools.AddKeyListener(definition.Key, BindMethod);
            }
        }

        private void BindMethod (bool value) {
            UpdateEvent();
        }

        public void UnbindChangeMonitor () {
            foreach (var definition in _definitions) {
                _database.Bools.RemoveKeyListener(definition.Key, BindMethod);
            }
        }
    }
}
