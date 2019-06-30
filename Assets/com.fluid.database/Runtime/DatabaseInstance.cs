using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.Databases {
    public interface IDatabaseInstance {
        IKeyValueData<bool> Bools { get; }
        IKeyValueData<string> Strings { get; }
        IKeyValueData<int> Ints { get; }
        IKeyValueData<float> Floats { get; }

        void Clear ();
        string Save ();
        void Load (string save);
    }

    public class DatabaseInstance : IDatabaseInstance {
        public IKeyValueData<bool> Bools { get; } = new KeyValueDataBool();
        public IKeyValueData<string> Strings { get; } = new KeyValueDataString();
        public IKeyValueData<int> Ints { get; } = new KeyValueDataInt();
        public IKeyValueData<float> Floats { get; } = new KeyValueDataFloat();

        public class SaveData {
            public string strings;
            public string bools;
            public string ints;
            public string floats;
        }

        public void Clear () {
            Strings.Clear();
            Bools.Clear();
            Ints.Clear();
            Floats.Clear();
        }

        public string Save () {
            return JsonUtility.ToJson(new SaveData {
                strings = Strings.Save(),
                bools = Bools.Save(),
                ints = Ints.Save(),
                floats = Floats.Save(),
            });
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<SaveData>(save);

            Strings.Load(data.strings);
            Bools.Load(data.bools);
            Ints.Load(data.ints);
            Floats.Load(data.floats);
        }
    }
}
