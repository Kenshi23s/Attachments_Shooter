using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioHolder : MonoBehaviour
{
    AudioSource _myAudioSource;
    Action<AudioHolder> ReturnMethod;
    private void Awake()
    {
        enabled = false;
    }
    public void Configure(Action<AudioHolder> ReturnMethod)
    {
        _myAudioSource = this.gameObject.GetComponent<AudioSource>();
        this.ReturnMethod = ReturnMethod;
        
    }
    public void PlayClip(AudioClip clip, Vector3 WhereToPlay)
    {
        if (clip != null)
        {
            transform.name = clip.name;
            transform.position = WhereToPlay;
            _myAudioSource.PlayOneShot(clip);
            StartCoroutine(WaitForEndClip(clip.length));
        }
        else ReturnMethod(this);
    }
    IEnumerator WaitForEndClip(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnMethod(this);
    }
}
