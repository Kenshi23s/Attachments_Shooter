using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using FacundoColomboMethods;
using static StatsHandler;

[RequireComponent(typeof(DebugableObject))]
public class UI_AttachmentInventory : MonoBehaviour
{
    #region Canvas

    #region Attachments
    Canvas _inventoryCanvas;
    [SerializeField,Header("Attachments")] UI_Attachment_Button _templateAttachments;
    [SerializeField] Transform _accesoriesPanel;
    List<UI_Attachment_Button> _buttons = new List<UI_Attachment_Button>();
    #endregion

    #region Gun Stats 
    [SerializeField,Header("Stats Display")] GunStatDisplay statDisplayTemplate;
    Dictionary<StatsHandler.StatNames, GunStatDisplay> statCollection = new Dictionary<StatsHandler.StatNames, GunStatDisplay>();
    [SerializeField] Transform _statsPanel;
    #endregion

    #endregion

    #region Camera
    Camera _cam;

    Transform _inventoryCamParent;
    [SerializeField,Header("Camera ")] float _inventoryFov = 40f;

    Transform _camTransform;
    Transform _ogCamParent;
    float _ogFov;

    #region Transition Variables
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] float offsetAttachment = 5;
    float _transitionTime;
    Vector3 _startPosition;
    Quaternion _startRotation;
    Transform _endPosAndRot;

    float _startFov, _endFov;

    bool _isTransitioning;
    #endregion
    #endregion

    DebugableObject _debug;
    Gun _displayGun;

    public const int MinimumButtonsInInventory = 5;

    private void Awake()
    {   
        _inventoryCanvas = GetComponent<Canvas>();
        _debug = GetComponent<DebugableObject>();
        _inventoryCanvas.enabled = false;

        foreach (StatNames stat in Enum.GetValues(typeof(StatNames)))
        {
            statCollection.Add(stat, Instantiate(statDisplayTemplate, _statsPanel));
        }
       
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

    #region Stats Methods
    void RefreshStats()
    {
        foreach (var stat in statCollection)
        {
            stat.Value.SetStatDisplay(stat.Key,_displayGun.stats.GetStat(stat.Key));
        }
    }
    #endregion

    #region Attachments Methods
    void SetAttachmentInteractions(Attachment x)
    {
        var button = x.gameObject.CreateComponent<ButtonGameObject>();
        button.ClearAllEvents();
        button.OnSelected.AddListener(() => OpenAttachmentColectionOfType(x));

        button.OnNormal.AddListener(x.VFX.DeactivateOutline);
        button.OnHighlighted.AddListener(x.VFX.ActivateOutline);
       
    }

    public void OpenAttachmentColectionOfType<T>(T attachment) where T : Attachment
    {
        DestroyButtons();
        _debug.Log("abro coleccion de tipo " + attachment.GetType());
        // por si no tengo un arma equipada actualmente(?) lo dejo por las dudas

        if (_displayGun == null) { Debug.LogWarning("Se abrio el intventario pero no se asigno arma");  return; }
      

        Func<Attachment,bool> _isSameType = x => x.GetType() == attachment.GetType();


        var gunAttachments = _displayGun.attachmentHandler.activeAttachments.Values.Where(_isSameType);
        //accesorios del mismo arma que no son default
        var noDefault = gunAttachments.Where(_displayGun.attachmentHandler.IsntDefaultAttachment);
        //accesorios guardados de tipo T
        var AttachmentsSaved = AttachmentManager.instance.GetAllSavedAttachments<T>().Where(_isSameType);

        var emptyButtons = new FList<UI_Attachment_Button>(); 
        int amount = noDefault.Count() + AttachmentsSaved.Count();

        _debug.Log($"hay {AttachmentsSaved.Count()}");
        if (amount < MinimumButtonsInInventory)
        {
            _debug.Log($"La cantidad de accesorios guardados de tipo {attachment.MyType} es de {amount}, creo {MinimumButtonsInInventory - amount} para rellenar el inventario");
            emptyButtons = CreateEmptyButtons(MinimumButtonsInInventory - amount);
        }
        var result = CreateButtons<T>(noDefault) + CreateButtons<T>(AttachmentsSaved) + emptyButtons; 

        _buttons = result.ToList();       
    }

    void RefreshAttachments()
    {
        foreach (Tuple<Vector3, Attachment> attach in _displayGun.attachmentHandler.GetPivotPosAndAttachment())
        {
            //pos attachment                  // pos pivot de attachment type en arma
            //despues lo paso a struct o otro formato, lo nesecitaba en tuple para prototipar mas rapido
                                 //pos Inicial                      //pos final
            Vector3 inverseDir = attach.Item2.transform.position - attach.Item1;
            //StartCoroutine(MoveAttachment(attach.Item2, inverseDir.normalized * offsetAttachment));
            //preguntar a jocha pq no hace lo que quiero esta wea :C


            SetAttachmentInteractions(attach.Item2);
            attach.Item2.gameObject.GetComponent<Collider>().enabled = true;
           
       
        }
    }

    #region Buttons Methods
    public FList<UI_Attachment_Button> CreateButtons<T>(IEnumerable<Attachment> attachments) where T : Attachment
    {
        return attachments.OfType<T>().Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {
            var fillTemplate = Instantiate(_templateAttachments, _accesoriesPanel);
            return x + fillTemplate.AssignAttachment(y, _displayGun);
            //return x + fillTemplate.AssignAttachment(y, _displayGun, ChangePosition);
        });
    }

    public FList<UI_Attachment_Button> CreateEmptyButtons(int amount)
    {
        FList<UI_Attachment_Button> col = new FList<UI_Attachment_Button>();

        for (int i = 0; i < amount; i++) col += Instantiate(_templateAttachments, _accesoriesPanel);

        return col;
    }

    public void AddButton(Attachment target)
    {

        if (!_inventoryCanvas.enabled) { _debug.Log("El canvas no esta activado, no creo ningun boton"); return; }

        UI_Attachment_Button aux = _buttons.FirstOrDefault(x => x.owner == target);

        string debugmsg = $"el accesorio{target}";
        if (aux != default)
        {
            debugmsg += $" tenia un boton asignado!";
            //ChangePosition(aux);
        }
        else
        {
            //_buttons = (_buttons + CreateButtons(FList.Create(target))).ToList();
            debugmsg += $"NO tenia un boton asignado en el inventario, " +
                $"asi que le cree uno";
        }
        _debug.Log(debugmsg);
    }

    void DestroyButtons()
    {
        if (!_buttons.Any()) return;

        _debug.Log($"Destruyo {_buttons.Count} botones numero");
        for (int i = 0; i < _buttons.Count; i++) {; Destroy(_buttons[i].gameObject); }

        _buttons.Clear();
    }
    #endregion

    #endregion

    #region Input Inventory
    public void EnterInventory(Gun displayGun)
    {
        ScreenManager.PauseGame();
        _inventoryCanvas.enabled = true;

        _displayGun = displayGun;

        RefreshStats(); RefreshAttachments();

        SetCameraTransition(_inventoryCamParent, _inventoryFov);
    }

    public void ExitInventory()
    {
        _inventoryCanvas.enabled = false; DestroyButtons();

        var copy = _displayGun.attachmentHandler.activeAttachments.Values.ToArray();
        foreach (var item in copy)
        {
            Destroy(item.GetComponent<ButtonGameObject>());
           
            item.gameObject.GetComponent<Collider>().enabled = false;
            _displayGun.attachmentHandler.AddAttachment(item);
        }
        ScreenManager.ResumeGame();
        SetCameraTransition(_ogCamParent, _ogFov);
        //SetCameraTransition(_ogCamParent, _ogFov, () => ScreenManager.ResumeGame());
    }
    #endregion

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

    IEnumerator MoveAttachment(Attachment x, Vector3 targtPos)
    {
        float time = 0;
        if (Vector3.Distance(x.transform.position, targtPos) < 1)
        {
            time = 1;
        }

        while (time < 1 && ScreenManager.IsPaused())
        {
            x.transform.position = Vector3.Lerp(x.transform.position, targtPos, time);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        x.transform.position = Vector3.Lerp(x.transform.position, targtPos, 1);


    }
}
