using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EggEscapeModel;

public class LookToObjective : MonoBehaviour
{
    [SerializeField]EggGameChaseMode gamemode;
    [SerializeField] float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (gamemode&&gamemode.isActiveAndEnabled)
        {
            StartCoroutine(LookTowards());
        }
       
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(LookTowards());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator LookTowards()
    {
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }
      

        while (true)
        {
            float time = Time.deltaTime;
            
            yield return null;
            time += Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward,GetObjectiveDir(), time * rotationSpeed);
            
        }
    }
  

    Vector3 GetObjectiveDir()
    {
        Vector3 FinalPos=Vector3.zero;
        if (gamemode.eggsEscaping.Any(x => x.actualState == EggStates.Kidnapped))
        {
            FinalPos = gamemode.incubators
           .Minimum(x => Vector3.Distance(x.transform.position,Player_Movement.position)).transform.position;
        }
        else
        {
            FinalPos = gamemode.eggsEscaping
           .Minimum(x => Vector3.Distance(x.transform.position, Player_Movement.position)).transform.position;
        }
        return (FinalPos - Player_Movement.position).normalized;
    }

   
}
