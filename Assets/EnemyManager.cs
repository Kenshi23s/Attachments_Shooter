using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FacundoColomboMethods;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    //preguntarle a jocha como era para hacer q no se pueda modificar esta lista
    public ReadOnlyCollection<Enemy> activeEnemies { get; private set; }
    List<Enemy> _activeEnemies
    {
        get => _activeEnemies;
        set 
        {
            _activeEnemies = value;
            activeEnemies= new ReadOnlyCollection<Enemy>(_activeEnemies);
        }
    }
    // Start is called before the first frame update

    protected override void SingletonAwake()
    {
        activeEnemies = new ReadOnlyCollection<Enemy>(_activeEnemies);
    }

    public void AddEnemy(Enemy x) => _activeEnemies.CheckAndAdd(x);

    public void RemoveEnemy(Enemy x) => _activeEnemies.CheckAndRemove(x);   
}
