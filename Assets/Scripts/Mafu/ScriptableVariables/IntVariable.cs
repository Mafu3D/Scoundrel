using UnityEngine;

namespace Mafu.ScriptableVariables
{

    [CreateAssetMenu(menuName = "Variables/Int Variable")]
    public class IntVariable : ScriptableVariable<int>
    {
        // public static IntVariable operator ++(IntVariable variable)
        // {
        //     variable.Value++;
        //     return variable;
        // }

        // public static IntVariable operator --(IntVariable variable)
        // {
        //     variable.Value--;
        //     return variable;
        // }

        // public static IntVariable operator +(IntVariable variable, int value)
        // {
        //     variable.Value += value;
        //     return variable;
        // }

        // public static IntVariable operator -(IntVariable variable, int value)
        // {
        //     variable.Value -= value;
        //     return variable;
        // }

        // public static bool operator ==(IntVariable variable, int value) => variable.Value == value;
        // public static bool operator !=(IntVariable variable, int value) => variable.Value != value;

        // public override bool Equals(object other) => base.Equals(other);
        // public override int GetHashCode() => base.GetHashCode();
    }
}