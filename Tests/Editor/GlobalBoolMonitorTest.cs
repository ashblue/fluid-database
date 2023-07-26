using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Databases.Editors {
    public class GlobalBoolMonitorTest {
        private const string KEY = "test";
        private const string KEY_ALT = "alt";

        private IDatabaseInstance _database;
        private GlobalBoolMonitorInternal _monitor;
        private IKeyValueDefinition<bool> _definition;
        private IKeyValueDefinition<bool> _definitionAlt;

        [SetUp]
        public void BeforeEach () {
            _definition = CreateDefinitionStub(KEY, true);
            _definitionAlt = CreateDefinitionStub(KEY_ALT, true);

            _database = Substitute.For<IDatabaseInstance>();
            _monitor = new GlobalBoolMonitorInternal(_database, new[] {_definition});
        }

        private IKeyValueDefinition<bool> CreateDefinitionStub (string key, bool defaultValue) {
            var definition = Substitute.For<IKeyValueDefinition<bool>>();
            definition.Key.Returns(key);
            definition.DefaultValue.Returns(defaultValue);

            return definition;
        }

        public class UpdateEventMethod {
            public class TrueEvents {
                public class SingleVariable : GlobalBoolMonitorTest {
                    [Test]
                    public void It_should_trigger_the_true_event_if_set_to_true_and_value_is_true () {
                        _database.Bools.Get(KEY, true).Returns(true);

                        var evenTriggered = false;
                        _monitor.EventTrue.AddListener(() => evenTriggered = true);

                        _monitor.UpdateEvent();

                        Assert.IsTrue(evenTriggered);
                    }

                    [Test]
                    public void It_should_not_trigger_the_true_event_if_set_to_true_and_value_is_false () {
                        _database.Bools.Get(KEY, true).Returns(false);

                        var evenTriggered = false;
                        _monitor.EventTrue.AddListener(() => evenTriggered = true);

                        _monitor.UpdateEvent();

                        Assert.IsFalse(evenTriggered);
                    }
                }

                public class MultiVariable : GlobalBoolMonitorTest {
                    [Test]
                    public void It_should_trigger_the_true_event_if_all_variables_are_true () {
                        _monitor = new GlobalBoolMonitorInternal(_database, new[] {_definition, _definitionAlt});

                        _database.Bools.Get(KEY, true).Returns(true);
                        _database.Bools.Get(KEY_ALT, true).Returns(true);

                        var evenTriggered = false;
                        _monitor.EventTrue.AddListener(() => evenTriggered = true);

                        _monitor.UpdateEvent();

                        Assert.IsTrue(evenTriggered);
                    }

                    [Test]
                    public void It_should_not_trigger_the_true_event_if_any_variables_are_false () {
                        _monitor = new GlobalBoolMonitorInternal(_database, new[] {_definition, _definitionAlt});

                        _database.Bools.Get(KEY, true).Returns(true);
                        _database.Bools.Get(KEY_ALT, true).Returns(false);

                        var evenTriggered = false;
                        _monitor.EventTrue.AddListener(() => evenTriggered = true);

                        _monitor.UpdateEvent();

                        Assert.IsFalse(evenTriggered);
                    }
                }
            }

            public class FalseEvents : GlobalBoolMonitorTest {
                [Test]
                public void It_should_trigger_the_false_event_if_set_to_false_and_value_is_false () {
                    _database.Bools.Get(KEY, true).Returns(false);

                    var evenTriggered = false;
                    _monitor.EventFalse.AddListener(() => evenTriggered = true);

                    _monitor.UpdateEvent();

                    Assert.IsTrue(evenTriggered);
                }

                [Test]
                public void It_should_not_trigger_the_false_event_if_set_to_false_and_value_is_true () {
                    _database.Bools.Get(KEY, true).Returns(true);

                    var evenTriggered = false;
                    _monitor.EventFalse.AddListener(() => evenTriggered = true);

                    _monitor.UpdateEvent();

                    Assert.IsFalse(evenTriggered);
                }
            }
        }

        public class BindChangeMonitorMethod : GlobalBoolMonitorTest {
            [Test]
            public void It_should_add_a_listener_to_boolean_key_values () {
                _monitor.BindChangeMonitor();

                _database.Bools.ReceivedWithAnyArgs(1).AddKeyListener(KEY, null);
            }
        }

        public class UnbindChangeMonitorMethod : GlobalBoolMonitorTest {
            [Test]
            public void It_should_add_a_listener_to_boolean_key_values () {
                _monitor.BindChangeMonitor();
                _monitor.UnbindChangeMonitor();

                _database.Bools.ReceivedWithAnyArgs(1).RemoveKeyListener(KEY, null);
            }
        }
    }
}
