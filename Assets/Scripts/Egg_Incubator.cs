using System;
using System.Collections.Generic;
using UnityEngine;
using static EggState;

public class Egg_Incubator : InteractableObject
{
    [Header("EggIncubator")]
    [SerializeField]string Can_InteractText;
    [SerializeField]string CanT_InteractText;

    Action CantInsert;

    List<EggEscapeModel> EggsKidnapped;
    protected override void Awake()
    {
        base.Awake();
        CantInsert = () =>
        {
            InteractData._promptText = CanT_InteractText;
            InteractData.canInteract = false;
            DataChange();
            
        };
        CantInsert.Invoke();
       

    }
   
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (egg.actualState == States.Kidnapped)
            {
                InteractData._promptText = Can_InteractText;
                InteractData.canInteract = true;
                EggsKidnapped.Add(egg);
                DataChange();
            }
            else
               CantInsert.Invoke();
            
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (EggsKidnapped.Contains(egg))
            {
                EggsKidnapped.Remove(egg);
                if (EggsKidnapped.Count == 0)
                    CantInsert.Invoke();
            }           
               

           
            
            
           
        }
    }
}
