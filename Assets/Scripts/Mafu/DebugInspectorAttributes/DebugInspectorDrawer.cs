using UnityEngine;
using UnityEditor;

namespace Mafu
{
	[CustomPropertyDrawer(typeof(DebugInspectorAttribute))]
	class DebugInspectorDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return 0f;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {}
	}
}