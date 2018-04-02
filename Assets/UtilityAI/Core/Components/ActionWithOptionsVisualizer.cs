namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    public class ActionWithOptionsVisualizer: MonoBehaviour
    {
        
        private UtilityAIComponent utilityAIComponent;
        private IContextProvider contextProvider;
        private IContext context;

        [Range(0,1)]
        public float gizmoAlpha = 1f;

        public ActionWithOptions<Vector3> action;


        void Awake()
        {
            utilityAIComponent = gameObject.GetComponent<UtilityAIComponent>();
            contextProvider = utilityAIComponent.contextProvider;
            context = utilityAIComponent.context;

        }

        void OnEnable()
        {
            //if (contextProvider == null){
            //    contextProvider = gameObject.GetComponent(typeof(IContextProvider)) as IContextProvider;
            //    ContextProvider _contextProvider = contextProvider as ContextProvider;
            //    _contextProvider.context = new AIContext(GetComponent<Bang.NpcController>());
            //    context = _contextProvider.GetContext();
            //}
        }





        /// <summary>
        /// Unity Method
        /// </summary>
        private void OnGUI()
        {
            var c = context as AIContext;
            var cam = Camera.main;

            if (c != null)
            {
                if (Application.isEditor)
                {
                    if (Camera.current == cam || Camera.current == SceneView.lastActiveSceneView.camera)
                    {
                        if (action != null){
                            DrawGUI(action.scoredOptions);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Unity Method
        /// </summary>
        private void OnDrawGizmos()
        {
            var c = context as AIContext;
            var cam = Camera.main;

            if (c != null)
            {
                if (Application.isEditor)
                {
                    if (Camera.current == cam || Camera.current == SceneView.lastActiveSceneView.camera)
                    {
                        if (action != null)
                        {
                            DrawGizmos(action.scoredOptions);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// From Apex
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void DrawGUI(List<ScoredOption<Vector3>> data)
        {
            var cam = Camera.main;

            if (cam == null)
                return;

            foreach (var scoredOption in data)
            {
                var score = scoredOption.score;

                var p = cam.WorldToScreenPoint(scoredOption.option);
                p.y = Screen.height - p.y;


                if (score < 0f)
                {
                    GUI.color = Color.red;
                }
                else if (score == 0f)
                {
                    GUI.color = Color.black;
                }
                else
                {
                    GUI.color = Color.green;
                }


                var content = new GUIContent(score.ToString("F0"));
                var size = new GUIStyle(GUI.skin.label).CalcSize(content);

                GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
            }
        }


        /// <summary>
        /// From Apex
        /// </summary>
        /// <param name="data">Data.</param>
        protected virtual void DrawGizmos(List<ScoredOption<Vector3>> data)
        {
            float maxScore = 0f;
            float minScore = Mathf.Infinity;

            foreach (var scoredOption in data)
            {
                var value = scoredOption.score;
                if (value > maxScore)
                {
                    maxScore = value;
                }

                if (value < minScore)
                {
                    minScore = value;
                }
            }

            var diffScore = maxScore - minScore;

            foreach (var scoredOption in data)
            {
                var pos = scoredOption.option;
                var score = scoredOption.score;

                var normScore = score - minScore;

                Gizmos.color = GetColor(normScore, diffScore, gizmoAlpha);
                Gizmos.DrawSphere(pos, 0.25f);
            }
        }


        /// <summary>
        /// From Apex
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="score">Score.</param>
        /// <param name="maxScore">Max score.</param>
        private static Color GetColor(float score, float maxScore, float alpha = 1f)
        {
            if (maxScore <= 0)
            {
                return Color.green;
            }

            if (score == maxScore)
            {
                return Color.cyan;
            }

            var quotient = score / maxScore;

            return new Color((1 - quotient), quotient, 0, alpha);
        }









        ///// <summary>
        ///// My version
        ///// </summary>
        ///// <param name="data">Data.</param>
        //protected virtual void DrawGizmos(List<Vector3> data)
        //{
        //    foreach (Vector3 scoredOption in data)
        //    {
        //        var pos = scoredOption;
        //        Gizmos.DrawSphere(pos, 0.25f);
        //    }
        //}

        ///// <summary>
        ///// My version
        ///// </summary>
        ///// <param name="data">Data.</param>
        //protected virtual void DrawGUI(List<Vector3> data)
        //{
        //    var cam = Camera.main;

        //    if (cam == null)
        //        return;

        //    foreach (Vector3 scoredOption in data)
        //    {
        //        var p = cam.WorldToScreenPoint(scoredOption);
        //        p.y = Screen.height - p.y;

        //        GUI.color = Color.green;

        //        var content = new GUIContent(scoredOption.ToString());
        //        var size = new GUIStyle(GUI.skin.label).CalcSize(content);

        //        GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
        //    }
        //}

    }
}