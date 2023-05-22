using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Attachment;

[DisallowMultipleComponent]
public class AttachmentHandler : MonoBehaviour
{
    //se encarga de organizar los accesorios en el arma
    Gun _gun;

    #region Descripcion Pos
    //si no existe una posicion para el accesorio, es imposible colocarlo
    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun"),
     Tooltip("diccionario usado para asignarle a un accesorio x su posicion. " +
        "si el accesorio de tipo x NO tiene posicion no se podra equipar ")]
    #endregion
    SerializedDictionary<AttachmentType, Transform> _attachmentPos = new SerializedDictionary<AttachmentType, Transform>();

    #region ActiveAttachments
    // mis accesorios activos en este momento
    SerializedDictionary<AttachmentType, Attachment> _activeAttachments = new SerializedDictionary<AttachmentType, Attachment>();
    public SerializedDictionary<AttachmentType, Attachment> activeAttachments => _activeAttachments;
    #endregion

    #region Descripcion _Default
    //AAAA
    [SerializeField,SerializedDictionary("Attachment Type","Attachment Prefab"),
        Tooltip("se define un accesorio default en caso de sacar alguno, no es necesario para algunas armas probablemente")]
    #endregion
    SerializedDictionary<AttachmentType, Attachment> _DefaultAttachMent = new SerializedDictionary<AttachmentType, Attachment>();

    // lo uso para crear eventos para cuando quiero cambiar algun tipo de accesorio
    #region Diccionario Eventos
    public Dictionary<AttachmentType, Action> onAttachmentCgange => _onAttachmentChange;
    private Dictionary<AttachmentType, Action> _onAttachmentChange = new Dictionary<AttachmentType, Action>();
    #endregion


    public Transform _shootPos { get; private set; }
   
    [SerializeField, Tooltip("La posicion default de salida de bala en caso de no tener cañon")]
    Transform _defaultShootPos;

    /// <summary>
    /// este metodo inicializa la clase, requiere q pases un "GunFather"
    /// </summary>
    /// <param name="gun"></param>
    public void Awake()
    {
        _gun = GetComponent<Gun>();

        _shootPos = _defaultShootPos;
        // este metodo lo uso para chequear desde donde tiene q salir mi bala
        Action onMuzzleChange = () =>
        {
            if (activeAttachments[AttachmentType.Muzzle].TryGetComponent<Muzzle>(out var a))
            {
                _shootPos = a.shootPos;
                _gun._debug.Log($"cambio mi shootpos a la de {a.gameObject.name}");
                return;          
            }
            _gun._debug.Log($"no tengo muzzle, vuelvo a mi shootpos default");
            _shootPos = _defaultShootPos;                       
        };
        AddOnChangeEvent(AttachmentType.Muzzle, onMuzzleChange);           
    }

    private void Start() => SetDefaultAttachments();

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
                    if (_onAttachmentChange.TryGetValue(key, out var action)) action?.Invoke();
                    continue;
                }
                //chequeo por las dudas para que no salten errores              
            }                
        }
    }
    //añado cosas al diccionario de eventos(no podes hacer un diccionario de eventos en si, tiene q ser de actions)
    public void AddOnChangeEvent(AttachmentType eventType, Action new_action)
    {
        if (!_onAttachmentChange.ContainsKey(eventType))
        {
            Action OnCallCreate = default;
            _onAttachmentChange.Add(eventType, OnCallCreate);
            AddOnChangeEvent(eventType, new_action);//recursion   
            return;
        }
        _onAttachmentChange[eventType] += new_action;
    }

    public void RemoveOnChangeEvent(AttachmentType eventType, Action action)
    {
        if (_onAttachmentChange.ContainsKey(eventType))        
            _onAttachmentChange[eventType] -= action;          
    }
    /// <summary>
    /// llama a un evento de tipo AttachmentType en caso de q exista
    /// </summary>
    /// <param name="type"></param>
    void CallEvent(AttachmentType type)
    {
        if (_onAttachmentChange.TryGetValue(type, out var Call))
        {
            Call?.Invoke(); _gun._debug.Log("Invoco el evento change de tipo " + type);
        }                    
    }

    /// <summary>
    /// Añade un accesorio x al arma(si los de su tipo tienen una ubicacion en el arma)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddAttachment(Attachment value)
    {
        //si contengo una posicion
        if (value == null) return;
        
        AttachmentType key = value.myType;

        if (!_attachmentPos.ContainsKey(key)) 
        {
            _gun._debug.WarningLog($"NO conecte el attachment de tipo{key} a el arma," +
                $" ya que no tengo una posicion para darle"); return;
        }
        
        //y no tengo un accesorio de ese tipo ya activo
        if (!_activeAttachments.ContainsKey(key))
        {
            //lo añado y agrego sus stats
            _activeAttachments.Add(key, value);               
            _activeAttachments[key].Attach(_gun, _attachmentPos[key]);

            //hago un callback para chequear que cambio
            CallEvent(key);
            _gun._debug.Log($"conecte el attachment de tipo {key} a el arma");
        
        }
        else
        {
            //si ya tenia un accesorio de ese tipo lo remplazo
            ReplaceAttachment(value);
        }
    }

    /// <summary>
    /// remplaza el accesorio que ya hay por el que se me pasa por parametro
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void ReplaceAttachment(Attachment value)
    {
        AttachmentType key= value.myType;
        _gun._debug.Log($"Remplazo el accesorio {_activeAttachments[key].name}, por {value.name}");
        SaveAttachment(_activeAttachments[key]);
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
        x.Dettach(); AttachmentManager.instance.Inventory_SaveAttachment(_gun, x);
    }
}
