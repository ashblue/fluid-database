using UnityEngine;

namespace CleverCrow.Fluid.Databases {
    public interface IKeyValueDefinition<V> {
        string Key { get; }
        V DefaultValue { get; }
    }

    public abstract class KeyValueDefinitionBase<V> : ScriptableObject, IKeyValueDefinition<V> {
        protected const string CREATE_PATH = "Fluid/Database";

        public string key;
        public V defaultValue;

        public string Key => key;
        public V DefaultValue => defaultValue;
    }
}
