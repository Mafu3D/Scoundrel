using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mafu.ScriptableVariables
{
    [CustomPropertyDrawer(typeof(IntVariable))]
    public class IntVariableDrawer : ScriptableVariablePropertyDrawer<IntVariable, int> { }

    [CustomPropertyDrawer(typeof(FloatVariable))]
    public class FloatVariableDrawer : ScriptableVariablePropertyDrawer<FloatVariable, float> { }

    [CustomPropertyDrawer(typeof(BoolVariable))]
    public class BoolVariableDrawer : ScriptableVariablePropertyDrawer<BoolVariable, bool> { }

    [CustomPropertyDrawer(typeof(StringVariable))]
    public class StringVariableDrawer : ScriptableVariablePropertyDrawer<StringVariable, string> { }

    public class ScriptableVariablePropertyDrawer<TVariable, TPrimitive> : PropertyDrawer where TVariable : ScriptableVariable<TPrimitive>
    {
        private const float PADDING_LEFT = 20f;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();

            ObjectField objectField = new ObjectField(property.displayName)
            {
                objectType = typeof(TVariable)
            };
            objectField.BindProperty(property);

            Label valueLabel = new();
            valueLabel.style.paddingLeft = PADDING_LEFT;

            container.Add(objectField);
            container.Add(valueLabel);

            objectField.RegisterValueChangedCallback(
                evt =>
                {
                    TVariable variable = evt.newValue as TVariable;
                    if (variable != null)
                    {
                        valueLabel.text = $"Current Value: {variable.Value}";
                        variable.OnValueChanged += newValue => valueLabel.text = $"Current Value: {newValue}";
                    }
                    else
                    {
                        valueLabel.text = string.Empty;
                    }
                }
            );

            TVariable currentVariable = property.objectReferenceValue as TVariable;
            if (currentVariable != null)
            {
                valueLabel.text = $"Current Value: {currentVariable.Value}";
                currentVariable.OnValueChanged += newValue => valueLabel.text = $"Current Value: {newValue}";
            }

            return container;
        }
    }
}