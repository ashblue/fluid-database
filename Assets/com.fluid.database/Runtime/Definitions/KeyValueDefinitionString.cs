using UnityEngine;

namespace CleverCrow.Fluid.Databases {
    [CreateAssetMenu(
        menuName = CREATE_PATH + "/Key Value String",
        fileName = "KeyValueString")]
    public class KeyValueDefinitionString : KeyValueDefinitionBase<string> {
    }
}
