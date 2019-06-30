using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.Databases {
    public class KeyValueDataString : KeyValueDataBase<string> {
        [System.Serializable]
        private class SaveData {
            public List<SaveKeyValue> keyValuePairs;
        }

        [System.Serializable]
        private class SaveKeyValue {
            public string key;
            public string value;
        }

        public override string Save () {
            return JsonUtility.ToJson(new SaveData {
                keyValuePairs = _data.ToArray()
                    .Select(kvp => new SaveKeyValue {
                        key = kvp.Key,
                        value = kvp.Value
                    })
                    .ToList()
            });
        }

        public override void Load (string save) {
            _data = JsonUtility
                .FromJson<SaveData>(save)
                .keyValuePairs
                .ToDictionary(k => k.key, v => v.value);
        }
    }
}
