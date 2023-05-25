using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(IA_Movement))]
public class E_ExplosiveDog : Enemy
{
    StateMachine<string> _fsm;
    public IA_Movement agent { get; private set; }

    #region Idle
    float alarmRadius;
    #endregion
    #region Pursuit
    float pursuitMaxSpeed;
    float minJumpDistance;
   

    #region OnJump
    float JumpSpeed;
    #endregion


    #endregion
    private void Awake()
    {
        agent= GetComponent<IA_Movement>();
        _fsm = GetComponent<StateMachine<string>>();
    }

    void Start()
    {
        //esto no deberia poder hacerlo
        EnemyManager.instance.AddEnemy(this);
        agent.SetTargets(EnemyManager.instance.activeEnemies
            .Where(x => x.TryGetComponent<IA_Movement>(out var T))
            .Select(x => x.GetComponent<IA_Movement>()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
