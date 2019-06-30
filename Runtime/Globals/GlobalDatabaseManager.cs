using CleverCrow.Fluid.Utilities;

namespace CleverCrow.Fluid.Databases {
    public class GlobalDatabaseManager : Singleton<GlobalDatabaseManager> {
        public DatabaseInstance Database { get; } = new DatabaseInstance();

        public string Save () {
            return Database.Save();
        }

        public void Load (string save) {
            Database.Load(save);
        }
    }
}
