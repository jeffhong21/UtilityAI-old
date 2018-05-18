namespace UtilityAI
{
    using UnityEngine;
    using UnityEditor;



    public class PerceptionVisualizer : MonoBehaviour
    {
        AIPerceptionComponent entity;

        Color sightRangeColor = new Color(1, 1, 1, 0.5f);
        Color rangeAttackRangeColor = new Color(1, 1, 1, 0.5f);

        Color viewAngleColor = new Color(1, 1, 1, 0.15f);
        Color targetInViewRangeColor = new Color(1, 0, 0, 0.15f);



        void OnEnable()
        {
            if (entity == null) entity = GetComponent<AIPerceptionComponent>();

        }


        void OnDrawGizmos()
        {
            if (entity != null) {
                //  View Arch
                //Handles.color = entity.hasLocationOfInterest ? targetInViewRangeColor : viewAngleColor;
                Handles.color = viewAngleColor;
                Handles.DrawSolidArc(entity.transform.position + Vector3.up * entity.yOffset, Vector3.up, entity.DirFromAngle(entity.transform.rotation.y), entity.fieldOfView / 2, entity.sightRange);
                Handles.DrawSolidArc(entity.transform.position + Vector3.up * entity.yOffset, Vector3.up, entity.DirFromAngle(entity.transform.rotation.y), -entity.fieldOfView / 2, entity.sightRange);

                //  Sight Range
                Handles.color = sightRangeColor;
                Handles.DrawWireArc(entity.transform.position + Vector3.up * entity.yOffset, Vector3.up, Vector3.forward, 360, entity.sightRange);
            }


        }

    }
}

