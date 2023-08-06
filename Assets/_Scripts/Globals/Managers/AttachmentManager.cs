using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Attachment;
[RequireComponent(typeof(DebugableObject))]
public class AttachmentManager : MonoSingleton<AttachmentManager>
{
    //lo debe crear el gunManager?
    //tipo de accesorio => codigo de accesorio => accesorio
    Dictionary<AttachmentType, Dictionary<int,Attachment>> _attachmentsInventory = new Dictionary<AttachmentType, Dictionary<int, Attachment>>();
    [SerializeField] View_Attachment _canvasAttachments;
    [SerializeField] KeyCode _inventoryKey;
    [SerializeField] UI_AttachmentInventory _canvasInventory;

    #region  InteractWithAttachments
    public LayerMask attachmentLayer => _attachmentLayer;
    [SerializeField] LayerMask _attachmentLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] KeyCode Equip = KeyCode.F, Save = KeyCode.G;
    #endregion

   
    public int InventoryAmount => _attachmentsInventory.SelectMany(x => x.Value).Count();
    GunHandler _gunHandler;
  
    DebugableObject _debug;

   public Action InventoryState { get; private set; }

    public IEnumerable<T> GetAllSavedAttachments<T>() where T : Attachment
    {

        return _attachmentsInventory
            .SelectMany(x => x.Value)        
            .Select(x => x.Value)
            .OfType<T>();                   
    }

    protected override void SingletonAwake()
    {
        _gunHandler = GetComponent<GunHandler>();
        _debug = GetComponent<DebugableObject>(); _debug.AddGizmoAction(DrawRaycast);

        _canvasAttachments = Instantiate(_canvasAttachments); _canvasInventory = Instantiate(_canvasInventory);

        InventoryState = OpenInventory;
    }

    private void Update() => AttachmentOnSight();


    void AttachmentOnSight()
    {
        Transform tr = Camera.main.transform;
     
        if (Physics.SphereCast(tr.position, 1f ,tr.forward, out RaycastHit hit, raycastDistance, attachmentLayer))
        {
            _canvasAttachments.gameObject.SetActive(true);
           
            if (hit.transform.TryGetComponent(out Attachment x))
            {
                _canvasAttachments.NewAttachment(x);
                
                if (Input.GetKey(Equip)) _gunHandler.ActualGun.attachmentHandler.AddAttachment(x);

                else if (Input.GetKey(Save)) Inventory_SaveAttachment(x);
            }
            

        }
        else       
            _canvasAttachments.gameObject.SetActive(false);       
    }

    #region InventoryView
    private void LateUpdate()
    {
        if (Input.GetKeyDown(_inventoryKey)) InventoryState?.Invoke();
    }
  
    void OpenInventory()
    {
        //IEnumerable<Attachment> finalResult = _attachmentsInventory.Aggregate(FList.Create<Attachment>(), (x, y) =>
        //{
        //    var result = y.Value.Aggregate(FList.Create<Attachment>(), (x, y) => x + FList.Create(y.Value));
        //    return x + result;
        //});

        _canvasInventory.EnterInventory( _gunHandler.ActualGun);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InventoryState = CloseInventory;
    }

    void CloseInventory()
    {
        _canvasInventory.ExitInventory();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InventoryState = OpenInventory;
    }
    #endregion

    #region Inventory Managment
    public int Inventory_SaveAttachment(Attachment value)
    {

        _debug.Log("Saving Attachment" + value.name);
        int key = value.GetHashCode();
        if (!_attachmentsInventory.ContainsKey(value.MyType))
            _attachmentsInventory.Add(value.MyType, new Dictionary<int, Attachment>());

        if (_attachmentsInventory[value.MyType].ContainsKey(key)) return key;

        if (value.isAttached)
        {
            value.owner.attachmentHandler.RemoveAttachment(value.MyType);
          
        }
        value.Dettach();


        if (!_attachmentsInventory[value.MyType].ContainsKey(key))       
            _attachmentsInventory[value.MyType].Add(key, value);
        
       
        value.gameObject.SetActive(false);

        _debug.Log(" Attachment"+ value.name + " Saved!");

        if (InventoryState != OpenInventory)
        {
            _debug.Log($"Intento Crear un boton para{value}");
            _canvasInventory.AddButton(value);
        }

        _debug.Log($"hay un total de {InventoryAmount} accesorios guardados" );
        return key;

    }

    //void DebugAmountAndType()
    //{
    //    string debug="";
    //    foreach (var item in _attachmentsInventory.Keys)
    //    {
    //        _attachmentsInventory[]
    //    }
    //}

    public bool Inventory_GetAttachment(out Attachment x, Tuple<AttachmentType, int> keys)
    {
        x = default;
        
        if (_attachmentsInventory.TryGetValue(keys.Item1,out var a))        
            if (a.TryGetValue(keys.Item2,out var attachment))
            {
                x = attachment;
                x.gameObject.SetActive(true);
                _attachmentsInventory[keys.Item1].Remove(keys.Item2); return true;
            }

        return false;    
    }

    public Attachment RemoveFromInventory(Attachment x)
    {
        if (_attachmentsInventory.TryGetValue(x.MyType,out var y))              
            if (_attachmentsInventory[x.MyType].ContainsKey(x.GetHashCode()))
            {
                _debug.Log($"Se removio el accesiorio {x} DEL INVENTARIO");
                _attachmentsInventory[x.MyType].Remove(x.GetHashCode());
            }
               
      
        return x;
    }
    #endregion



   
    void DrawRaycast()
    {
        Transform tr = Camera.main.transform;
        Gizmos.color = Color.blue;
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, attachmentLayer))
            Gizmos.DrawLine(tr.position, hit.point);
        else
            Gizmos.DrawLine(tr.position, tr.position + tr.forward * raycastDistance);

    }


}
