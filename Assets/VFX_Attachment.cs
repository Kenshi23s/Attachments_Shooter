using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(VFX_Sign))]
[RequireComponent(typeof(Outline))]
[DisallowMultipleComponent]
public class VFX_Attachment : MonoBehaviour
{
   Attachment owner;
   public Outline Outline {get; private set;}
   public VFX_Sign LaserSign { get; private set; }

    [SerializeField,Header("LaserSign")] float unitsAboveAttachment = 2f;


    public void Initialize()
    {
        owner = GetComponent<Attachment>();
        LaserSign = GetComponent<VFX_Sign>();
        SetOutline(); SetVFXsign();
    }


    private void Start()
    {
        
        LaserSign.NewInitialPos = owner.transform.position + (Vector3.up * (unitsAboveAttachment > 1 ? unitsAboveAttachment : 1));
    }
   

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


    void SetVFXsign()
    {
        
        owner.OnAttach += LaserSign.DeactivateSign;
        owner.OnDettach += LaserSign.ActivateSign;
    }
}
