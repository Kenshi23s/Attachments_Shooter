using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using FacundoColomboMethods;
using URandom = UnityEngine.Random;

public class UI_AttachmentInventory : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] UI_Attachment_Button template;
    [SerializeField] GameObject SavedPanel, EquippedPanel;
    [SerializeField] Camera viewGunCamPrefab;
    Camera viewGunCamInstance;
    [SerializeField] Transform pointTogo;

    List<UI_Attachment_Button> buttons= new List<UI_Attachment_Button>();

    event Action u;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TestInventory();
        }

        u?.Invoke();
    }

    void TestInventory()
    {
        ScreenManager.PauseGame();
        int testNumber = URandom.Range(1, 50);

        List<Attachment> testAttachments = new List<Attachment>();
        for (int i = 0; i < testNumber; i++)
        {

            Sight a = new Sight();
            a.TESTATTACH(URandom.Range(0, 100) % 2 == 0);
            a.TESTNAME = ColomboMethods.GenerateName(6);
            testAttachments.Add(a);
        }
        SetCamera();
        SetInventoryUI(testAttachments);
    }

    Camera[] enabledbeforeUI;
    void SetCamera()
    {
        Transform cam = Camera.main.transform;
        viewGunCamInstance = Instantiate(viewGunCamPrefab, cam.position, cam.rotation);

        enabledbeforeUI = Camera.allCameras.Where(x => x != viewGunCamInstance).ToArray();

        foreach (var item in enabledbeforeUI) item.gameObject.SetActive(false);




        u += MoveCamera;
    }
    void MoveCamera()
    {
        viewGunCamInstance.transform.position = Vector3.Slerp(viewGunCamInstance.transform.position, pointTogo.transform.position, Time.deltaTime * 3);
        viewGunCamInstance.transform.rotation = Quaternion.Slerp(viewGunCamInstance.transform.rotation, pointTogo.rotation, Time.deltaTime * 4);
        
    }
    
    public void SetInventoryUI(IEnumerable<Attachment> Attachments)
    {
        canvas.enabled = true;
        buttons = Attachments.Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {

            Transform parent = y.isAttached ? EquippedPanel.transform : SavedPanel.transform;
            var fillTemplate = Instantiate(template, parent);
            fillTemplate.AssignAttachment(y);

            return x;
        }).ToList();


    }

    void DestroyButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
    }

   
}
