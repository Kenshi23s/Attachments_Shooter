using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using static Attachment;

[System.Serializable]
public class AttachmentManager
{

    GunFather gun;
    

    //si no existe una posicion para el accesorio, es imposible colocarlo
    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun")]
    SerializedDictionary<AttachmentType, Transform> _attachmentPos = new SerializedDictionary<AttachmentType, Transform>();

    //[SerializeField, SerializedDictionary("Type,Class")]
    SerializedDictionary<AttachmentType, Attachment> _activeAttachments = new SerializedDictionary<AttachmentType, Attachment>();
    public SerializedDictionary<AttachmentType, Attachment> activeAttachments => _activeAttachments;

    [SerializeField,SerializedDictionary("Attachment Type","Position In Gun")]
    SerializedDictionary<AttachmentType, Attachment> _DefaultAttachMent = new SerializedDictionary<AttachmentType, Attachment>();

   public void Initialize(GunFather gun)
   {
        this.gun = gun;
        //si se cambia el cargador chequea q tipo de municion usa
        OnChange += (x) =>
        {
            if (x == AttachmentType.Magazine && activeAttachments[x].TryGetComponent(out Magazine magazine))
            {

                if (_magazineAmmoType != default)
                {
                    _magazineAmmoType = magazine.bulletKey;
                }
                else
                {
                    _magazineAmmoType = Bullet_Manager.defaultBulletKey;
                }
            }

        };

        //si se cambia el cargador chequea q tipo de municion usa
        OnChange += (x) =>
        {
            if (x == AttachmentType.Muzzle && activeAttachments[x].TryGetComponent(out Muzzle muzzle))
            {

                if (shootPos != default)
                {
                    _shootPos = muzzle.shootPos;
                }
                return;
            }

            Debug.Log("Shoot Pos Default");
            _shootPos = _defaultShootPos;


        };

        SetDefaultAttachments();

       
    }

    public Action<AttachmentType> OnChange;
    public string magazineAmmoType => _magazineAmmoType;
    [SerializeField,Tooltip("no modificar, solo lectura")]
    string _magazineAmmoType;
    


    public Transform shootPos => _shootPos;
    Transform _shootPos;
    [SerializeField,Tooltip("La posicion default de la mira en caso de no tener cañon")]
    Transform _defaultShootPos;







    void SetDefaultAttachments()
    {
        foreach (AttachmentType key in Enum.GetValues(typeof(AttachmentType)))
        {
            if (_DefaultAttachMent.ContainsKey(key) && _attachmentPos.ContainsKey(key))
            {
                if (_DefaultAttachMent[key] != null && _attachmentPos[key] != null)
                {
                    AddAttachment(key, _DefaultAttachMent[key]);
                    continue;
                }
                OnChange(key);
            }
            
        }
        
        
    }
  
    public void AddAttachment(AttachmentType key, Attachment value)
    {
        if (_attachmentPos.ContainsKey(key))
        {
            if (!_activeAttachments.ContainsKey(key))
            {
                _activeAttachments.Add(key, value);               
                _activeAttachments[key].Attach(gun, gun.transform, _attachmentPos[key].position);
                OnChange?.Invoke(key);
                Debug.Log($"conecte el attachment de tipo{key} a el arma");
                return;
            }
            else
            {
                ReplaceAttachment(key, value);
            }
        }
        Debug.Log($"NO conecte el attachment de tipo{key} a el arma, ya que no tengo una posicion para darle");

    }

    void ReplaceAttachment(AttachmentType key, Attachment value)
    {
        _activeAttachments[key].UnAttach();
        _activeAttachments.Remove(key);
        AddAttachment(key, value);
    }

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
