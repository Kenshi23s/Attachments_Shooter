using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Diagnostics;


[RequireComponent(typeof(DebugableObject))]
public class UI_AttachmentInventory : MonoBehaviour
{
    #region Canvas
    Canvas _inventoryCanvas;
    [SerializeField] UI_Attachment_Button template;
    [SerializeField] GameObject savedPanel, equippedPanel;
    #endregion

    #region InventoryCamera Camera
    [SerializeField] Camera viewGunCamPrefab;
    Camera viewGunCamInstance;

   Transform _pointTogo;

    [SerializeField,Range(1,10)] float _camRotationSpeed, _cam_MoveSpeed;

    event Action _camUpdate;
    #endregion

    List<UI_Attachment_Button> _buttons = new List<UI_Attachment_Button>();

    //las camaras que habia activadas antes de poner la UI
    Camera[] enabledbeforeUI;

    DebugableObject _debug;
    Gun displayGun; 

    private void Awake()
    {   
        _inventoryCanvas = GetComponent<Canvas>();
        _debug = GetComponent<DebugableObject>();
        _inventoryCanvas.enabled= false;
       
    }

    private void Start()
    {
        _pointTogo = PlayerCameraPivots.instance.ViewFromInventory;
    }




    private void LateUpdate()
    {
        _camUpdate?.Invoke();
    }

    public void SetInventoryUI(IEnumerable<Attachment> AttachmentsSaved,Gun displayGun)
    {       
        _inventoryCanvas.enabled = true;

        this.displayGun = displayGun;
        // por si no tengo un arma equipada actualmente(?) lo dejo por las dudas
        IEnumerable<Attachment> gunAttachments = displayGun != null ? displayGun.attachmentHandler.activeAttachments.Values : default;

        var noDefault = gunAttachments.Where(x => displayGun.attachmentHandler.IsDefaultAttachment(x));

        var result =  CreateButtons(AttachmentsSaved) + CreateButtons(noDefault);

        _buttons = result.ToList();       

        ScreenManager.PauseGame();
        SetCamera();     
    }

    public void AddButton(Attachment target)
    {

        if (!_inventoryCanvas.enabled) { _debug.Log("el canvas no esta activado, no creo ningun boton"); return; }
      
        UI_Attachment_Button aux = _buttons.FirstOrDefault(x => x.owner == target);

        string debugmsg= $"el accesorio{target}";
        if (aux!=default)
        {
            debugmsg += $" tenia un boton asignado!";
            ChangePosition(aux);
        }         
        else
        {
            _buttons = (_buttons + CreateButtons(FList.Create(target))).ToList();
            debugmsg += $"NO tenia un boton asignado en el inventario, " +
                $"asi que le cree uno";
        }
        _debug.Log(debugmsg);
        print(debugmsg);
    }

    public void DeactivateInventory()
    {

        foreach (var item in enabledbeforeUI) item.gameObject.SetActive(true);

        enabledbeforeUI = default;
        _inventoryCanvas.enabled = false;

        _camUpdate = null;

        Destroy(viewGunCamInstance.gameObject);
        DestroyButtons();

        ScreenManager.ResumeGame();
    }

    #region Camera Methods
    void SetCamera()
    {
        Transform cam = Camera.main.transform;
        viewGunCamInstance = Instantiate(viewGunCamPrefab, cam.position, cam.rotation);

        enabledbeforeUI = Camera.allCameras.Where(x => x != viewGunCamInstance).ToArray();

        foreach (var item in enabledbeforeUI) item.gameObject.SetActive(false);

        _camUpdate += MoveCamera;
    }

    void MoveCamera()
    {
        //cree este variable para que sea un poco mas legible
        Transform viewGun = viewGunCamInstance.transform; 

        viewGun.position = Vector3.Slerp(viewGun.position, _pointTogo.transform.position, _cam_MoveSpeed * Time.deltaTime);
        viewGun.rotation = Quaternion.Slerp(viewGun.rotation, _pointTogo.rotation, _camRotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Buttons Methods
    public FList<UI_Attachment_Button> CreateButtons(IEnumerable<Attachment> attachments)
    {
        return attachments.Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {

            Transform parent = y.isAttached ? equippedPanel.transform : savedPanel.transform;
            var fillTemplate = Instantiate(template, parent);

            return x + fillTemplate.AssignAttachment(y, displayGun, ChangePosition);
        }).ToFList();
    }

    void ChangePosition(UI_Attachment_Button x)
    {
         
        x.transform.SetParent(x.owner.isAttached ? equippedPanel.transform : savedPanel.transform); 
        x.AssignAttachment(x.owner, displayGun, ChangePosition);

    }

    void DestroyButtons()
    {
        for (int i = 0; i < _buttons.Count; i++) { _debug.Log("Destruyo el boton numero"+ i); Destroy(_buttons[i].gameObject); } 
    }
    #endregion

}
