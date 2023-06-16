using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_EggActualCount;

    [SerializeField] TextMeshProUGUI txt_EggTotal;

    void Start()
    {
        ModesManager.instance.actualMode.onPointsChange += UpdateEggCount;
        txt_EggTotal.text = "/" + ModesManager.instance.actualMode.maxPoints.ToString(); 
    }

    void UpdateEggCount(int num)
    {
        txt_EggActualCount.text = num.ToString();

    }
}
