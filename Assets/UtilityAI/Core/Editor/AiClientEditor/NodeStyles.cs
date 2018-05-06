namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;

    public static class NodeStyles
    {

        public static GUIStyle nodeHeaderStyle;
        public static GUIStyle nodeStyle;
        public static GUIStyle selectedStyle;

        public static GUIStyle nodeHeaderTextStyle;
        public static GUIStyle nodeTextStyle;
        public static GUIStyle nodeSelectedTextStyle;


        public static Color headerColor;
        public static Color nodeColor;


        static NodeStyles()
        {

            nodeHeaderStyle = new GUIStyle("PopupCurveEditorSwatch");
            nodeStyle = new GUIStyle("IN ThumbnailShadow");   // TE NodeBox   IN ThumbnailShadow
            selectedStyle = new GUIStyle("TL SelectionButton PreDropGlow");


            nodeHeaderTextStyle = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
            };

            nodeTextStyle = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                richText = true,
                alignment = TextAnchor.MiddleLeft,
                contentOffset = new Vector2(25, -3)
            };

            nodeSelectedTextStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                richText = true,
                alignment = TextAnchor.MiddleLeft,
            };


            headerColor = Color.grey;
            nodeColor = new Color32(194, 194, 194, 255);
        }
    }
}

