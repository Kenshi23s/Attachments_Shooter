using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GunStats : MonoBehaviour
{
    //quien deberia hacer el cambio de estadistica, el arma o el accesorio?
    //el arma deberia tener referencia de todos sus accesorios o no es de gran importancia?
    //(si, deberia para saber donde posicionarlos)

    [SerializeField] Transform defaultShootPos;
    public Transform _shootPos => shootPos;
    Transform shootPos;

    [Header("GunStats")]
    [SerializeField, Range(1, 100)] float _aimingZoom;
    [SerializeField, Range(1, 100)] float _range;

    [SerializeField, Range(1, 100)] float _handling;
    [SerializeField, Range(1, 100)] float _stability;

    [SerializeField, Range(1, 100)] float _reloadSpeed;

    [SerializeField, Range(1, 100)] float _bulletMagnetism;

    [SerializeField] float _maxMagazineAmmo;
    #region Stat Getters    

    public float aimingZoom => _aimingZoom;
    public float range => _range;

    public float handling => _handling;
    public float stability => _stability;

    public float reloadSpeed => _reloadSpeed;
    public float maxMagazineAmmo => _maxMagazineAmmo;

    #endregion

    #region StatMethods
    private void AddAimZoom(float value) => _aimingZoom += Mathf.Clamp(value, 1, 100);
    private void AddRange(float value) => _range += Mathf.Clamp(value, 1, value);

    private void AddReloadSpeed(float value) => _reloadSpeed += Mathf.Clamp(value, 1, 100);
    private void AddHandling(float value) => _handling += Mathf.Clamp(value, 1, 100);

    private void AddStability(float value) => _stability += Mathf.Clamp(value, 1, 100);
    private void AddMaxMagCapacity(float value) => _stability += Mathf.Clamp(value, 1, 100);
    #endregion

    private void ChangeShootPos(Transform newShootpos)
    {
        if (newShootpos != null)
        {
            shootPos = newShootpos;
        }
        else
        {
            shootPos = defaultShootPos;
        }
    }

}
