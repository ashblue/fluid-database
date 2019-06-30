using CleverCrow.DungeonsAndHumans.Databases;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.Editors.Testing.Databases {
    public class KeyValueData {
        private KeyValueDataBase<int> _keyValue;

        [SetUp]
        public void BeforeEach () {
            _keyValue = new KeyValueDataInt();
        }

        public class AddKeyListenerMethod : KeyValueData {
            private int _result;

            [SetUp]
            public void BeforeEachMethod () {
                _result = 0;
                _keyValue.AddKeyListener("key", (value) => _result += value);
            }

            [Test]
            public void It_should_trigger_when_Set_is_called () {
                _keyValue.Set("key", 2);

                Assert.AreEqual(2, _result);
            }

            [Test]
            public void It_should_not_trigger_when_a_different_Set_is_called () {
                _keyValue.Set("other", 2);

                Assert.AreEqual(0, _result);
            }

            [Test]
            public void It_should_trigger_multiple_listeners () {
                _keyValue.AddKeyListener("key", (value) => _result += 1);
                _keyValue.AddKeyListener("key", (value) => _result += 1);
                _keyValue.Set("key", 0);

                Assert.AreEqual(2, _result);
            }
        }

        public class RemoveKeyListenerMethod : KeyValueData {
            private int result = 0;

            private void MyListen (int value) {
                result += value;
            }

            [Test]
            public void It_should_not_fire_a_listener_after_removal () {
                _keyValue.AddKeyListener("key", MyListen);
                _keyValue.RemoveKeyListener("key", MyListen);
                _keyValue.Set("key", 2);

                Assert.AreEqual(0, result);
            }
        }

        public class SetMethod : KeyValueData {
            [Test]
            public void It_should_set_a_key_value_pair () {
                _keyValue.Set("key", 2);
            }

            [Test]
            public void It_should_silently_fail_if_key_is_empty () {
                _keyValue.Set("", 2);
            }

            [Test]
            public void It_should_silently_fail_if_key_is_null () {
                _keyValue.Set(null, 2);
            }
        }

        public class GetMethod : KeyValueData {
            [Test]
            public void It_should_return_a_key_value () {
                _keyValue.Set("key", 5);
                var result = _keyValue.Get("key");

                Assert.AreEqual(5, result);
            }

            [Test]
            public void It_should_return_the_defult_type_if_key_is_empty () {
                var result = _keyValue.Get("");

                Assert.AreEqual(0, result);
            }

            [Test]
            public void It_should_return_the_defult_type_if_key_is_null () {
                var result = _keyValue.Get(null);

                Assert.AreEqual(0, result);
            }

            [Test]
            public void It_should_return_a_default_value_if_key_is_empty () {
                var result = _keyValue.Get("", 7);

                Assert.AreEqual(7, result);
            }

            [Test]
            public void It_should_return_a_default_value_if_key_is_null () {
                var result = _keyValue.Get(null, 7);

                Assert.AreEqual(7, result);
            }

            [Test]
            public void It_should_return_the_type_default_if_key_does_not_exist () {
                var result = _keyValue.Get("asdf");

                Assert.AreEqual(0, result);
            }

            [Test]
            public void It_should_return_the_default_value_if_key_does_not_exist () {
                var result = _keyValue.Get("asdf", 7);

                Assert.AreEqual(7, result);
            }
        }

        public class ClearMethod : KeyValueData {
            [Test]
            public void It_should_clear_written_values () {
                _keyValue.Set("key", 2);

                _keyValue.Clear();

                Assert.AreEqual(0, _keyValue.Get("key"));
            }
        }
    }
}
