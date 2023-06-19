using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
[RequireComponent(typeof(InteractableComponent))]

public class TargetRotation : MonoBehaviour
{
    [SerializeField] float rangeRotation = 50f,speed=3,deface;
  
    InteractableComponent _interactableComponent;

    [SerializeField] int steps = 6;

    [SerializeField] TextAndFiller textnFiller;

    private void Awake()
    {
        callbakcRandomRotation = () => MakeRandomRotation(steps);
        _interactableComponent = GetComponent<InteractableComponent>();
       
    }

    private void Start()
    {
        _interactableComponent.OnInteract.AddListener(callbakcRandomRotation);
        _interactableComponent.onFocus.AddListener(()=>textnFiller.gameObject.SetActive(true));
        _interactableComponent.onUnFocus.AddListener(() => textnFiller.gameObject.SetActive(false));

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
    UnityAction callbakcRandomRotation;
    void MakeRandomRotation(int manyTimes)
    {
        _interactableComponent.OnInteract.RemoveAllListeners();
        Debug.Log(manyTimes);
        if (0 >= manyTimes) 
        {
            _interactableComponent.OnInteract.AddListener(callbakcRandomRotation);
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
