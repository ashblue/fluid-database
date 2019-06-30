using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.Databases {
    public interface IKeyValueDefinition<V> {
        string Key { get; }
        V DefaultValue { get; }
    }
    
    public abstract class KeyValueDefinitionBase<V> : ScriptableObject, IKeyValueDefinition<V> {
        public string key;
        public V defaultValue;
        
        public string Key => key;
        public V DefaultValue => defaultValue;
    }
}
