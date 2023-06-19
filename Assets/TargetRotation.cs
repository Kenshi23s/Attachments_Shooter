using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(InteractableComponent))]
public class TargetRotation : MonoBehaviour
{
    [SerializeField] float rangeRotation = 50f,speed=3,deface;
  
    InteractableComponent _interactableComponent;

    [SerializeField] int steps = 6;

    private void Awake()
    {
        //_interactableComponent = GetComponent<InteractableComponent>();
        //_interactableComponent.OnInteract.AddListener(() => MakeRandomRotation(steps));
    }
    private void Update()
    {
  
    }
    [SerializeField]bool button;
    private void LateUpdate()
    {
        if (button)
        {
            button = false;
            MakeRandomRotation(steps);
        }
    }
    void MakeRandomRotation(int manyTimes)
    {
        Debug.Log(manyTimes);
        if (0 >= manyTimes) 
        {
            StartCoroutine(StartRotatingTowards(0, () => { }));
            return;
        }
       
        manyTimes--;
  
        float degrees = Random.Range(-rangeRotation, rangeRotation);

        


        StartCoroutine(StartRotatingTowards(degrees, () => MakeRandomRotation(manyTimes)));
    }


    IEnumerator StartRotatingTowards(float degrees,Action onEnd)
    {
        Quaternion target = Quaternion.Euler(0, degrees, 0);
        while (Quaternion.Angle(transform.rotation, target) > 3f)
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.fixedDeltaTime * speed);
          
            yield return null;
        }
      
        Debug.Log("nextStep");
        onEnd();
    }


   
}
