using UnityEngine;
using UnityEngine.Events;
using CleverCrow.Fluid.Databases.Utilities;

namespace CleverCrow.Fluid.Databases {
    /// <summary>
    /// Triggers the proper event on `Start()` and triggers the opposite event if the variable changes value
    /// </summary>
    public class GlobalBoolMonitor : MonoBehaviour {
        private GlobalBoolMonitorInternal _internal;

        [SerializeField]
        private KeyValueDefinitionBool _boolean;

        [SerializeField]
        private UnityEvent _eventTrue;

        [SerializeField]
        private UnityEvent _eventFalse;

        private void Start () {
            _internal = new GlobalBoolMonitorInternal(
                GlobalDatabaseManager.Instance.Database,
                Instantiate(_boolean));

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
        private readonly IKeyValueDefinition<bool> _definition;

        public IUnityEvent EventTrue { get; } = new UnityEventPlus();
        public IUnityEvent EventFalse { get; } = new UnityEventPlus();

        public GlobalBoolMonitorInternal (IDatabaseInstance database, IKeyValueDefinition<bool> definition) {
            _database = database;
            _definition = definition;
        }

        public void UpdateEvent () {
            if (_database.Bools.Get(_definition.Key, _definition.DefaultValue)) {
                EventTrue.Invoke();
            } else {
                EventFalse.Invoke();
            }
        }

        public void BindChangeMonitor () {
            _database.Bools.AddKeyListener(_definition.Key, BindMethod);
        }

        private void BindMethod (bool value) {
            UpdateEvent();
        }

        public void UnbindChangeMonitor () {
            _database.Bools.RemoveKeyListener(_definition.Key, BindMethod);
        }
    }
}
