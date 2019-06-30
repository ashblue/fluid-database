using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.Databases.Editors {
    public class DatabaseInstanceTest {
        public class SaveMethod {
            [Test]
            public void It_should_return_all_KeyValueData_in_a_string () {
                var database = new DatabaseInstance();
                database.Strings.Set("a", "b");
                database.Bools.Set("a", true);
                database.Ints.Set("a", 2);
                database.Floats.Set("a", 5);

                var strings = new KeyValueDataString();
                strings.Set("a", "b");

                var bools = new KeyValueDataBool();
                bools.Set("a", true);

                var ints = new KeyValueDataInt();
                ints.Set("a", 2);

                var floats = new KeyValueDataFloat();
                floats.Set("a", 5);

                var save = JsonUtility.ToJson(new DatabaseInstance.SaveData {
                    strings = strings.Save(),
                    bools = bools.Save(),
                    ints = ints.Save(),
                    floats = floats.Save(),
                });

                var saveResult = database.Save();

                Assert.AreEqual(save, saveResult);
            }
        }

        public class LoadMethod {
            [Test]
            public void It_should_load_save_data () {
                var database = new DatabaseInstance();
                database.Strings.Set("a", "b");
                database.Bools.Set("a", true);
                database.Ints.Set("a", 2);
                database.Floats.Set("a", 5);

                var save = database.Save();
                database.Clear();
                database.Load(save);

                Assert.AreEqual("b", database.Strings.Get("a"));
                Assert.AreEqual(true, database.Bools.Get("a"));
                Assert.AreEqual(2, database.Ints.Get("a"));
                Assert.AreEqual(5, database.Floats.Get("a"));
            }
        }
    }
}
