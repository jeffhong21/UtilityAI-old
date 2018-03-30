using UnityEngine;
using UnityEditor;

using NpcBehavior;



public class UtilityDebug : MonoBehaviour
{
    public NPC_BehaviorAI npc;
    
    Color sightRangeColor = new Color(1, 1, 1, 0.5f);
    Color rangeAttackRangeColor = new Color(1, 1, 1, 0.5f);

    Color viewAngleColor = new Color(1, 1, 1, 0.15f);
    Color targetInViewRangeColor = new Color(1, 0, 0, 0.15f);

    float offset = 0.4f;

    void Start(){
        if(npc == null) npc = GetComponent<NPC_BehaviorAI>();
        offset = npc.head.position.y;
    }




    void OnDrawGizmos()
    {
        //  View Arch
        Handles.color = npc.hasLocationOfInterest ? targetInViewRangeColor : viewAngleColor;
        Handles.DrawSolidArc(npc.transform.position + Vector3.up * offset, Vector3.up, npc.DirFromAngle(npc.transform.rotation.y), npc.viewAngle/2, npc.sightRange);
        Handles.DrawSolidArc(npc.transform.position + Vector3.up * offset, Vector3.up, npc.DirFromAngle(npc.transform.rotation.y), -npc.viewAngle/2, npc.sightRange);

        //  Sight Range
        Handles.color = sightRangeColor;
        Handles.DrawWireArc(npc.transform.position + Vector3.up * offset, Vector3.up, Vector3.forward, 360, npc.sightRange);
        Handles.color = rangeAttackRangeColor;
        Handles.DrawWireArc (npc.transform.position + Vector3.up * offset, Vector3.up, Vector3.forward, 360, npc.rangeAttackRange);



        if(npc.locationOfInterest != Vector3.zero && npc.hasTargetInSight == false){
            Handles.color = Color.yellow;
            Handles.DrawLine(transform.position + Vector3.up * offset, npc.locationOfInterest + Vector3.up * offset);
        }


        if (npc.wanderLocation != Vector3.zero){
            Handles.color = Color.green;
            Handles.DrawLine(npc.transform.position + Vector3.up * offset, npc.wanderLocation + Vector3.up * offset);
        }


        if(npc.pursueTarget != null){
            Handles.color = npc.hasTargetInSight ? Color.red : Color.black;
            Handles.DrawLine(transform.position + Vector3.up * offset, npc.pursueTarget.position + Vector3.up * offset);
        }
    }

}
