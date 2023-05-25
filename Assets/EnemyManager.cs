using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    //preguntarle a jocha como era para hacer q no se pueda modificar esta lista
    public List<Enemy> activeEnemies => _activeEnemies;
    List<Enemy> _activeEnemies = new List<Enemy>();
    // Start is called before the first frame update

    protected override void SingletonAwake()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
       
    }

    public Enemy[] GetEnemies()
    {
        return activeEnemies.ToArray();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

   
}
