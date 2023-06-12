using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRecoil : MonoBehaviour
{
    Vector3 currentRotation,targetRotation;

    public float VerticalRecoil;
    public float recoilz;
    public float HorizontalRecoil;

    public float snapines, returnSpeed;

   public GunHandler _gunHandler;

    StatsHandler _gunstats => _gunHandler.actualGun.stats;
    private void Awake()
    {
       
    }

    private void Start()
    {
        _gunHandler.onActualGunShoot += Recoil;
    }

    // Update is called once per frame
    void Update()
    {
        //la rotacion que quiero tener, siempre intenta volver a el medio
        targetRotation=Vector3.Lerp(targetRotation,Vector3.zero,returnSpeed*Time.deltaTime);
        //mi rotacion actual,intenta ir a mi rotacion target
        currentRotation=Vector3.Slerp(currentRotation,targetRotation,snapines*Time.deltaTime);
        //seteo la rotacion actual
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        //se llama en onShoot, aumenta la rotacion(tendria que ser las stats del arma y no esto O la stats del arma + estos valores)
        targetRotation += new Vector3(-VerticalRecoil, Random.Range(-HorizontalRecoil, HorizontalRecoil), 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 aux = new Vector3(-VerticalRecoil, Random.Range(-HorizontalRecoil, HorizontalRecoil), 0);
        Gizmos.DrawLine(transform.position, transform.position + aux +transform.forward);
    }
}
