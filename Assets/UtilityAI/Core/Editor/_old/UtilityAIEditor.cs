//namespace UtilityAI
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using UnityEngine;
//    using UnityEditor;
//    using UnityEditorInternal;


//    [CustomEditor(typeof(TaskNetworkComponent))]
//    public class UtilityAIEditor : Editor
//    {
//        public static string nameID_fieldName = "nameID";
//        public static string logicType_fieldName = "logicType";
//        public static string qualifiers_fieldName = "qualifiers";
//        public static string action_fieldName = "actionOption";
//        public static string isSelected_fieldName = "isSelected";


//        private TaskNetworkComponent utilityAIComponent;
//        private List<SerializedObject> soUtilityAI = new List<SerializedObject>();

//        private List<string> utilityAIOptions = new List<string>();
//        private int utilityAI_index;


//        private void OnEnable()
//        {
//            if (target == null) 
//                return;
            
//            utilityAIComponent = (TaskNetworkComponent)target;

//            soUtilityAI.Clear();
//            utilityAIOptions.Clear();

//            foreach(UtilityAIClient utilityAI in utilityAIComponent.clients){
//                if (utilityAI != null){
//                    var so = new SerializedObject(utilityAI);
//                    soUtilityAI.Add(so);
//                    utilityAIOptions.Add(utilityAI.name);
//                }
//            }
                
//        }


//        public override void OnInspectorGUI()
//        {
//            //DrawDefaultInspector(); //base.OnInspectorGUI();
//            EditorGUI.BeginChangeCheck();

//            utilityAIComponent.currentTab = GUILayout.Toolbar(utilityAIComponent.currentTab, new string[] { "Utility AI Component", "Utility AI Editor", "Referenced Types" });


//            switch (utilityAIComponent.currentTab)
//            {
//                case 0:
//                    DrawDefaultInspector(); //base.OnInspectorGUI();
//                    DrawDefaultTab();
//                    break;
//                case 1:
//                    DrawEditorTab();
//                    break;
//                case 2:
//                    DrawReferencedTypesTab();
//                    break;
//            }



//            if (EditorGUI.EndChangeCheck())
//            {
//                GUI.FocusControl(null);
//            }

//        }



//        private void DrawDefaultTab()
//        {
//            //EditorGUILayout.PropertyField(utilityAIs);

//            /*  This is the very last button for updating the AI scriptable object. */
//            if (GUILayout.Button("Debug AI")){
//                //Debug.Log("Clicked the button");
//                UtilityAIManager.UpdateOptionClassMapping(true);
//                UtilityAIManager.GetOptionType(typeof(CompositeScoreQualifier));
//            }
//        }


//        private void DrawEditorTab()
//        {
//            utilityAI_index = EditorGUILayout.Popup("UtilityAIClient:", utilityAI_index, utilityAIOptions.ToArray());

//            DrawEditorContents(utilityAI_index);


//            /*  This is the very last button for updating the AI scriptable object. */
//            //if (GUILayout.Button("Update AI")){
//            //    //Debug.Log("Clicked the button");
//            //    //UtilityAIManager.GetFieldInfoAndValue(typeof(CompositeScoreQualifier));
//            //}
//        }

//        private void DrawEditorContents(int index)
//        {
//            if (soUtilityAI.Count > 0)
//            {
//                soUtilityAI[index].Update();
//                /* This Displays the List of Qualifiers and its action for this Selector.  */
//                SerializedProperty rootSelector = soUtilityAI[index].FindProperty("selectorOption");

//                //  Displays the reorderable list of Qualifiers and its Actions.
//                EditorGUILayout.PropertyField(rootSelector);
//                /* This displays the selected item in the Selectors reorderable list. */
//                //  TODO: need to find the selection.


//                SerializedProperty qualifiers = rootSelector.FindPropertyRelative(qualifiers_fieldName);

//                var selectedIndex = -1;
//                var actionIsSelected = false;
//                for (int i = 0; i < qualifiers.arraySize; i++)
//                {
//                    SerializedProperty isSelected = qualifiers.GetArrayElementAtIndex(i).FindPropertyRelative(isSelected_fieldName);
//                    SerializedProperty isActionSelected = qualifiers.GetArrayElementAtIndex(i).FindPropertyRelative( action_fieldName + "."  + isSelected_fieldName);
//                    if (isSelected.boolValue == true)
//                    {
//                        selectedIndex = i;
//                        actionIsSelected = false;
//                        break;
//                    }
//                    if (isActionSelected.boolValue == true)
//                    {
//                        selectedIndex = i;
//                        actionIsSelected = true;
//                        break;
//                    }
//                }

//                if (selectedIndex >= 0)
//                {
//                    SerializedProperty selected_element = actionIsSelected == false ? qualifiers.GetArrayElementAtIndex(selectedIndex) : qualifiers.GetArrayElementAtIndex(selectedIndex).FindPropertyRelative(action_fieldName);
//                    EditorGUILayout.PropertyField(selected_element);
//                }



//                if (GUILayout.Button("Update AI")){
//                    //Debug.Log("Clicked the button");
//                    Debug.Log(rootSelector.propertyPath);
//                }

