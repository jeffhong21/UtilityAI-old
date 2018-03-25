//namespace UtilityAI
//{
//    using UnityEngine;
//    using UnityEditor;
//    using System;
//    using System.Collections.Generic;


//    public class PositionScoreVisualizerComponent: MonoBehaviour
//    {
        
//        private UtilityAIComponent utilityAIComponent;
//        private IContextProvider contextProvider;
//        private IContext context;


//        private List<UtilityAIClient> aiClient = new List<UtilityAIClient>();
//        private ActionWithOptions<Vector3> action;


//        private void Start()
//        {
//            contextProvider = gameObject.GetComponent(typeof(IContextProvider)) as IContextProvider;
//            utilityAIComponent = gameObject.GetComponent<UtilityAIComponent>();
//            context = contextProvider.GetContext();
//        }


//        public void Initialize(List<UtilityAIClient> clients)
//        {
//            aiClient = clients;

//            if (aiClient != null)
//            {
//                for (int i = 0; i < aiClient.Count; i++)
//                {
//                    var client = aiClient[i];
//                    var map = client.aiClientMap.ReturnActionList();

//                    if (map.Count > 0)
//                    {
//                        action = map[0] as ActionWithOptions<Vector3>;
//                        Debug.Log(action);
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogWarning("No UtilityAIClient");
//            }
//        }





//        /// <summary>
//        /// Unity Method
//        /// </summary>
//        private void OnGUI()
//        {
//            var c = context as AIContext;
//            var cam = Camera.main;

//            if (c != null)
//            {
//                if (Application.isEditor)
//                {
//                    if (Camera.current == cam || Camera.current == SceneView.lastActiveSceneView.camera)
//                    {
//                        //DrawGUI(c.sampledPositions); // ExampleMoveAction

//                        if (action != null)
//                        {
//                            DrawGUI(action.scoredOptions);
//                        }

//                    }
//                }
//            }
//        }


//        /// <summary>
//        /// Unity Method
//        /// </summary>
//        private void OnDrawGizmos()
//        {
//            var c = context as AIContext;
//            var cam = Camera.main;

//            if (c != null)
//            {
//                if (Application.isEditor)
//                {
//                    if (Camera.current == cam || Camera.current == SceneView.lastActiveSceneView.camera)
//                    {
//                        //DrawGizmos(c.sampledPositions);

//                        if (action != null)
//                        {
//                            DrawGizmos(action.scoredOptions);
//                        }
//                    }
//                }
//            }
//        }



//        /// <summary>
//        /// From Apex
//        /// </summary>
//        /// <param name="data">Data.</param>
//        protected virtual void DrawGUI(List<ScoredOption<Vector3>> data)
//        {
//            var cam = Camera.main;

//            if (cam == null)
//                return;

//            foreach (var scoredOption in data)
//            {
//                var score = scoredOption.score;

//                var p = cam.WorldToScreenPoint(scoredOption.option);
//                p.y = Screen.height - p.y;


//                if (score < 0f)
//                {
//                    GUI.color = Color.red;
//                }
//                else if (score == 0f)
//                {
//                    GUI.color = Color.black;
//                }
//                else
//                {
//                    GUI.color = Color.green;
//                }


//                var content = new GUIContent(score.ToString("F0"));
//                var size = new GUIStyle(GUI.skin.label).CalcSize(content);

//                GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
//            }
//        }


//        /// <summary>
//        /// From Apex
//        /// </summary>
//        /// <param name="data">Data.</param>
//        protected virtual void DrawGizmos(List<ScoredOption<Vector3>> data)
//        {
//            float maxScore = 0f;
//            float minScore = Mathf.Infinity;

//            foreach (var scoredOption in data)
//            {
//                var value = scoredOption.score;
//                if (value > maxScore)
//                {
//                    maxScore = value;
//                }

//                if (value < minScore)
//                {
//                    minScore = value;
//                }
//            }

//            var diffScore = maxScore - minScore;

//            foreach (var scoredOption in data)
//            {
//                var pos = scoredOption.option;
//                var score = scoredOption.score;

//                var normScore = score - minScore;

//                Gizmos.color = GetColor(normScore, diffScore);
//                Gizmos.DrawSphere(pos, 0.25f);
//            }
//        }


//        /// <summary>
//        /// From Apex
//        /// </summary>
//        /// <returns>The color.</returns>
//        /// <param name="score">Score.</param>
//        /// <param name="maxScore">Max score.</param>
//        private static Color GetColor(float score, float maxScore)
//        {
//            if (maxScore <= 0)
//            {
//                return Color.green;
//            }

//            if (score == maxScore)
//            {
//                return Color.cyan;
//            }

//            var quotient = score / maxScore;

//            return new Color((1 - quotient), quotient, 0, 0.2f);
//        }









//        ///// <summary>
//        ///// My version
//        ///// </summary>
//        ///// <param name="data">Data.</param>
//        //protected virtual void DrawGizmos(List<Vector3> data)
//        //{
//        //    foreach (Vector3 scoredOption in data)
//        //    {
//        //        var pos = scoredOption;
//        //        Gizmos.DrawSphere(pos, 0.25f);
//        //    }
//        //}

//        ///// <summary>
//        ///// My version
//        ///// </summary>
//        ///// <param name="data">Data.</param>
//        //protected virtual void DrawGUI(List<Vector3> data)
//        //{
//        //    var cam = Camera.main;

//        //    if (cam == null)
//        //        return;

//        //    foreach (Vector3 scoredOption in data)
//        //    {
//        //        var p = cam.WorldToScreenPoint(scoredOption);
//        //        p.y = Screen.height - p.y;

//        //        GUI.color = Color.green;

//        //        var content = new GUIContent(scoredOption.ToString());
//        //        var size = new GUIStyle(GUI.skin.label).CalcSize(content);

//        //        GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
//        //    }
//        //}

//    }
//}