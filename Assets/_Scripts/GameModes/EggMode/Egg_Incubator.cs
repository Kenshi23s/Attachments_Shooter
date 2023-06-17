using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static EggEscapeModel;

[RequireComponent(typeof(InteractableComponent))]
[RequireComponent(typeof(DebugableObject))]
public class Egg_Incubator : MonoBehaviour
{
    [Header("EggIncubator")]
    [SerializeField] string can_InteractText;
    [SerializeField] string canT_InteractText;
    [SerializeField] TextAndFiller IncubatorText;
    [SerializeField] Transform pointInsideIncubator;
    DebugableObject _debug;
    InteractableComponent interactComponent;
    private void Awake()
    {
        _debug= GetComponent<DebugableObject>();
        interactComponent = GetComponent<InteractableComponent>();
       
        interactComponent.OnInteract.AddListener(IncubateEgg);

        UnityAction focus = () =>
        {
            IncubatorText.gameObject.SetActive(true);
            string text = CheckEggs() ? can_InteractText : canT_InteractText;
            IncubatorText.SetText(text);
        };
        interactComponent.onFocus.AddListener(focus);
       
        interactComponent.onUnFocus.AddListener(() => IncubatorText.gameObject.SetActive(false));

       
    }


    List<EggEscapeModel> _eggs = new List<EggEscapeModel>();
    
    public void UpdateSlider()
    {
        IncubatorText.SetSliderValue(interactComponent.currentInteractTime/interactComponent.interactTimeNeeded);
    }
    void IncubateEgg()
    {
        if (_eggs[0] == null) return;

        ModesManager.instance.actualMode.AddPoints(1);


        var x = Instantiate(_eggs[0].view, pointInsideIncubator.position, Quaternion.identity);
        x.transform.parent = pointInsideIncubator;
 
        Destroy(_eggs[0].gameObject);
    }
    bool CheckEggs()
    {
        return _eggs.Where(x => x.actualState == EggStates.Kidnapped).Any();
    }

    #region Triggers
    protected  void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (!_eggs.Contains(egg))
            {
                _debug.Log("Egg Add");
                _eggs.Add(egg);
            }       

            string text = CheckEggs() ? can_InteractText : canT_InteractText;
            IncubatorText.SetText(text);
            


        }
    }

  
    protected  void OnTriggerExit(Collider other)
    {     
        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (_eggs.Contains(egg))
                _eggs.Remove(egg);

            string text = CheckEggs() ? can_InteractText : canT_InteractText;
            IncubatorText.SetText(text);

        }
    }
    #endregion
}
