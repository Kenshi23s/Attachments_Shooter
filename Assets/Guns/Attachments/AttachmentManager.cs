using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using static Attachment;

[System.Serializable]
public class AttachmentHandler
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

    public Action<AttachmentType> OnChange;
    public string magazineAmmoType => _magazineAmmoType;
    [SerializeField, Tooltip("no modificar, solo lectura")]
    string _magazineAmmoType;



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
        //si se cambia el cargador chequea q tipo de municion usa
        OnChange += (x) =>
        {
            if (x == AttachmentType.Magazine && activeAttachments.ContainsKey(x) && activeAttachments[x].TryGetComponent(out Magazine magazine))
            {

                if (_magazineAmmoType != null)
                {
                    _magazineAmmoType = magazine.bulletKey;
                }
                else
                {
                    _magazineAmmoType = Bullet_Manager.defaultBulletKey;
                }
            }

        };

        _magazineAmmoType = Bullet_Manager.defaultBulletKey;

        //si se cambia el cargador chequea q tipo de municion usa
        OnChange += (x) =>
        {
            if (x == AttachmentType.Muzzle && activeAttachments.ContainsKey(x) && activeAttachments[x].TryGetComponent(out Muzzle muzzle))
            {

               
                if (muzzle.shootPos != null)
                {
                    _shootPos = muzzle.shootPos;
                }
                else
                {
                    _shootPos = _defaultShootPos;
                    Debug.Log("Shoot Pos Default");
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
                    AddAttachment(key, _DefaultAttachMent[key]);
                    continue;
                }
                //chequeo por las dudas para que no salten errores
                OnChange(key);
            }
            OnChange(key);
        }
        
        
    }
  /// <summary>
  /// Añade un accesorio al arma, requiere q le pases el tipo(key) y la clase de tipo attachment que deseas agregar
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
    public void AddAttachment(AttachmentType key, Attachment value)
    {
        //si contengo una posicion
        if (_attachmentPos.ContainsKey(key))
        {
            //y no tengo un accesorio de ese tipo ya activo
            if (!_activeAttachments.ContainsKey(key))
            {
                //lo añado y agrego sus stats
                _activeAttachments.Add(key, value);               
                _activeAttachments[key].Attach(gun, gun.transform, _attachmentPos[key].position);
                //hago un callback para chequear que cambio
                OnChange?.Invoke(key);
                Debug.Log($"conecte el attachment de tipo{key} a el arma");
                return;
            }
            else
            {
                //si ya tenia un accesorio de ese tipo lo remplazo
                ReplaceAttachment(key, value);
            }
        }
        Debug.Log($"NO conecte el attachment de tipo{key} a el arma, ya que no tengo una posicion para darle");

    }

    /// <summary>
    /// remplaza el accesorio que ya hay por el que se me pasa por parametro
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].UnAttach();
        _activeAttachments.Remove(key);
        AddAttachment(key, value);
    }
    /// <summary>
    /// saca el accesorio del arma, si tengo un accesorio default para esa ranura, lo añado
    /// </summary>
    /// <param name="key"></param>
    public void RemoveAttachment(AttachmentType key)
    {
        if (!_activeAttachments.ContainsKey(key))
            return;     

        _activeAttachments.Remove(key);

        if (_DefaultAttachMent.ContainsKey(key)) 
        {
            AddAttachment(key, _DefaultAttachMent[key]);
            return;
        }

        OnChange?.Invoke(key);



    }

}
