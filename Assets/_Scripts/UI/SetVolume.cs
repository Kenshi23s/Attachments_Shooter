using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public void SetAudioLevel(float sliderValue)
    {
        AudioListener.volume = sliderValue;
    }
}
