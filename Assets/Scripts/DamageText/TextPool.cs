using UnityEngine;

public class TextPool 
{
    #region PoolRequirements
    public PoolObject<FloatingText> DamageTextPool = new PoolObject<FloatingText>();

    [SerializeField]
    FloatingText sample;
    //Action<FloatingText> ReturnMethod;
    public int prewarm = 5;
    Transform _transform;
    #endregion

    public void Initialize(Transform _transform, FloatingText sample)
    {
        this.sample= sample;    
        this._transform = _transform;
        DamageTextPool.Intialize(TurnOnHolder, TurnOffHolder, BuildHolder, prewarm);
    }

    FloatingText BuildHolder()
    {
        
        FloatingText dmgText = GameObject.Instantiate(sample);
        dmgText.Configure(ReturnHolder);
        dmgText.transform.SetParent(_transform);
        return dmgText;
    }

    public void TurnOnHolder(FloatingText t) => t.gameObject.SetActive(true);

    public void TurnOffHolder(FloatingText t) => t.gameObject.SetActive(false);

    public void ReturnHolder(FloatingText t) => DamageTextPool.Return(t);

    public FloatingText GetHolder() => DamageTextPool.Get();


}
