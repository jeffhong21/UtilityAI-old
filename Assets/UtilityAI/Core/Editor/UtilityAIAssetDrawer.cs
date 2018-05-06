namespace UtilityAI
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(UtilityAIAsset)) ]
    public class UtilityAIAssetDrawer : Editor
    {

        UtilityAIAsset obj;

		public void OnEnable()
		{
            obj = target as UtilityAIAsset;
		}


		public override void OnInspectorGUI()
		{
            //if (serializedObject.isEditingMultipleObjects)
            //{
            //    DrawDefaultInspector();
            //    return;
            //}
            DrawDefaultInspector();

            serializedObject.Update();


            GUILayout.Space(8);
            string config = DebugEditorUtilities.DebugSelectorInfo(obj.configuration.selector);
            EditorGUILayout.LabelField("Selector");
            EditorGUILayout.HelpBox(config, MessageType.Info);

            string rootConfig = DebugEditorUtilities.DebugSelectorInfo(obj.configuration.rootSelector);
            EditorGUILayout.LabelField("RootSelector");
            EditorGUILayout.HelpBox(rootConfig, MessageType.Info);

            serializedObject.ApplyModifiedProperties();
		}
	}
}
