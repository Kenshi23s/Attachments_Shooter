using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(DebugableObject))]
public class PlayerDetector : MonoBehaviour
{
    //esta clase podria ser mas generica y detectar cualquier cosa, por ahora solo la nesecito para esto
    //desp hacer mas generica
    DebugableObject _debug;
    IDetector callBackTarget;
    //[SerializeField]float range = 10f;
    [SerializeField] Player_Handler target;


    private void Awake()
    {
        _debug=GetComponent<DebugableObject>();
        callBackTarget = GetComponentInParent<IDetector>();
        if (callBackTarget!=null)
        {
            _debug.Log("Tengo papa");
        }
        SphereCollider col = this.GetComponent<SphereCollider>();
        //col.radius= range;
        col.isTrigger= true;
        this.gameObject.layer = 9;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler item))
        {
            target = item;
            callBackTarget.OnRangeCallBack(target);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player_Handler item))
        {
            target = default;
            callBackTarget.OutOfRangeCallBack(target);
        }
    }
}

public interface IDetector
{
    void OnRangeCallBack(Player_Handler item);

    void OutOfRangeCallBack(Player_Handler item);
   
}
