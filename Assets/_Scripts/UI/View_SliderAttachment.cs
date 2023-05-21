using UnityEngine;
using UnityEngine.UI;

public class View_SliderAttachment : MonoBehaviour
{
    float maxShowValue = 100f;
    [SerializeField] Slider positiveStat, negativeStat;
    [SerializeField] Text statName;

    public void SetSliderValue(string name,int value)
    {
        statName.text = "Stat";

        if (value > 0)
        {
            negativeStat.value = 0;
            positiveStat.value = value * (1 / maxShowValue);
        }
        else
        {
            positiveStat.value = 0;
            negativeStat.value = value * (1 / -maxShowValue);
        }
    }

}
