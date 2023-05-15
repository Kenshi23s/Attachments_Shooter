using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class PlayerDetector : MonoBehaviour
{
    //esta clase podria ser mas generica y detectar cualquier cosa, por ahora solo la nesecito para esto
    //desp hacer mas generica

    IDetector callBackTarget;
    //[SerializeField]float range = 10f;
    [SerializeField]Player_Movement target;


    private void Awake()
    {
        callBackTarget = GetComponentInParent<IDetector>();
        if (callBackTarget!=null)
        {
            Debug.Log("Tengo papa");
        }
        SphereCollider col = this.GetComponent<SphereCollider>();
        //col.radius= range;
        col.isTrigger= true;
        this.gameObject.layer = 9;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player_Movement item))
        {
            target = item;
            callBackTarget.OnRangeCallBack(target);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player_Movement item))
        {
            target = default;
            callBackTarget.OutOfRangeCallBack(target);
        }
    }
}

public interface IDetector
{
    void OnRangeCallBack(Player_Movement item);

    void OutOfRangeCallBack(Player_Movement item);
   
}
