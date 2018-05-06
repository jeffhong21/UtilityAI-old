//namespace UtilityAI
//{
//    using UnityEngine;
//    using UnityEditor;
//    using UnityEditorInternal;
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using System.Linq;

//    /// <summary>
//    //  
//    /// </summary>
//    [CustomPropertyDrawer(typeof(SelectorOption))]
//    public class SelectorOptionsDrawer : PropertyDrawer
//    {
//        ReorderableList list;
//        int selectedElement;                                    //  Which Qualifier or action is selected using controlID.  If -1, nothing selected

//        Rect clickBounds;                                       //  This is the rect of Selector container so selecting/deselcting is within this property.
//        int headerHeight = 24;

//        Vector2 contentOffset = new Vector2(25, -3);
//        Vector2 childContentOffset = new Vector2(40, -2);




//        /* Field Names of SelectorOption property */
//        string qualifiersFieldName = "qualifiers";
//        string actionFieldName = "actionOption";
//        string nameIDFieldName = "nameID";
//        string isSelectedFieldName = "isSelected";


//        //public SelectorOptionsDrawer()
//        //{
//        //    UtilityAIDrawers.processSelectionEvent += ProcessSelection;
//        //    UtilityAIDrawers.processRightClickEvent += ProcessElementRightClick;
//        //}


//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            SerializedProperty qualifiers = property.FindPropertyRelative(qualifiersFieldName);
//            return DrawReorderableList(qualifiers).GetHeight() + Styles.lineHeightSpace;
//        }


//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            property.serializedObject.Update();


//            SerializedProperty qualifiers = property.FindPropertyRelative(qualifiersFieldName);
//            GUIContent headerName = new GUIContent(property.FindPropertyRelative(nameIDFieldName).stringValue);

//            clickBounds = position;
//            position.x += 5;
//            position.y += 8;
//            position.width -= 20;
//            Rect header_rect = new Rect(position.x, position.y, position.width, headerHeight);
//            Rect list_rect = new Rect(position.x, position.y + headerHeight, position.width, position.height - headerHeight);


//            if (Event.current.type == EventType.MouseDown && 
//                Event.current.button == 0 && 
//                position.Contains(Event.current.mousePosition))
//            {
//                selectedElement = -1;
//            }


//            //  Draw Header
//            DrawContainerHeader(header_rect, headerName);
//            //  Draw List
//            list = DrawReorderableList(qualifiers);
//            list.DoList(list_rect);


//            //  Process right clicking on header.
//            ProcessContainerContext(header_rect, qualifiers);


//            property.serializedObject.ApplyModifiedProperties();
//            EditorGUI.EndProperty();
//        }


//        private void DrawContainerHeader(Rect position, GUIContent content)
//        {
//            GUIStyle selectorHeaderStyle;
//            selectorHeaderStyle = new GUIStyle(EditorStyles.toolbar)    //new GUIStyle(EditorStyles.toolbar);  "ShurikenModuleTitle"
//            {
//                fontStyle = FontStyle.Bold,
//                alignment = TextAnchor.MiddleCenter,
//                fontSize = 12,
//                fixedHeight = headerHeight
//            }; 

//            EditorGUI.LabelField(position, content, selectorHeaderStyle);
//        }


//        ///*
//        public ReorderableList DrawReorderableList(SerializedProperty list_property)
//        {
//            float element_buffer = 2;

//            if(list == null)
//            {
//                list = new ReorderableList(list_property.serializedObject, list_property, true, false, false, false);


//                list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//                {
//                    float offset = 20;
//                    rect.xMin -= offset;
//                    rect.xMax += 5;
//                    rect.yMin -= 1;
//                    rect.yMax += 1;
//                    rect.x += 1;
//                    rect.width -= 1;


//                    SerializedProperty listElement = list.serializedProperty.GetArrayElementAtIndex(index);
//                    SerializedProperty listChildElement = listElement.FindPropertyRelative(actionFieldName);

//                    Rect listElementRect = new Rect(rect.x, rect.y, rect.width, Styles.lineHeightSpace + element_buffer);
//                    Rect listChildElementRect = new Rect(rect.x, rect.y + Styles.lineHeightSpace + element_buffer, rect.width, Styles.lineHeightSpace + element_buffer);


//                    //DrawReorderableListElement(rect, listElement, listChildElement);
//                    DrawReorderableListElement(rect, new SerializedProperty[] { listElement, listChildElement });
//                    //  Make sure to apply changes to serialized object.
//                    list_property.serializedObject.ApplyModifiedProperties();
//                };


//                list.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//                {
//                    //SerializedProperty list_element = list.serializedProperty.GetArrayElementAtIndex(index);
//                };


//                list.onSelectCallback = (ReorderableList l) =>
//                {
//                    list_property.serializedObject.ApplyModifiedProperties();
//                };



//                list.elementHeight = (EditorGUIUtility.singleLineHeight + 10 + element_buffer) * 2;
//                list.showDefaultBackground = true;
//                list.headerHeight = 0;
//            }


//            //list.DoList(position);
//            return list;
//        }


//        private void DrawReorderableListElement(Rect position, SerializedProperty[] elements)
//        {
//            float element_buffer = 2;
//            Rect rect = new Rect(position.x, position.y, position.width, Styles.lineHeightSpace + element_buffer);

//            for (int i = 0; i < elements.Length; i++)
//            {
//                float height = i * Styles.lineHeightSpace + element_buffer;
//                rect.y += height;
//                bool isChild = i == elements.Length - 1;
//                DrawListToggleElement(rect, elements[i], isChild);
//                ProcessElementRightClick(rect, elements[i]);
//            }
//        }


