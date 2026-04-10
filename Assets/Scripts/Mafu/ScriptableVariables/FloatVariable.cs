using UnityEngine;

namespace Mafu.ScriptableVariables
{
    [CreateAssetMenu(menuName = "Variables/Float Variable")]
    public class FloatVariable : ScriptableVariable<float>
    {
        // public static FloatVariable operator ++(FloatVariable variable)
        // {
        //     variable.Value++;
        //     return variable;
        // }

        // public static FloatVariable operator --(FloatVariable variable)
        // {
        //     variable.Value--;
        //     return variable;
        // }

        // public static FloatVariable operator +(FloatVariable variable, int value)
        // {
        //     variable.Value += value;
        //     return variable;
        // }

        // public static FloatVariable operator -(FloatVariable variable, int value)
        // {
        //     variable.Value -= value;
        //     return variable;
        // }

        // public static bool operator ==(FloatVariable variable, float value) => variable.Value == value;
        // public static bool operator !=(FloatVariable variable, float value) => variable.Value != value;

        // public override bool Equals(object other) => base.Equals(other);
        // public override int GetHashCode() => base.GetHashCode();
    }
}