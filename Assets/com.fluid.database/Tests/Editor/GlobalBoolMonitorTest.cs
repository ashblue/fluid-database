using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Databases.Editors {
    public class GlobalBoolMonitorTest {
        const string KEY = "test";

        private IDatabaseInstance _database;
        private GlobalBoolMonitorInternal _monitor;
        private IKeyValueDefinition<bool> _definition;

        [SetUp]
        public void BeforeEach () {
            _definition = Substitute.For<IKeyValueDefinition<bool>>();
            _definition.Key.Returns(KEY);
            _definition.DefaultValue.Returns(true);

            _database = Substitute.For<IDatabaseInstance>();
            _monitor = new GlobalBoolMonitorInternal(_database, _definition);
        }

        public class UpdateEventMethod {
            public class TrueEvents : GlobalBoolMonitorTest {
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
