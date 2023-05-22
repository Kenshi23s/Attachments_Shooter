using UnityEngine;

[DisallowMultipleComponent]
public class BulletHandler : MonoBehaviour
{
    // se encarga de manejar las balas del arma(no la municion, las balas. mas adelante habria q hacer un ammo handler)
    [SerializeField]
    BaseBulltet _bulletPrefab;
    int poolKey;
    void Start()
    {        
        poolKey = Bullet_Manager.instance.CreateBulletPool(_bulletPrefab);
        enabled= false;
    }

    public BaseBulltet GetBullet()
    {
        var aux = Bullet_Manager.instance.GetBullet(poolKey);
        aux.ExitPool();
        return aux;
    } 

}
