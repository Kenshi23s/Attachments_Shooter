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
    [SerializeField] UI_Attachment_Button _template;
    [SerializeField] GameObject _savedPanel, _equippedPanel;
    #endregion

    #region Camera
    Camera _cam;

    Transform _inventoryCamParent;
    [SerializeField] float _inventoryFov = 40f;

    Transform _camTransform;
    Transform _ogCamParent;
    float _ogFov;
    #endregion

    List<UI_Attachment_Button> _buttons = new List<UI_Attachment_Button>();

    DebugableObject _debug;
    Gun _displayGun;

    #region Transition Variables
    [SerializeField] AnimationCurve _animationCurve;

    float _transitionTime;
    Vector3 _startPosition;
    Quaternion _startRotation;
    Transform _endPosAndRot;

    float _startFov, _endFov;

    bool _isTransitioning;
    #endregion

    private void Awake()
    {   
        _inventoryCanvas = GetComponent<Canvas>();
        _debug = GetComponent<DebugableObject>();
        _inventoryCanvas.enabled = false;
       
    }

    private void Start()
    {
        _inventoryCamParent = PlayerCameraPivots.instance.ViewFromInventory;
        _cam = Camera.main;
        _ogFov = _cam.fieldOfView;
        _camTransform = _cam.transform;
        _ogCamParent = _camTransform.parent;
    }




    private void LateUpdate()
    {

        if (_isTransitioning)
            MoveCamera();
    }

    public void EnterInventory(IEnumerable<Attachment> AttachmentsSaved, Gun displayGun)
    {       
        _inventoryCanvas.enabled = true;

        this._displayGun = displayGun;
        // por si no tengo un arma equipada actualmente(?) lo dejo por las dudas
        IEnumerable<Attachment> gunAttachments = displayGun != null ? displayGun.attachmentHandler.activeAttachments.Values : default;

        var noDefault = gunAttachments.Where(x => !displayGun.attachmentHandler.IsDefaultAttachment(x));

        var result =  CreateButtons(AttachmentsSaved) + CreateButtons(noDefault);

        _buttons = result.ToList();       

        ScreenManager.PauseGame();

        SetCameraTransition(_inventoryCamParent, _inventoryFov);
    }

    public void ExitInventory()
    {

        _inventoryCanvas.enabled = false;

        DestroyButtons();

        ScreenManager.ResumeGame();
        SetCameraTransition(_ogCamParent, _ogFov);
        //SetCameraTransition(_ogCamParent, _ogFov, () => ScreenManager.ResumeGame());
    }


    public void AddButton(Attachment target)
    {

        if (!_inventoryCanvas.enabled) { _debug.Log("el canvas no esta activado, no creo ningun boton"); return; }
      
        UI_Attachment_Button aux = _buttons.FirstOrDefault(x => x.owner == target);

        string debugmsg = $"el accesorio{target}";
        if (aux != default)
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


    #region Camera Methods

    Action _onFinishTransition;

    void SetCameraTransition(Transform target, float targetFov, Action onFinishTransition = null)
    {
        _isTransitioning = true;

        _camTransform.parent = target;

        _startPosition = _camTransform.position;
        _startRotation = _camTransform.rotation;
        _startFov = _cam.fieldOfView;

        _endPosAndRot = target;
        _endFov = targetFov;

        _transitionTime = 0;

        _onFinishTransition = onFinishTransition;
    }

    void MoveCamera()
    {
        _transitionTime += Time.deltaTime;
        float t = _animationCurve.Evaluate(_transitionTime);

        _camTransform.position = Vector3.Lerp(_startPosition, _endPosAndRot.position, t);
        _camTransform.rotation = Quaternion.Lerp(_startRotation, _endPosAndRot.rotation, t);
        _cam.fieldOfView = Mathf.Lerp(_startFov, _endFov, t);

        if (t >= 1)
        {
            _isTransitioning = false;
            _onFinishTransition?.Invoke();
        }
    }
    #endregion

    #region Buttons Methods
    public FList<UI_Attachment_Button> CreateButtons(IEnumerable<Attachment> attachments)
    {
        return attachments.Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {

            Transform parent = y.isAttached ? _equippedPanel.transform : _savedPanel.transform;
            var fillTemplate = Instantiate(_template, parent);

            return x + fillTemplate.AssignAttachment(y, _displayGun, ChangePosition);
        }).ToFList();
    }

    void ChangePosition(UI_Attachment_Button x)
    {
         
        x.transform.SetParent(x.owner.isAttached ? _equippedPanel.transform : _savedPanel.transform); 
        x.AssignAttachment(x.owner, _displayGun, ChangePosition);

    }

    void DestroyButtons()
    {
        for (int i = 0; i < _buttons.Count; i++) { _debug.Log("Destruyo el boton numero"+ i); Destroy(_buttons[i].gameObject); } 
    }
    #endregion

}
