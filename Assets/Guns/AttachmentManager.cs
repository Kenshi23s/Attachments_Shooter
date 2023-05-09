using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static Attachment;
[RequireComponent(typeof(DebugableObject))]
public class AttachmentManager : MonoSingleton<AttachmentManager>
{
    //lo debe crear el gunManager?
    Dictionary<AttachmentType, Dictionary<int,Attachment>> _attachmentsInventory = new Dictionary<AttachmentType, Dictionary<int, Attachment>>();

    public LayerMask attachmentLayer => _attachmentLayer;

    [SerializeField] LayerMask _attachmentLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] KeyCode Equip = KeyCode.F, Save = KeyCode.G;

    Func<GunFather> getGun;
    DebugableObject _debug;

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawRaycast);
     
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
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, attachmentLayer))
        {
            Attachment x = hit.transform.GetComponent<Attachment>();
            GunFather gun = getGun?.Invoke();

            if (Input.GetKey(Equip)) getGun?.Invoke().attachmentHandler.AddAttachment(x);

            else if (Input.GetKey(Save)) Inventory_SaveAttachment(getGun?.Invoke(), x);

           
           
        }
    }


    void DrawRaycast()
    {
        Transform tr = Camera.main.transform;
        Gizmos.color = Color.blue;
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, attachmentLayer))      
            Gizmos.DrawLine(tr.position, hit.point);    
        else
            Gizmos.DrawLine(tr.position, tr.position + tr.forward * raycastDistance);

    }
    #region Inventory
    public int Inventory_SaveAttachment(GunFather gun,Attachment value)
    {
        _debug.Log("Saving Attachment");
        int key = value.GetHashCode();
        if (!_attachmentsInventory.ContainsKey(value.myType))
            _attachmentsInventory.Add(value.myType, new Dictionary<int, Attachment>());

        if (_attachmentsInventory[value.myType].ContainsKey(key)) return key;

        if (value.isAttached)       
           gun.attachmentHandler.RemoveAttachment(value.myType); value.Dettach();


        
        _attachmentsInventory[value.myType].Add(key,value);
        value.gameObject.SetActive(false);

        _debug.Log(" Attachment Saved!");
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
        x.gameObject.SetActive(true);
        _attachmentsInventory[keys.Item1].Remove(keys.Item2); return true;
    }
    #endregion



}
