using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;
using TMPro;
using FacundoColomboMethods;

[RequireComponent(typeof(InteractableComponent))]
public class VendorTable : MonoBehaviour
{
    InteractableComponent InteractableComponent;

    [SerializeField] Transform desiredCamPos;

    public UnityEvent OnEnterInventory, OnCloseInventory;

    public event Action OnUpdate = delegate { };

    public GameObject TableContent;
    public bool InsideInventory = false;

    public TextMeshProUGUI VendorText;

    Camera _tempCamera;
    IEnumerable<Camera> _camerasToReactivate;
    private void Awake()
    {
  
        InteractableComponent = GetComponent<InteractableComponent>();
        //sete que la condicion para que se pueda interactuar sea que NO esta en el inventario
        InteractableComponent.SetFocusCondition(() => !InsideInventory);
        InteractableComponent.OnInteract.AddListener(OpenInventory);

        if (VendorText == null) return;
        OnEnterInventory.AddListener(() => VendorText.gameObject.SetActive(false));
        OnCloseInventory.AddListener(() => VendorText.gameObject.SetActive(true));
        VendorText.gameObject.SetActive(true);

    }



 //abre el inventario, lerpea la camara y escucha los inputs para salir
    async void OpenInventory()
    {
        InsideInventory = true;
        ScreenManager.PauseGame();
        await LerpNewCamera(desiredCamPos);

       //sumo el listen inputs para saber cuando salir del inventario
        OnUpdate += ListenInputs;
        TableContent.SetActive(true);
        OnEnterInventory.Invoke();
    }

    //esto solo se llama si el inventario esta abierto
    void ListenInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    async void CloseInventory()
    {
        InsideInventory = false;
        ScreenManager.ResumeGame();
        ColomboMethods.UnlockMouse();
        OnUpdate -= ListenInputs;
        OnCloseInventory.Invoke();
        TableContent.SetActive(false);
        await ReturnToMainCamera();

    }
    /// <summary>
    /// hace un lerpeo entre la posicion del jugador y la posicion deseada para ver la mesa
    /// </summary>
    /// <param name="DesiredCamPos"></param>
    /// <returns></returns>
    public async Task LerpNewCamera(Transform DesiredCamPos)
    {
        //creo una nueva camara para esto y desactivo las otras 
        _tempCamera = Instantiate(Camera.main, Camera.main.transform.position, Camera.main.transform.rotation);
        _camerasToReactivate = _tempCamera.DeactivateAllCamerasExcept();

        //seteo las variables que deben lerpear
        Vector3 InitialPos = _tempCamera.transform.position;
        Quaternion InitialRot = _tempCamera.transform.rotation;

        float t = Time.deltaTime;
        //lerpeo
        while (t < 1)
        {

            _tempCamera.transform.position = Vector3.Lerp(InitialPos, DesiredCamPos.position, t);
            _tempCamera.transform.rotation = Quaternion.Lerp(InitialRot, DesiredCamPos.rotation, t);
            await Task.Yield();//esto es igual a yield return
            t += Time.deltaTime;
        }
        //si se paso de 1, lo seteo como que ya llego
        _tempCamera.transform.position = DesiredCamPos.position;
        _tempCamera.transform.rotation = DesiredCamPos.rotation;
    }


    //re activa las camaras activada y destruye la temporal
    async Task ReturnToMainCamera()
    {
        
        foreach (Camera item in _camerasToReactivate)      
            item.gameObject.SetActive(true);

        Destroy(_tempCamera);
        //aca hacia una transicion de nuevo hacia donde estaba el jugador, decidi descartarlo
        //return;
        //Vector3 actualPos = Camera.main.transform.position;
        //Vector3 actualRot = Camera.main.transform.localEulerAngles;

        //float t = Time.deltaTime;
        //while (t < 1)
        //{
        //    Camera.main.transform.position = Vector3.Lerp(actualPos, Vector3.zero, t);
        //    Camera.main.transform.localEulerAngles = Vector3.Lerp(actualRot, Vector3.zero, t);
        //    await Task.Yield();
        //    t += Time.deltaTime;
        //}
        //Camera.main.transform.position = Vector3.zero;
        //Camera.main.transform.eulerAngles = Vector3.zero;
    }
}
