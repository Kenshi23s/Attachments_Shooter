using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FacundoColomboMethods;

public class EnemyManager : MonoSingleton<EnemyManager>
{
  
    public ReadOnlyCollection<Enemy> activeEnemies { get; private set; }
    public List<Enemy> _activeEnemies = new List<Enemy>();
    //{
    //    get => _activeEnemies;
    //    set 
    //    {
    //        _activeEnemies = value;
    //        activeEnemies = new ReadOnlyCollection<Enemy>(_activeEnemies);
    //    }
    //}
    // Start is called before the first frame update

    protected override void SingletonAwake()
    {
        activeEnemies = new ReadOnlyCollection<Enemy>(_activeEnemies);
    }

    public void AddEnemy(Enemy x) => _activeEnemies.CheckAndAdd(x);

    public void RemoveEnemy(Enemy x) => _activeEnemies.CheckAndRemove(x);   
}
