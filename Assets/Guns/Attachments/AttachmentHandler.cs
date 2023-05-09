using AYellowpaper.SerializedCollections;
using System;
using System.Linq;
using UnityEngine;
using static Attachment;

[DisallowMultipleComponent]
public class AttachmentHandler : MonoBehaviour
{

    GunFather gun;

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

  
    //diccionario de eventos para cambios
    public Action<AttachmentType> OnChange;


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
    public void Initialize(GunFather gun)
    {
        this.gun = gun;

        #region OBSOLETE, DELETE LATER
       

        _magazineAmmoType = Bullet_Manager.defaultBulletKey;
        #endregion

      
        OnChange += (x) =>
        {
            //activeAttachments.Where(x => x.Key == AttachmentType.Muzzle).Where(x => x.Value != null).Any();
            //if (x == AttachmentType.Muzzle && activeAttachments.ContainsKey(x) && activeAttachments[x].TryGetComponent(out Muzzle muzzle))
            if (activeAttachments.Where(x => x.Key == AttachmentType.Muzzle).Any())
            {
                Muzzle m = activeAttachments[x].GetComponent<Muzzle>();
               
                if (m != null) _shootPos = m.shootPos;                            
                else
                {
                    _shootPos = _defaultShootPos;
                    gun._debug.Log("Shoot Pos Default");
                }
                return;
            }
        };

        _shootPos = _defaultShootPos;


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
            OnChange(key);
        }      
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
        if (_attachmentPos.ContainsKey(key))
        {
            //y no tengo un accesorio de ese tipo ya activo
            if (!_activeAttachments.ContainsKey(key))
            {
                //lo añado y agrego sus stats
                _activeAttachments.Add(key, value);               
                _activeAttachments[key].Attach(gun, _attachmentPos[key]);

                //hago un callback para chequear que cambio
                OnChange?.Invoke(key);
                gun._debug.Log($"conecte el attachment de tipo{key} a el arma");
                return;
            }
            else
            {
                //si ya tenia un accesorio de ese tipo lo remplazo
                ReplaceAttachment(key, value);
            }
        }
        gun._debug.WarningLog($"NO conecte el attachment de tipo{key} a el arma, ya que no tengo una posicion para darle");

    }

    /// <summary>
    /// remplaza el accesorio que ya hay por el que se me pasa por parametro
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].Dettach();
        AttachmentManager.instance.Inventory_SaveAttachment(gun, _activeAttachments[key]);
        _activeAttachments.Remove(key);
        AddAttachment(value);
    }
    /// <summary>
    /// saca el accesorio del arma, si tengo un accesorio default para esa ranura, lo añado
    /// </summary>
    /// <param name="key"></param>
    public void RemoveAttachment(AttachmentType key)
    {
        if (!_activeAttachments.ContainsKey(key)) return;

        _activeAttachments[key].Dettach();
        _activeAttachments.Remove(key);

        if (_DefaultAttachMent.ContainsKey(key)) 
        {
            AddAttachment(_DefaultAttachMent[key]);
            return;
        }

        OnChange?.Invoke(key);



    }

}
