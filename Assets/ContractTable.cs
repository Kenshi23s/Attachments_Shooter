using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(InteractableComponent))]
public class ContractTable : MonoBehaviour
{
    InteractableComponent InteractableComponent;

    [SerializeField] Transform desiredCamPos;

    Transform originalCameraParent;

    public event Action OnUpdate = delegate { };

    public bool InsideInventory = false;

    private void Awake()
    {
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractableComponent.SetFocusCondition(() => !InsideInventory);
        InteractableComponent.OnInteract.AddListener(OpenInventory);
    }

   

    //si abro el inventario creo otra camara en la posicion del jugador?
    //o muevo la camara del jugador hacia el inventario?
    async void OpenInventory()
    {
        InsideInventory = true;
        ScreenManager.PauseGame();
        await LerpCamera(desiredCamPos);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnUpdate+= ListenInputs;
    }

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OnUpdate -= ListenInputs;
        await ReturnCamera();
    }

    public async Task LerpCamera(Transform DesiredCamPos)
    {

        Vector3 InitialPos = Camera.main.transform.position;
        Quaternion InitialRot = Camera.main.transform.rotation;

        //para despues hacerlo hijo de su "pivote original"
        originalCameraParent = Camera.main.transform.parent != null ? Camera.main.transform.parent : null;

        float t = Time.deltaTime;

        while (t < 1)
        {
            
            Camera.main.transform.position = Vector3.Lerp(InitialPos, DesiredCamPos.position,t);
            Camera.main.transform.rotation = Quaternion.Lerp(InitialRot, DesiredCamPos.rotation, t);
            await Task.Yield();
            t += Time.deltaTime;
        }

        Camera.main.transform.position = DesiredCamPos.position;
        Camera.main.transform.rotation = DesiredCamPos.rotation;
    }

   

    async Task ReturnCamera()
    {     
        Camera.main.transform.parent = originalCameraParent != null 
            ? originalCameraParent.transform 
            : null;

           Vector3 actualPos = Camera.main.transform.position;
        Vector3 actualRot = Camera.main.transform.localEulerAngles;
       
        float t = Time.deltaTime;
        while (t < 1)
        {
            Camera.main.transform.position = Vector3.Lerp(actualPos, Vector3.zero, t);
            Camera.main.transform.localEulerAngles = Vector3.Lerp(actualRot, Vector3.zero, t);
            await Task.Yield();
            t += Time.deltaTime;
        }
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.eulerAngles = Vector3.zero;
    }
}
