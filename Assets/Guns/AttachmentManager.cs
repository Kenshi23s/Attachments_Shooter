using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attachment;
[RequireComponent(typeof(DebugableObject))]
public class AttachmentManager : MonoSingleton<AttachmentManager>
{
    //lo debe crear el gunManager?
    Dictionary<AttachmentType, Dictionary<int,Attachment>> _attachmentsInventory;

    public LayerMask AttachmentLayer => _attachmentLayer;

    [SerializeField] LayerMask _attachmentLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] KeyCode Equip = KeyCode.F, Save = KeyCode.G;

    Func<GunFather> getGun;

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        GetComponent<DebugableObject>().AddGizmoAction(DrawRaycast);
    }
    private void Start()
    {
        getGun = () => GunManager.instance.actualGun;
    }

    private void Update()
    {
        AttachmentOnSight();
    }
    void AttachmentOnSight()
    {
        Transform tr = Camera.main.transform;
        //desde                     //hacia                         //distancia      //layer
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, AttachmentLayer))
        {
            Attachment x = hit.transform.GetComponent<Attachment>();
            GunFather gun = getGun?.Invoke();

            if (Input.GetKeyDown(Equip)) getGun?.Invoke().attachmentHandler.AddAttachment(x);

            else if (Input.GetKeyDown(Save)) Inventory_SaveAttachment(getGun?.Invoke(), x);
           
           
        }
    }


    void DrawRaycast()
    {
        Transform tr = Camera.main.transform;
        Gizmos.color = Color.blue;
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, AttachmentLayer))      
            Gizmos.DrawLine(tr.position, hit.point);    
        else
            Gizmos.DrawLine(tr.position, tr.position + tr.forward * raycastDistance);

    }
    #region Inventory
    public int Inventory_SaveAttachment(GunFather gun,Attachment value)
    {
        int key = value.GetHashCode();
        if (_attachmentsInventory[value.myType].ContainsKey(key)) return key;

        gun.attachmentHandler.RemoveAttachment(value.myType);
        _attachmentsInventory[value.myType].Add(key,value);

        return key;

    }

    //key diccionario 1(enum Attachment) 
    //key diccionario 2(HashCode)

    public bool Inventory_GetAttachment(out Attachment x, Tuple<AttachmentType, int> keys)
    {
        x = default;

        if (!_attachmentsInventory.ContainsKey(keys.Item1)) return false;
        if (!_attachmentsInventory[keys.Item1].ContainsKey(keys.Item2)) return false;

        x = _attachmentsInventory[keys.Item1][keys.Item2];

        _attachmentsInventory[keys.Item1].Remove(keys.Item2); return true;
    }
    #endregion



}
