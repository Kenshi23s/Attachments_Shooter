using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using FacundoColomboMethods;
[RequireComponent(typeof(DebugableObject))]
public class UI_AttachmentInventory : MonoBehaviour
{
    #region Canvas
    Canvas _inventoryCanvas;
    [SerializeField] UI_Attachment_Button template;
    [SerializeField] GameObject SavedPanel, EquippedPanel;
    #endregion

    #region InventoryCamera Camera
    [SerializeField] Camera viewGunCamPrefab;
    Camera viewGunCamInstance;

    [SerializeField] Transform pointTogo;

    [SerializeField,Range(1,10)] float _camRotationSpeed, _cam_MoveSpeed;

    event Action _camUpdate;
    #endregion

    List<UI_Attachment_Button> buttons = new List<UI_Attachment_Button>();
 
    Action _inventoryState;

    //las camaras que habia activadas antes de poner la UI
    Camera[] enabledbeforeUI;

    private void Awake()
    {
        
        _inventoryCanvas = GetComponent<Canvas>();
        pointTogo = PlayerCameraPivots.instance.ViewFromInventory;
    }
 
    //private void LateUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.V)) _inventoryState?.Invoke();    

    //    _camUpdate?.Invoke();
    //}

    //void TestInventory()
    //{      
    //    int testNumber = URandom.Range(1, 50);

    //    List<Attachment> testAttachments = new List<Attachment>();
    //    for (int i = 0; i < testNumber; i++)
    //    {
    //        Sight a = new Sight();
    //        a.TESTATTACH(URandom.Range(0, 100) % 2 == 0);
    //        a.TESTNAME = ColomboMethods.GenerateName(6);
    //        testAttachments.Add(a);
    //    }       
    //}

 
    
    
    public void SetInventoryUI(IEnumerable<Attachment> AttachmentsSaved,Gun DisplayGun)
    {       
        _inventoryCanvas.enabled = true;

        // por si no tengo un arma equipada actualmente(?) lo dejo por las dudas
        IEnumerable<Attachment> gunAttachments = DisplayGun != null ? DisplayGun.attachmentHandler.activeAttachments.Values : default;

        var result = FList.Create<UI_Attachment_Button>() + CreateButtons(AttachmentsSaved) + CreateButtons(gunAttachments);

        buttons = result.ToList();       

        ScreenManager.PauseGame();
        SetCamera();     
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

        viewGun.position = Vector3.Slerp(viewGun.position, pointTogo.transform.position, _cam_MoveSpeed * Time.deltaTime);
        viewGun.rotation = Quaternion.Slerp(viewGun.rotation, pointTogo.rotation, _camRotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Buttons Methods
    public IEnumerable<UI_Attachment_Button> CreateButtons(IEnumerable<Attachment> attachments)
    {
        return attachments.Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {

            Transform parent = y.isAttached ? EquippedPanel.transform : SavedPanel.transform;
            var fillTemplate = Instantiate(template, parent);


            return x + fillTemplate.AssignAttachment(y);
        });
    }

    void DestroyButtons()
    {
        for (int i = 0; i < buttons.Count; i++) { Debug.Log("Destruyo el boton numero"+ i); Destroy(buttons[i].gameObject); } 
    }
    #endregion

}
