using System;
using System.Collections.Generic;

namespace CleverCrow.Fluid.Databases {
    public interface IKeyValueData<V> {
        V Get (string key, V defaultValue = default);
        void Set (string key, V value);
        bool Has (string key);

        void AddKeyListener (string key, Action<V> callback);
        void RemoveKeyListener (string test, Action<V> callback);

        void Clear ();
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

        public bool Has (string key) {
            return _data.ContainsKey(key);
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
