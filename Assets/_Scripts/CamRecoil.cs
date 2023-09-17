using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatsHandler;
[RequireComponent(typeof(PausableObject))]
public class CamRecoil : MonoBehaviour
{
    Vector3 currentRotation,targetRotation;

    public float VerticalRecoil;
    public float HorizontalRecoil;

    public float snapines, returnSpeed,cameraSmooth=0.1f;

  
    public GunHandler _gunHandler;

    StatsHandler _gunstats => _gunHandler.ActualGun.stats;
    private void Awake()
    {
        var x = GetComponent<PausableObject>();
        x.onPause += () => enabled = false;
        x.onResume += () => enabled = true;
        enabled = true;
    }

    private void Start()
    {
        //_gunHandler.ActualGun.onShoot += Recoil;
        
      
    }

    // Update is called once per frame
    void Update()
    {
       
            //la rotacion que quiero tener, siempre intenta volver a el medio
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero,returnSpeed*Time.deltaTime);
        //mi rotacion actual,intenta ir a mi rotacion target
        currentRotation = Vector3.Slerp(currentRotation,targetRotation,snapines*Time.deltaTime);
       
        Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.Euler(currentRotation), cameraSmooth * Time.deltaTime);
        //seteo la rotacion actual
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        //se llama en onShoot, aumenta la rotacion(tendria que ser las stats del arma y no esto O la stats del arma + estos valores)
        
        float recoilY = VerticalRecoil * (1-_gunstats.GetStat(StatNames.VerticalRecoil)/100);

        recoilY = -recoilY;

        float recoilx_minus = HorizontalRecoil * (1-_gunstats.GetStat(StatNames.HorizontalRecoil) / 100);
      
        float recoilX = Random.Range(-recoilx_minus, recoilx_minus);
       

        targetRotation += new Vector3(recoilY , recoilX, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 aux = new Vector3(-VerticalRecoil, Random.Range(-HorizontalRecoil, HorizontalRecoil), 0);
        Gizmos.DrawLine(transform.position, transform.position + aux +transform.forward);
    }
}
