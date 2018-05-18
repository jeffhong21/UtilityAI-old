namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.AI;

    public class NavigationVisualizer : ContextGizmoGUIVisualizerComponent
    {
        [SerializeField]
        float locationRadius = 0.5f;
        [SerializeField]
        Color routeColor = new Color32(180, 255, 0, 255);
        [SerializeField]
        Color waypointColor = new Color32(0, 180, 255, 128);
        [SerializeField]
        float yOffset = 0.1f;


        float drawRange = 2;        //  How far away from the destination does entity need to be before Gizmo is drawn.

        protected override void DrawGUI(IAIContext context)
        {
            
        }


        protected override void DrawGizmos(IAIContext context)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            AIContext aiContext = context as AIContext;
            var entity = aiContext.entity;
            //NavMeshPath path = entity.aiSteer.path;
            NavMeshPath path = entity.aiSteer.agent.path;

            float distSqr = (aiContext.destination - entity.transform.position).sqrMagnitude;


            if (aiContext.destination != Vector3.zero || distSqr > drawRange * drawRange)
            {
                Vector3 height = Vector3.up * yOffset;

                if(path != null)
                {
                    Vector3[] corners = path.corners;
                    for (int c = 0; c < corners.Length - 1; c++)
                    {
                        //distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
                        Handles.color = routeColor;
                        Handles.DrawLine(corners[c] + height, corners[c + 1] + height);
                    }
                }


                Handles.color = waypointColor;
                Handles.DrawSolidDisc(aiContext.destination + height, height, locationRadius);
                //  Stopping distance
                Handles.color = new Color(1, 1, 1, 0.25f);;
                Handles.DrawSolidDisc(aiContext.destination + height, height, entity.aiSteer.arrivalDistance);
            }





        }
    }
}

