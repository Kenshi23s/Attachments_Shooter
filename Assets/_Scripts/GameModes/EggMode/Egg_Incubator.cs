using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static EggEscapeModel;

public class Egg_Incubator : InteractableObject
{
    [Header("EggIncubator")]
    [SerializeField]string can_InteractText;
    [SerializeField]string canT_InteractText;

    Action cantInsert;

    Action canInsert;

   [SerializeField] Transform pointInsideIncubator;

    List<EggEscapeModel> _eggs = new List<EggEscapeModel>();
    protected override void Awake()
    {
        base.Awake();
        //"casteo" de funcion a unity action
        UnityAction action = () =>
        {
            ModesManager.instance.gameMode.AddPoints(1);
          
            Destroy(_eggs[0].gameObject);
            //var x = Instantiate(_eggs[0], pointInsideIncubator.position,Quaternion.identity);

            //x.transform.parent = pointInsideIncubator;

       
         
            //IEnumerable<Component> y = x.GetComponents<Component>().Concat(x.GetComponentsInChildren<Component>());
            //var z = y
            //.Where(x => x.GetType() != typeof(Transform))
            //.Where(x => x.GetType() != typeof(MeshRenderer))
            //.Where(x => x.GetType() != typeof(MeshFilter));
            //foreach (Behaviour item in z) item.enabled = false;   
        };

        InteractData._OnInteract.AddListener(action);

        canInsert = () =>
        {
            InteractData._promptText = can_InteractText;
            InteractData.canInteract = true;
            DataChange();
            _debug.Log("Can Interact");
        };
       

        cantInsert = () =>
        {
            InteractData._promptText = canT_InteractText;
            InteractData.canInteract = false;
            DataChange();
            _debug.Log("CanT Interact");

        };

        cantInsert.Invoke();
       
    }

    
    void CheckEggs()
    {
        if (_eggs.Count > 0)     
         foreach (var item in _eggs)
         {
             if (item.actualState == EggStates.Kidnapped)
             {
                 canInsert.Invoke();
                 return;
             }
         }
          
    }

    #region Triggers
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (!_eggs.Contains(egg))
            {
                _debug.Log("egg");
                _eggs.Add(egg);
            }
                 
            if (egg.actualState == EggStates.Kidnapped)
            {

                canInsert?.Invoke();
            }
            else
            {
                _debug.Log(egg.actualState.ToString());
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
    #endregion
}
