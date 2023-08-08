using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Attachment;
using FacundoColomboMethods;

[RequireComponent(typeof(DebugableObject))]
public class AttachmentManager : MonoSingleton<AttachmentManager>
{
    //lo debe crear el gunManager?
    //tipo de accesorio => codigo de accesorio => accesorio

    Dictionary<AttachmentType, Dictionary<int, Attachment>> _attachmentsInventory = new Dictionary<AttachmentType, Dictionary<int, Attachment>>();
    [SerializeField, Header("Canvas")] View_Attachment _canvasAttachments;

    [SerializeField] UI_AttachmentInventory _canvasInventory;
    [SerializeField] Transform _canvasSight;

    [field: SerializeField, Header("Inventory Light")]
    public GameObject InventoryLight { get; private set; }

    public event Action OnInventoryOpen, OnInventoryClose;

    #region  InteractWithAttachments

    [field: SerializeField, Header("InventoryParameters")] public LayerMask AttachmentLayer { get; private set; }
    [SerializeField] float raycastDistance;

    [SerializeField, Header("Inputs")] KeyCode _inventorySwitchInput;
    [SerializeField] KeyCode Equip = KeyCode.F, Save = KeyCode.G;
    #endregion


    public int InventoryAmount => _attachmentsInventory.SelectMany(x => x.Value).Count();
    GunHandler _gunHandler;

    DebugableObject _debug;

    public Action InventoryState { get; private set; }

    Transform _camTransform;

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

        _canvasAttachments = Instantiate(_canvasAttachments, _canvasSight); _canvasInventory = Instantiate(_canvasInventory);

        OnInventoryOpen += ShowMouse; OnInventoryClose += HideMouse;

        InventoryState = OpenInventory;

    }

    private void Start()
    {
        OnInventoryOpen += TurnOnInventoryLights; OnInventoryClose += TurnOffInventoryLights;

        OnInventoryOpen += () => _canvasSight.gameObject.SetActive(false);
        OnInventoryClose += () => _canvasSight.gameObject.SetActive(true);

       _camTransform = Camera.main.transform;
    }


    private void Update() => AttachmentOnSight();


    void AttachmentOnSight()
    {
        //si veo algo 
        Debug.Log(AttachmentLayer.LayerMaskToLayerNumber().ToString());
        if (!Physics.SphereCast(_camTransform.position, 1f, _camTransform.forward, out RaycastHit hit, raycastDistance, AttachmentLayer))
        {
            _canvasAttachments.gameObject.SetActive(false);
            return;
        }

        //si es un attachment
        if (!hit.transform.TryGetComponent(out Attachment x)) return;
        //throw new NotImplementedException("Theres Something that is NOT an attachment in the attachment layer, Execption Name " + hit.transform.name);

        _canvasAttachments.gameObject.SetActive(true); _canvasAttachments.NewAttachment(x); ListenGrabInputs(x);
    }

    void ListenGrabInputs(Attachment x)
    {
        var handler = _gunHandler.ActualGun.attachmentHandler;
        if (Input.GetKey(Equip))
        {
            if (!handler.HasEquippedItemOfType(x) && handler.HasPivotFor(x))
                _gunHandler.ActualGun.attachmentHandler.AddAttachment(x);
            else
                Inventory_SaveAttachment(x);
            return;
        }

        if (Input.GetKey(Save)) Inventory_SaveAttachment(x);
    }

    #region InventoryView
    private void LateUpdate()
    {
        if (Input.GetKeyDown(_inventorySwitchInput)) InventoryState?.Invoke();
    }

    void OpenInventory()
    {
        //IEnumerable<Attachment> finalResult = _attachmentsInventory.Aggregate(FList.Create<Attachment>(), (x, y) =>
        //{
        //    var result = y.Value.Aggregate(FList.Create<Attachment>(), (x, y) => x + FList.Create(y.Value));
        //    return x + result;
        //});

        _canvasInventory.EnterInventory(_gunHandler.ActualGun);

        OnInventoryOpen?.Invoke();
        InventoryState = CloseInventory;
    }

    #region LightInventory

    void TurnOnInventoryLights()
    {
        RenderSettings.sun.gameObject.SetActive(false);
        RenderSettings.ambientIntensity = 0;
        InventoryLight.SetActive(true);
    }

    void TurnOffInventoryLights()
    {
        InventoryLight.SetActive(false);
        RenderSettings.ambientIntensity = 1;
        RenderSettings.sun.gameObject.SetActive(true);

    }
    #endregion

    #region CursorShow
    void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    void CloseInventory()
    {
        _canvasInventory.ExitInventory();
        OnInventoryClose?.Invoke();

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

        _debug.Log(" Attachment" + value.name + " Saved!");

        if (InventoryState != OpenInventory)
        {
            _debug.Log($"Intento Crear un boton para{value}");
            _canvasInventory.AddButton(value);
        }

        _debug.Log($"hay un total de {InventoryAmount} accesorios guardados");
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

        if (_attachmentsInventory.TryGetValue(keys.Item1, out var a))
            if (a.TryGetValue(keys.Item2, out var attachment))
            {
                x = attachment;
                x.gameObject.SetActive(true);
                _attachmentsInventory[keys.Item1].Remove(keys.Item2); return true;
            }

        return false;
    }

    public Attachment RemoveFromInventory(Attachment x)
    {
        if (_attachmentsInventory.TryGetValue(x.MyType, out var y))
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
        if (Physics.Raycast(tr.position, tr.forward, out RaycastHit hit, raycastDistance, AttachmentLayer))
            Gizmos.DrawLine(tr.position, hit.point);
        else
            Gizmos.DrawLine(tr.position, tr.position + tr.forward * raycastDistance);

    }


}
