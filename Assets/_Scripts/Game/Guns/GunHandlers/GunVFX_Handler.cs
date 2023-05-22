using UnityEngine;

public class GunVFX_Handler : MonoBehaviour
{
    [Header("Particle")]
    [SerializeField]
    ParticleHolder _muzzleFlash;
    int muzzleFlashKey;

    [Header("SFX")]
    [SerializeField] AudioClip Shot_clip;
    [SerializeField] AudioClip HitMarker_clip;
    // Start is called before the first frame update
    Gun gun;
    private void Awake()
    {
        gun = GetComponent<Gun>();
        gun.onShoot += ShootFedback;
        gun.onHit += (x) => AudioPool.instance.SpawnAudio(HitMarker_clip, gun.transform.position);
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

        Transform shootPos = gun.attachmentHandler._shootPos;

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
