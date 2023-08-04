using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Hace que un GameObject con collider actue como un botton. Se necesita un EventSystem en escena, y un PhysicsRaycaster acoplado a la camara.
/// </summary>
public class ButtonGameObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public Material normalMaterial;
    public Material highlightedMaterial;
    public Material pressedMaterial;
    public Material selectedMaterial;
    public Material disabledMaterial;

    private MeshRenderer meshRenderer;

    private bool isDisabled = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normalMaterial;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = highlightedMaterial;
            Debug.Log("POINTER ENTER");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = normalMaterial;
            Debug.Log("POINTER EXIT");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            // Call your custom onClick function here
            Debug.Log("POINTER CLICK");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = pressedMaterial;
            Debug.Log("POINTER DOWN");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = highlightedMaterial;
            Debug.Log("POINTER UP");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = selectedMaterial;
            Debug.Log("ON SELECT");
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!isDisabled)
        {
            meshRenderer.material = normalMaterial;
            Debug.Log("ON DESELECT");
        }
    }

    // Use this function to disable/enable the button
    public void SetButtonState(bool disabled)
    {
        isDisabled = disabled;
        meshRenderer.material = disabled ? disabledMaterial : normalMaterial;
    }
}