//        private void DrawListToggleElement(Rect position, SerializedProperty element, bool isChild = false)
//        {
//            GUIStyle defaultNodeStyle;
//            GUIStyle selectedNodeStyle;
//            GUIStyle style;

//            defaultNodeStyle = new GUIStyle
//            {
//                fontStyle = FontStyle.Bold,
//                alignment = TextAnchor.MiddleLeft,
//                contentOffset = isChild ? childContentOffset : contentOffset
//            };
//            selectedNodeStyle = new GUIStyle("TL SelectionButton PreDropGlow")
//            {
//                fontStyle = FontStyle.Bold,
//                alignment = TextAnchor.MiddleLeft,
//                contentOffset = isChild ? childContentOffset : contentOffset
//            };



//            if (isChild)
//            {
//                Rect icon_rect = new Rect(position.x + childContentOffset.x - (childContentOffset.x - contentOffset.x),
//                                          position.y + -childContentOffset.y + 2,
//                                          position.width,
//                                          position.height);

//                GUI.Label(icon_rect, new GUIContent(Styles.childIcon));

//                icon_rect.xMin -= 14f;
//                icon_rect.xMax = icon_rect.xMin + 14f;
//                //GUI.DrawTexture(icon_rect, Styles.treeLineTexture);
//            }

//            position.height -= 4;

//            SerializedProperty elementName = element.FindPropertyRelative("nameID");
//            SerializedProperty elementIsSelected = element.FindPropertyRelative("isSelected");


//            style = elementIsSelected.boolValue == true ? selectedNodeStyle : defaultNodeStyle;

//            int controlID = GUIUtility.GetControlID(FocusType.Passive);

//            string cleanName = elementName.stringValue.Replace(typeof(UtilityAIClient).Namespace + ".", "");
//            GUIContent header_name = new GUIContent(cleanName);
//            GUIContent _nameAndControlID = new GUIContent(string.Format(" {0} : {1}", cleanName, controlID));

//            GUI.Box(position, _nameAndControlID, style);
//            //elementIsSelected.boolValue = GUI.Toggle(position, elementIsSelected.boolValue, _nameAndControlID, style);

//            ProcessSelection(position, elementIsSelected, controlID);
//        }
//        //*/





//        //  This processes the selection of the Qualifier or action.
//        private void ProcessSelection(Rect clickArea, SerializedProperty isSelected, int id)
//        {
//            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
//            {
//                //  If clicking on an already selected element, deselect it.
//                if (clickArea.Contains(Event.current.mousePosition) && isSelected.boolValue == true)
//                {
//                    GUI.changed = true;
//                    isSelected.boolValue = false;
//                    //return true;
//                }
//                //  If clicking on an element that is not selected.
//                else if (clickArea.Contains(Event.current.mousePosition))
//                {
//                    GUI.changed = true;
//                    isSelected.boolValue = true;
//                    selectedElement = id;
//                    //return true;
//                }
//                //  If clicking only within the reorderable list, but not within the element control.
//                else if (clickBounds.Contains(Event.current.mousePosition) && clickArea.Contains(Event.current.mousePosition) == false)
//                {
//                    GUI.changed = true;
//                    isSelected.boolValue = false;
//                    //return true;
//                }
//            }
//            //return false;
//        }


//        //  These Events are for modifing the Qualifier or action via RightClick
//        private void ProcessElementRightClick(Rect clickArea, SerializedProperty property)
//        {
//            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
//            {
//                if (clickArea.Contains(Event.current.mousePosition))
//                {
//                    ProcessElementContextMenu(property);
//                    Event.current.Use();
//                }
//            }

//            //return false;
//        }


//        //These Events are for modifying the Selector via RightClick.
//        private bool ProcessContainerContext(Rect clickArea, SerializedProperty property)
//        {
//            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && clickArea.Contains(Event.current.mousePosition))
//            {
//                ProcessContainerContextMenu(property);
//                Event.current.Use();
//                return true;
//            }
//            return false;
//        }



//        private void ProcessElementContextMenu(SerializedProperty property)
//        {
//            GenericMenu genericMenu = new GenericMenu();
//            genericMenu.AddItem(new GUIContent("Change Type"), false, () => OnClickChangeNode(property));
//            genericMenu.AddItem(new GUIContent("Delete"), false, () => OnClickRemoveNode(property));
//            genericMenu.ShowAsContext();
//        }


//        private void ProcessContainerContextMenu(SerializedProperty property)
//        {
//            GenericMenu genericMenu = new GenericMenu();
//            genericMenu.AddItem(new GUIContent("Add Qualifier"), false, () => OnClickAddNode(property));
//            genericMenu.ShowAsContext();
//        }


//        //  Adds a Qualifier to the Selector.
//        private void OnClickAddNode(SerializedProperty property)
//        {
//            Type type = typeof(QualifierBase);
//            UtilityAIDrawers.ShowOptionsTypeWindow(type, property, OptionWindowType.Add);
//        }


//        private void OnClickRemoveNode(SerializedProperty property, int index = 0)
//        {
//            Debug.Log("Deleting Qualifier or action:  \nProperty Path:  " + property.propertyPath);
//            property.DeleteCommand();
//            //property.DeleteArrayElementAtIndex(index);
//            property.serializedObject.ApplyModifiedProperties();
//        }


//        private void OnClickChangeNode(SerializedProperty property)
//        {
//            var logicType = property.FindPropertyRelative("logicType");
//            Type type = typeof(QualifierBase);

//            UtilityAIDrawers.ShowOptionsTypeWindow(type, logicType, OptionWindowType.ChangeType);
//        }







//    }


//}
