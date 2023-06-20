using System.Collections.Generic;
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
    InteractableComponent _interactComponent;
    List<EggEscapeModel> _eggs = new List<EggEscapeModel>();
    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _interactComponent = GetComponent<InteractableComponent>();
        _interactComponent.interactConditions.Add(()=>_eggs.Any());
        _interactComponent.OnInteract.AddListener(IncubateEgg);

        UnityAction focus = () =>
        {
            IncubatorText.gameObject.SetActive(true);
            string text = CheckEggs() ? can_InteractText : canT_InteractText;
            IncubatorText.SetText(text);
        };
        _interactComponent.onFocus.AddListener(focus);
       
        _interactComponent.onUnFocus.AddListener(() => IncubatorText.gameObject.SetActive(false));

        _interactComponent.onTryingToInteract.AddListener(UpdateSlider);
        _interactComponent.onTryingToInteract.AddListener(IncubatorText.TurnSliderTextOn);

        _interactComponent.OnInteractAbort.AddListener(UpdateSlider);
        _interactComponent.OnInteractAbort.AddListener(IncubatorText.TurnSliderTextOff);
    }


 
    
    public void UpdateSlider()
    {      
        IncubatorText.SetSliderValue(_interactComponent.currentInteractTime / _interactComponent.interactTimeNeeded);      
    }
    void IncubateEgg()
    {
        if (!_eggs.Any()) return;

        ModesManager.instance.actualMode.AddPoints(1);


        var x = Instantiate(_eggs[0].view, pointInsideIncubator.position, Quaternion.identity);
        x.transform.parent = pointInsideIncubator;

        var z = _eggs[0];
        
        _eggs.RemoveAt(0);
        IncubatorText.gameObject.SetActive(false);
        _interactComponent.NoMoreInteraction();
        Destroy(z.gameObject);
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
