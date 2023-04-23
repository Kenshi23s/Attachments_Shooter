using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static EggState;

public class Egg_Incubator : InteractableObject
{
    [Header("EggIncubator")]
    [SerializeField]string Can_InteractText;
    [SerializeField]string CanT_InteractText;

    Action cantInsert;

    Action canInsert;


    List<EggEscapeModel> _eggs = new List<EggEscapeModel>();
    protected override void Awake()
    {
        base.Awake();
        //"casteo" de funcion a unity action
        UnityAction action = ()=>
        {
            Debug.Log("Quit");
            Application.Quit(); 
        };

        InteractData._OnInteract.AddListener(action);

        canInsert = () =>
        {
            InteractData._promptText = Can_InteractText;
            InteractData.canInteract = true;
            DataChange();
            Debug.Log("Can Interact");
        };
       

        cantInsert = () =>
        {
            InteractData._promptText = CanT_InteractText;
            InteractData.canInteract = false;
            DataChange();
            Debug.Log("CanT Interact");

        };

        cantInsert.Invoke();
       

    }

    
    void CheckEggs()
    {
        if (_eggs.Count > 0)     
         foreach (var item in _eggs)
         {
             if (item.actualState == States.Kidnapped)
             {
                 canInsert.Invoke();
                 return;
             }
         }
          
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (!_eggs.Contains(egg))
            {
                Debug.Log("egg");
                _eggs.Add(egg);
            }
                 
            if (egg.actualState == States.Kidnapped)
            {

                canInsert?.Invoke();
            }
            else
            {
                Debug.Log(egg.actualState);
                cantInsert?.Invoke();
            }
              
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player_Movement player))
        {
            CheckEggs();
        }
      
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (_eggs.Contains(egg))            
                _eggs.Remove(egg);
          
               
           
        }
    }
}
