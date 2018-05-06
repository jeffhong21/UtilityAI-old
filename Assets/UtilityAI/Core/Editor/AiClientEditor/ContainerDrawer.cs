namespace UtilityAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;


    [CustomPropertyDrawer(typeof(StageContainer))]
    public class ContainerDrawer : PropertyDrawer
    {
        float elementHeight;
        float nodeWidth = 200;
        float nodeHeight;
        float headerHeight = 20;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //nodeStyle.border = new RectOffset(2, 2, 2, 2);

            // Create a rect that is adjusted to the editor zoom and pixel perfect
            Rect nodeRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
            Rect bodyRect = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, nodeRect.height);  //  - headerHeight
            Rect headerRect = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, headerHeight);

            // Create a headerRect out of the previous rect and draw it, marking the selected node as such by making the header bold
            GUI.Box(headerRect, GUIContent.none, NodeStyles.nodeHeaderStyle);
            GUI.Label(headerRect, headerRect.height.ToString());



            // Begin the body frame around the NodeGUI
            GUI.BeginGroup(bodyRect, NodeStyles.nodeStyle);    // -->  Being Group
            bodyRect.position = Vector2.zero;
            GUILayout.BeginArea(bodyRect);      // -->  Being Area






            // Call NodeGUI
            //GUI.changed = false;
            //if (Event.current.type == EventType.Repaint)
            //nodeGUIHeight = GUILayoutUtility.GetLastRect().max + nodeOffset;

            // End NodeGUI frame
            GUILayout.EndArea();        // -->  End Area
            GUI.EndGroup();         // -->  End Group

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
            //return list.GetHeight();
        }






    }





}