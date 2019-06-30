using System;
using System.Collections.Generic;

namespace CleverCrow.DungeonsAndHumans.Databases {
    public interface IKeyValueData<V> {
        void Clear ();
        V Get (string key, V defaultValue = default);
        void Set (string key, V value);
        void AddKeyListener (string key, Action<V> callback);
        void RemoveKeyListener (string test, Action<V> callback);
        string Save ();
        void Load (string save);
    }

    public abstract class KeyValueDataBase<V> : IKeyValueData<V> {
        private readonly Dictionary<string, List<Action<V>>> _callbacks = new Dictionary<string, List<Action<V>>>();
        protected Dictionary<string, V> _data = new Dictionary<string, V>();

        public void Set (string key, V value) {
            if (string.IsNullOrEmpty(key)) {
                return;
            }

            _data[key] = value;

            if (!_callbacks.TryGetValue(key, out var callbacks)) return;
            foreach (var callback in callbacks) {
                callback.Invoke(value);
            }
        }

        public V Get (string key, V defaultValue = default) {
            if (string.IsNullOrEmpty(key) || !_data.ContainsKey(key)) {
                return defaultValue;
            }

            return _data[key];
        }

        public void Clear () {
            _data.Clear();
        }

        public abstract string Save ();

        public abstract void Load (string save);

        public void AddKeyListener (string key, Action<V> callback) {
            if (!_callbacks.TryGetValue(key, out var list)) {
                list = new List<Action<V>>();
                _callbacks[key] = list;
            }

            _callbacks[key].Add(callback);
        }

        public void RemoveKeyListener (string key, Action<V> callback) {
            _callbacks[key].Remove(callback);
        }
    }
}
