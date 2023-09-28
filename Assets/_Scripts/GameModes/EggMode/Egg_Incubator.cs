using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

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
    List<EggEscapeModel> _eggs = new();

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        _interactComponent = GetComponent<InteractableComponent>();
        _interactComponent.InteractConditions.Add(() => _eggs.Any());
        _interactComponent.OnInteract.AddListener(IncubateEgg);

        _interactComponent.onFocus.AddListener(FocusText);

        _interactComponent.onUnFocus.AddListener(() => IncubatorText.gameObject.SetActive(false));

        _interactComponent.onTryingToInteract.AddListener(UpdateSlider);
        _interactComponent.onTryingToInteract.AddListener(IncubatorText.TurnSliderTextOn);

        _interactComponent.OnInteractAbort.AddListener(UpdateSlider);
        _interactComponent.OnInteractAbort.AddListener(IncubatorText.TurnSliderTextOff);

        _interactComponent.OnInteractFail.AddListener(() =>
        {
            StopAllCoroutines(); StartCoroutine(WarningSign());
        });


    }

    void FocusText()
    {
        IncubatorText.gameObject.SetActive(true);
        //string text = EggsKidnapedNearby ? can_InteractText : canT_InteractText;
        //IncubatorText.SetText(text);
        IncubatorText.SetFontColor(Color.white);
    }

    IEnumerator WarningSign()
    {
        IncubatorText.SetFontColor(Color.red);
        IncubatorText.SetText("YOU HAVE NO EGGS D:<");
        yield return new WaitForSeconds(3);
        FocusText();
    }


    public void UpdateSlider()
    {
        IncubatorText.SetSliderValue(_interactComponent.NormalizedProgress);
    }

    void IncubateEgg()
    {
        if (!_eggs.Any()) return;


        var x = Instantiate(_eggs[0].view, pointInsideIncubator.position, Quaternion.identity);
        x.transform.parent = pointInsideIncubator;

        var reference = _eggs[0];

        _eggs.RemoveAt(0);
        IncubatorText.gameObject.SetActive(false);
        _interactComponent.NoMoreInteraction();
        Destroy(reference.gameObject);
    }


    #region Triggers
    protected void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent(out EggEscapeModel egg))
        {
            if (!_eggs.Contains(egg))
            {
                _debug.Log("Egg Add");
                _eggs.Add(egg);
            }

            //string text = EggsKidnapedNearby ? can_InteractText : canT_InteractText;
            //IncubatorText.SetText(text);
        }
    }


    protected void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out EggEscapeModel egg)) return;
        if (_eggs.Contains(egg))
            _eggs.Remove(egg);

        //string text = EggsKidnapedNearby ? can_InteractText : canT_InteractText;
        //IncubatorText.SetText(text);


    }
    #endregion
}
