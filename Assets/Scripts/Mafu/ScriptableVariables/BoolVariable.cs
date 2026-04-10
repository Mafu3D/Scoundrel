using UnityEngine;

namespace Mafu.ScriptableVariables
{
    [CreateAssetMenu(menuName = "Variables/Bool Variable")]
    public class BoolVariable : ScriptableVariable<bool>
    {
        // public static bool operator ==(BoolVariable variable, bool value) => variable.Value == value;
        // public static bool operator !=(BoolVariable variable, bool value) => variable.Value != value;
        // public static bool operator !(BoolVariable variable, bool value) => variable.Value != value;

        // public override bool Equals(object other) => base.Equals(other);
        // public override int GetHashCode() => base.GetHashCode();
    }
}