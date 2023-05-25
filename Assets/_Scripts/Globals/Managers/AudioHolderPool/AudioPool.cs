using System;
using UnityEngine;

public class AudioPool : MonoSingleton<AudioPool>
{
    public PoolObject<AudioHolder> AudioHoldersPool = new PoolObject<AudioHolder>();
    [SerializeField] AudioHolder AudioHolderSample;
    [SerializeField] int prewarm=5;

    protected override void SingletonAwake()
    {
       

        Action<AudioHolder> _turnOn = (x) => x.gameObject.SetActive(true);

        Action<AudioHolder> _turnOff = (x) => x.gameObject.SetActive(false);

        Func<AudioHolder> _buildHolder = () =>
        {
            AudioHolder x = GameObject.Instantiate(AudioHolderSample);
            x.Configure(ReturnHolder);
            x.transform.SetParent(transform);
            return x;
        };

        AudioHoldersPool.Intialize(_turnOn, _turnOff, _buildHolder, prewarm);
    }
   
    public void SpawnAudio(AudioClip clip,Vector3 WhereToPlay)
    {
       
        AudioHolder A = GetHolder();

        if (A != null&& clip != null) A.PlayClip(clip, WhereToPlay);

        else Debug.LogWarning(" el objeto (" + A.gameObject.name + ") trato de ejecutar un audio no existente " +
            "O el audio holder es igual a null :( ");



    }
     
    void ReturnHolder(AudioHolder a) => AudioHoldersPool.Return(a);    

    AudioHolder GetHolder() => AudioHoldersPool.Get();
  
}
