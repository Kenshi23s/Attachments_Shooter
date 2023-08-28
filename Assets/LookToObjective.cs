using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static EggEscapeModel;

public class LookToObjective : MonoBehaviour
{
    [SerializeField] EggGameChaseMode gamemode;
    [SerializeField] float maxDistance;
    RawImage _signalImage;
    Material SignalMat;
    // Start is called before the first frame update
    void Start()
    {
        if (gamemode && gamemode.isActiveAndEnabled)
        {
            StartCoroutine(LookTowards());
        }
        else
        {
            Destroy(gameObject);
        }

        _signalImage = GetComponent<RawImage>();
        SignalMat = GetComponent<RawImage>().material;


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
            yield return null;
            SignalMat.SetFloat("_SignalStrength", GetArrowCount());
        }
    }


    int GetArrowCount()
    {
        int divideBy = 4;
        float eggDistance = GetEggDistance();
        if (eggDistance <= 0)
        {
            _signalImage.enabled = false;
            Debug.LogWarning("Devuelvo 0");
            return 0;
        }
        else
        {
            _signalImage.enabled = true;
        }

        float dividedValue = (1 - eggDistance / maxDistance) * 4;
        return (int)Mathf.Max(1,Mathf.Ceil(dividedValue));
    
    }


    float GetEggDistance()
    {
        Vector3 FinalPos = Vector3.zero;

        if (gamemode.eggsEscaping.Where(x => x.actualState == EggStates.Kidnapped).Any()) return 0;
        FinalPos = gamemode.eggsEscaping
       .Minimum(x => Vector3.Distance(x.transform.position, Player_Handler.Position)).transform.position;

        return (FinalPos - Player_Handler.Position).magnitude;
    }


}
