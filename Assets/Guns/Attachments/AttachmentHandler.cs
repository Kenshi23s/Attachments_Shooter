using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Attachment;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;
using static UnityEngine.Rendering.DebugUI;

[DisallowMultipleComponent]
public class AttachmentHandler : MonoBehaviour
{

    Gun gun;

    #region Descripcion Pos
    //si no existe una posicion para el accesorio, es imposible colocarlo
    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun"),
     Tooltip("diccionario usado para asignarle a un accesorio x su posicion. " +
        "si el accesorio de tipo x NO tiene posicion no se podra equipar ")]
    #endregion
    SerializedDictionary<AttachmentType, Transform> _attachmentPos = new SerializedDictionary<AttachmentType, Transform>();

    // mis accesorios activos en este momento
    SerializedDictionary<AttachmentType, Attachment> _activeAttachments = new SerializedDictionary<AttachmentType, Attachment>();
    public SerializedDictionary<AttachmentType, Attachment> activeAttachments => _activeAttachments;
    #region Descripcion _Default
    //AAAA
    [SerializeField,SerializedDictionary("Attachment Type","Attachment Prefab"),
        Tooltip("se define un accesorio default en caso de sacar alguno, no es necesario para algunas armas probablemente")]
    #endregion
    SerializedDictionary<AttachmentType, Attachment> _DefaultAttachMent = new SerializedDictionary<AttachmentType, Attachment>();

    public Dictionary<AttachmentType, Action> onAttachmentCgange => _onAttachmentChange;
    private Dictionary<AttachmentType, Action> _onAttachmentChange = new Dictionary<AttachmentType, Action>();

    //diccionario de eventos para cambios
    


    public int magazineAmmoType => _magazineAmmoType;
    [SerializeField, Tooltip("no modificar, solo lectura")]
    int _magazineAmmoType;



    public Transform shootPos => _shootPos;
    Transform _shootPos;
    [SerializeField, Tooltip("La posicion default de salida de bala en caso de no tener cañon")]
    Transform _defaultShootPos;

    /// <summary>
    /// este metodo inicializa la clase, requiere q pases un "GunFather"
    /// </summary>
    /// <param name="gun"></param>
    public void Awake()
    {
        this.gun = GetComponent<Gun>();

        #region OBSOLETE, DELETE LATER
       

        _magazineAmmoType = Bullet_Manager.defaultBulletKey;
        #endregion

      
        Action onMuzzleChange = () =>
        {
            if (activeAttachments[AttachmentType.Muzzle].TryGetComponent<Muzzle>(out var a))
            { _shootPos = a.shootPos; return; }  _shootPos = _defaultShootPos;                       
        };
        _shootPos = _defaultShootPos;

        AddOnChangeEvent(AttachmentType.Muzzle, onMuzzleChange);

        SetDefaultAttachments();       
    }

    /// <summary>
    /// añade los accesorios default, en caso de tener alguno
    /// </summary>
    void SetDefaultAttachments()
    {
        foreach (AttachmentType key in Enum.GetValues(typeof(AttachmentType)))
        {
            //pregunta si tiene un accesorio default de ese tipo y si tengo una posicion para el
            if (_DefaultAttachMent.ContainsKey(key) && _attachmentPos.ContainsKey(key))
            {
                //pregunto si el value no es null
                if (_DefaultAttachMent[key] != null && _attachmentPos[key] != null)
                {
                    //añado
                    AddAttachment(_DefaultAttachMent[key]);
                    continue;
                }
                //chequeo por las dudas para que no salten errores              
            }
            if (_onAttachmentChange.TryGetValue(key,out var action)) action?.Invoke();
                 
        }
    }

    public void AddOnChangeEvent(AttachmentType eventType, Action new_action)
    {
        if (_onAttachmentChange.TryGetValue(eventType, out var OnCall))       
            OnCall += new_action;
        
        else
        {
            Action OnCallCreate = default;
            _onAttachmentChange.Add(eventType, OnCallCreate);
            AddOnChangeEvent(eventType, new_action);//recursion         
        }             
    }

    public void RemoveOnChangeEvent(AttachmentType eventType, Action action)
    {
        if (_onAttachmentChange.TryGetValue(eventType, out var OnCall))        
            OnCall -= action;
    }   

    void CallEvent(AttachmentType type)
    {
        if (_onAttachmentChange.TryGetValue(type, out var Call)) Call?.Invoke();
    }

    /// <summary>
    /// Añade un accesorio al arma, requiere q le pases el tipo(key) y la clase de tipo attachment que deseas agregar
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddAttachment(Attachment value)
    {
        //si contengo una posicion
        AttachmentType key = value.myType;

            if (!_attachmentPos.ContainsKey(key)) 
            {
                gun._debug.WarningLog($"NO conecte el attachment de tipo{key} a el arma," +
                    $" ya que no tengo una posicion para darle");
                return;
            }
        
            //y no tengo un accesorio de ese tipo ya activo
            if (!_activeAttachments.ContainsKey(key))
            {
                //lo añado y agrego sus stats
                _activeAttachments.Add(key, value);               
                _activeAttachments[key].Attach(gun, _attachmentPos[key]);

                //hago un callback para chequear que cambio
                CallEvent(key);
                gun._debug.Log($"conecte el attachment de tipo {key} a el arma");
         
            }
            else
            {
                //si ya tenia un accesorio de ese tipo lo remplazo
                ReplaceAttachment(key, value);
            }
            return;
        
       

    }

    /// <summary>
    /// remplaza el accesorio que ya hay por el que se me pasa por parametro
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        SaveAttachment(value);
        _activeAttachments.Remove(key);
        AddAttachment(value);
    }
    /// <summary>
    /// saca el accesorio del arma, si tengo un accesorio default para esa ranura, lo añado
    /// </summary>
    /// <param name="key"></param>
    public void RemoveAttachment(AttachmentType key)
    {
        if (_activeAttachments.TryGetValue(key,out var toRemove))
        {
            SaveAttachment(toRemove);
            _activeAttachments.Remove(key);
        }     
        if (_DefaultAttachMent.TryGetValue(key,out var _default)) AddAttachment(_default);
   
        _onAttachmentChange[key]?.Invoke();



    }

    void SaveAttachment(Attachment x)
    {
        x.Dettach();
        AttachmentManager.instance.Inventory_SaveAttachment(gun, x);
    }

}
