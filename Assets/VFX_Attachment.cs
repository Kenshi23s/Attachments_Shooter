using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rendering = UnityEngine.Rendering;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(VFX_Sign))]
[RequireComponent(typeof(Outline))]
[DisallowMultipleComponent]
public class VFX_Attachment : MonoBehaviour
{
    Attachment owner;
    public Outline Outline { get; private set; }
    public VFX_Sign LaserSign { get; private set; }

    [SerializeField,Header("View")] MeshRenderer _view;
    [SerializeField] float _augmentViewScale;
    public Vector3 originaViewlScale;

    [SerializeField, Header("LaserSign")] float unitsAboveAttachment = 2f;
    [field : SerializeField] public float PickUpCanvasUnitsAbove { get; private set; }

    public void Initialize()
    {
        owner = GetComponent<Attachment>();
        LaserSign = GetComponent<VFX_Sign>();

        _view = GetComponentInChildren<MeshRenderer>();
        originaViewlScale = _view.transform.localScale;
        SetOutline(); SetVFXsign(); SetScaleMethods(); SetShadowCasting();
    }


    private void Start()
    {
        SetInventoryEvents();
        LaserSign.NewInitialPos = owner.transform.position + Vector3.up * unitsAboveAttachment;
    }

    void SetInventoryEvents()
    {
        AttachmentManager.instance.OnInventoryClose += () =>
        {
            if (owner.isAttached)          
                StopCastingShadows();          
            else        
                StartCastingShadows();        
        };

        AttachmentManager.instance.OnInventoryOpen += StartCastingShadows;    
    }
    #region View Methods

    #region Shadows
    void SetShadowCasting()
    {
        owner.OnAttach += StopCastingShadows; 
        owner.OnDettach += StartCastingShadows;
    }

   
    public void StopCastingShadows()
    {
        _view.shadowCastingMode = Rendering.ShadowCastingMode.Off;
    }

    public void StartCastingShadows()
    {
        _view.shadowCastingMode = Rendering.ShadowCastingMode.On;
    }
    #endregion

    void SetScaleMethods()
    {
        owner.OnAttach  += UnscaleView;
        owner.OnDettach += ScaleView;
    }
    void ScaleView()
    {
       
        _view.transform.localScale = originaViewlScale * _augmentViewScale;
    }

    void UnscaleView()
    {
        _view.transform.localScale = originaViewlScale;
    }
    #endregion
    #region OutLine
    void SetOutline()
    {
        Outline = GetComponent<Outline>();
        Outline.OutlineColor = Color.white;
        owner.OnAttach += DeactivateOutline;
        owner.OnDettach += ActivateOutline;
    }

    public void ActivateOutline() => Outline.enabled = true;
    public void DeactivateOutline() => Outline.enabled = false;
    #endregion

    #region VFX Sign
    void SetVFXsign()
    {
        owner.OnAttach += LaserSign.DeactivateSign;
        owner.OnDettach += LaserSign.ActivateSign;
    }
    #endregion

    private void OnValidate()
    {
        unitsAboveAttachment = Mathf.Max(unitsAboveAttachment, 1);
        _augmentViewScale = Mathf.Max(_augmentViewScale, 1);
    }
}
