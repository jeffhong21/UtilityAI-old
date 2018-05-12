namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Collections.Generic;


    public class TaskNetworkVisualizerComponent : ActionWithOptionsVisualizerComponent<ActionWithOptions<Vector3>, Vector3>
    {
        [Range(0, 1)]
        public float gizmoAlpha = 1f;


        protected override List<Vector3> GetOptions(AIContext context){
            return context.tacticalPositions;
        }


        protected override void DrawGUI(List<ScoredOption<Vector3>> data)
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

        protected override void DrawGizmos(List<ScoredOption<Vector3>> data)
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


    }
}
