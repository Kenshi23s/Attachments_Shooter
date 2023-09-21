using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class testPerpendicular : MonoBehaviour
{
  

    public static Vector3 GetPerpendicular(Vector3 dir, Vector3 wallNormal)
    {

        Vector3 WallPerpendicular = Vector3.Cross(dir.normalized, wallNormal);

        return Vector3.Cross(WallPerpendicular, wallNormal).normalized;
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = transform.forward;

        float separation = 0.1f;

        if (!Physics.Raycast(transform.position, dir, out var hit)) return;


        Vector3 startPos = hit.point + hit.normal * separation;
        Gizmos.DrawLine(transform.position + dir.normalized,hit.point);

        Gizmos.color = Color.red;

        Vector3 WallPerpendicular = Vector3.Cross(dir.normalized, hit.normal).normalized;

        Gizmos.DrawLine(startPos - WallPerpendicular, startPos + WallPerpendicular);


        Gizmos.color = Color.blue;

        Vector3 doubleCross = Vector3.Cross(WallPerpendicular, hit.normal).normalized;
#if UNITY_EDITOR
        DrawDisc(startPos, doubleCross, 1);
#endif


        Gizmos.DrawLine(startPos, startPos + doubleCross);
    }
#if UNITY_EDITOR
    private void DrawDisc(Vector3 origin,Vector3 normal,float radius)
    {
       
        Handles.color = Color.red;
        Handles.DrawWireDisc(origin, normal, radius);
    }
#endif
}





