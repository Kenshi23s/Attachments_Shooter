using UnityEngine;

public class GunVFX_Handler : MonoBehaviour
{
    [SerializeField]
    ParticleHolder _muzzleFlash;
    int muzzleFlashKey;

   [SerializeField] 
    AudioClip Shot_clip;
    // Start is called before the first frame update
    Gun gun;
    private void Awake()
    {
        gun= GetComponent<Gun>();
        gun.onShoot += ShootFedback;
    }
    void Start()
    {     
        muzzleFlashKey = GameManager.instance.vfxPool.CreateVFXPool(_muzzleFlash);
        enabled= false;
    }

    void ShootFedback()
    {
        ShotParticleFeedback();
        ShotSFX();
    }
    void ShotParticleFeedback()
    {
        ParticleHolder aux = GameManager.instance.vfxPool.GetVFX(muzzleFlashKey);

        Transform shootPos = gun.attachmentHandler.shootPos;

        aux.transform.position = shootPos.position;
        aux.transform.forward = shootPos.forward;
        aux.transform.parent = shootPos;

        aux.OnFinish += () => aux.transform.parent = null;
    }
    void ShotSFX()
    {
        AudioPool.instance.SpawnAudio(Shot_clip,gun.transform.position);
    }

}
