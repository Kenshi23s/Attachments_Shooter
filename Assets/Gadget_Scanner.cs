using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using FacundoColomboMethods;

public class Gadget_Scanner : Gadget
{
    [field: SerializeField] public float PreferedFieldSize { get; private set; } = 10;

    [field: SerializeField] public float IncreaseSpeed { get; private set; } = 1;

    [field: SerializeField] public float OutlineActiveTime { get; private set; } = 3;

    [field: SerializeField] public float CD_waitingTime { get; private set; } = 5F;

    public bool IsActive { get; private set; } = false;

    //este deberia de tener el material
    [SerializeField] Transform ScanField;

    public override void AwakeContinue()
    {
        ScanField.gameObject.SetActive(false);
    }

    IEnumerator TurnOnRadar(Type[] targets)
    {
        FList<MonoBehaviour> alreadyScanned = FList.Create<MonoBehaviour>(this);
       
        IsActive = true;
        ScanField.gameObject.SetActive(true);
        ScanField.localScale = Vector3.zero;

        while (ScanField.localScale.magnitude < PreferedFieldSize)
        {

            ScanField.localScale += Vector3.one * Time.deltaTime * IncreaseSpeed;

             // esto va a ser pesadisimo, debe haber una manera mas optima.
             // existe. habria que usar las queries de IA 2, pero conviene meterlo en nuestro proyecto?
             // consultar el martes
            var col = ScanField.position.GetItemsOFTypeAround<MonoBehaviour>(ScanField.localScale.magnitude)
                .Where(x => !alreadyScanned.Contains(x))
                .Distinct()
                .ToArray();
            yield return null;

            if (col.Any()) alreadyScanned += col;

            foreach (var type in targets)
            {
                col.Where(x => x.GetType() == type)
                .ToList()
                .ForEach(x => StartCoroutine(SetOutline(x)));
            }
            yield return null;
        }

        yield return new WaitForSeconds(OutlineActiveTime);

        ScanField.localScale = Vector3.zero;
        yield return new WaitForSeconds(CD_waitingTime);
        IsActive = false;
        ScanField.gameObject.SetActive(false);
    }
    IEnumerator SetOutline(MonoBehaviour target)
    {
        Outline outline = default;
        Action OnTimeOut = delegate { };
        if (target.TryGetComponent(out Outline x))
        {
            outline = x;
            OnTimeOut = () => 
            { 
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.enabled = false;
            }; 
        }                   
        else
        {
            outline = target.gameObject.AddComponent<Outline>();
            OnTimeOut = () => { Destroy(outline); };
        }


        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineAll;

        yield return new WaitForSeconds(OutlineActiveTime);
        OnTimeOut();
    }

    public override bool UseGadget(IGadgetOwner x)
    {
        if (IsActive)
            return false;

        StopAllCoroutines();
        StartCoroutine(TurnOnRadar(x.TargetTypes));

        return true;
    }

    
}
