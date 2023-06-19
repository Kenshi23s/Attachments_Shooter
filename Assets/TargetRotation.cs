using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
[RequireComponent(typeof(InteractableComponent))]

public class TargetRotation : MonoBehaviour
{
    [SerializeField] float rangeRotation = 50f, speed = 3, deface;

    Quaternion _ogRotation;

    InteractableComponent _interactableComponent;

    [SerializeField] int steps = 6;

    [SerializeField] TextAndFiller textnFiller;

    private void OnValidate()
    {
        speed = Mathf.Abs(speed);
    }

    private void Awake()
    {
        callbakcRandomRotation = () => MakeRandomRotation(steps);
        _interactableComponent = GetComponent<InteractableComponent>();

        _ogRotation = transform.rotation;

        OnValidate();       
    }

    UnityAction callbakcRandomRotation;
    private void Start()
    {
        _interactableComponent.OnInteract.AddListener(callbakcRandomRotation);
        _interactableComponent.onFocus.AddListener(()=>textnFiller.gameObject.SetActive(true));
        _interactableComponent.onUnFocus.AddListener(() => textnFiller.gameObject.SetActive(false));

    }

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


    IEnumerator StartRotatingTowards(float degrees, Action onEnd)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion target = _ogRotation * Quaternion.Euler(0, degrees, 0);

        float currentDegrees = 0;
        float angle = Quaternion.Angle(startRotation, target);

        while (currentDegrees <= angle)
        {
            currentDegrees += Time.deltaTime * speed;
            transform.rotation = Quaternion.Lerp(startRotation, target, Mathf.SmoothStep(0, 1, currentDegrees / angle));
          
            yield return null;
        }
      
        Debug.Log("nextStep");
        onEnd();
    }


   
}