//                soUtilityAI[index].ApplyModifiedProperties();
//            }
//        }




//        private void DrawReferencedTypesTab()
//        {
//            //EditorGUILayout.LabelField(new GUIContent("Referenced Types"), Styles.defaultTextStyle);
//        }


//    }







//    public enum OptionWindowType
//    {
//        Add,
//        ChangeType
//    }

//    public class TaskNetworkOptionsWindow : EditorWindow // where T : class
//    {
//        TaskNetworkOptionsWindow window;
//        OptionWindowType windowType;
//        SerializedProperty property;
//        List<Type> availibleTypes = new List<Type>();
//        Type optionType;      


//        public void Init(TaskNetworkOptionsWindow window, Type type, SerializedProperty property, OptionWindowType windowType)
//        {
//            this.window = window;
//            this.window.titleContent = new GUIContent(type.Name + " Search");
//            this.window.minSize = this.window.maxSize = new Vector2(250, 350);
//            this.windowType = windowType;

//            availibleTypes = UtilityAIManager.GetAllAvailibleOfType(type);
//            optionType = UtilityAIManager.GetOptionType(type);
//            this.property = property;

//            this.window.ShowUtility();
//        }


//        void OnGUI()
//        {
//            var count = availibleTypes.Count;
//            for (int i = 0; i < count; i++)
//            {
//                Type type = availibleTypes[i]; 
//                if (GUILayout.Button(new GUIContent(type.Name)))
//                {
//                    switch(windowType)
//                    {
//                        case OptionWindowType.ChangeType:
//                            //  We need to get the serialized object of the property and Update/ApplyModifiedProperties.
//                            property.serializedObject.Update();
//                            this.property.stringValue = type.FullName;  //  Type name should be the full name
//                            property.serializedObject.ApplyModifiedProperties();
//                            break;

//                        case OptionWindowType.Add:
                            
//                            if(property.isArray)
//                            {
//                                var newOption = Activator.CreateInstance(optionType, type);
//                                //  Insert new Array element
//                                property.InsertArrayElementAtIndex(property.arraySize);
//                                SerializedProperty element = property.GetArrayElementAtIndex(property.arraySize - 1);
//                                //  Update the SerializedProperty object so it has the new Array list.
//                                element.serializedObject.ApplyModifiedProperties();

//                                element.SetValue(newOption);
//                            }
//                            break;
//                    }  //  End of Switch statement

//                    this.window.Close();

//                }  //  End of GUILayout.Button
//            }  //  End of for loop
//        }

//    }




//    //public static class Styles
//    //{
//    //    public static GUIStyle headerStyle;
//    //    public static GUIStyle defaultTextStyle;

//    //    public static GUIStyle selectorContainerStyle;


//    //    public static Texture changeElementIcon;
//    //    public static Texture deleteElementIcon;
//    //    public static Texture childIcon;


//    //    public static float lineHeight;
//    //    public static float lineHeightSpace;



//    //    static Styles()
//    //    {
//    //        headerStyle = new GUIStyle(); //new GUIStyle(EditorStyles.toolbar);  "ShurikenModuleTitle"
//    //        headerStyle.fontStyle = FontStyle.Bold;
//    //        headerStyle.alignment = TextAnchor.MiddleLeft;
//    //        headerStyle.fontSize = 12;

//    //        defaultTextStyle = new GUIStyle();
//    //        defaultTextStyle.fontStyle = FontStyle.Bold;

//    //        selectorContainerStyle = new GUIStyle();
//    //        selectorContainerStyle.normal.background = EditorGUIUtility.Load("ShurikenModuleBg") as Texture2D;
//    //        selectorContainerStyle.border = new RectOffset(6, 3, 3, 6);


//    //        changeElementIcon = EditorGUIUtility.IconContent("Toolbar Plus").image;
//    //        deleteElementIcon = EditorGUIUtility.IconContent("Toolbar Minus").image;

//    //        childIcon = EditorGUIUtility.IconContent("IN RenderLayer").image;   // HierarchyTreeMiddle     IN RenderLayer - this is the L shape parent to child icon


//    //        lineHeight = EditorGUIUtility.singleLineHeight;
//    //        lineHeightSpace = lineHeight + 10;
//    //    }

//    //}





//    //public static class SerializedPropertyExtensions
//    //{
//    //    public static T GetActualObjectForSerializedProperty(this SerializedProperty property) where T : class
//    //    {
//    //        var serializedObject = property.serializedObject;
//    //        if (serializedObject == null)
//    //        {
//    //            return null;
//    //        }
//    //        var targetObject = serializedObject.targetObject;
//    //        var field = targetObject.GetType().GetField(property.name);
//    //        var obj = field.GetValue(targetObject);
//    //        if (obj == null)
//    //        {
//    //            return null;
//    //        }
//    //        T actualObject = null;
//    //        if (obj.GetType().IsArray)
//    //        {
//    //            var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
//    //            actualObject = ((T[])obj)[index];
//    //        }
//    //        else
//    //        {
//    //            actualObject = obj as T;
//    //        }
//    //        return actualObject;
//    //    }
//    //}

//}