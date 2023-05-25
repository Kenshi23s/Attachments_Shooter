using System.Collections;
using UnityEngine;
using Cyan;
using System;

[RequireComponent(typeof(DebugableObject))]
public class Player_UI : MonoBehaviour
{
    [SerializeField] Player_Movement player;
    DebugableObject _debug;

    [SerializeField] Color _damage=Color.red,_heal=Color.green;
    [SerializeField] Blit _blitBlood;
    Material mat;
    [SerializeField,Range(0,1)]float lastLifecheck;

    public float LastLifecheck 
    {
        get 
        { 
            return lastLifecheck; 
        
        }  
        private set
        {
            lastLifecheck = Mathf.Clamp(value,0,1);
            mat.SetFloat("_Life", lastLifecheck);
        }
    
    }

    float actualLife;
    bool _isHealing = false;
    bool _isDamaging = false;

    private void Awake()
    {
        mat = _blitBlood.blitPass.blitMaterial;
        _debug = GetComponent<DebugableObject>();
        player.GetComponent<LifeComponent>().OnHealthChange += UpdateDamageShader; ;     
    }

    void UpdateDamageShader(int life,int maxLife)
    {
        _debug.Log($"la vida actual es  {life} y la vida maxima{maxLife}");
         actualLife = (float)life /(float) maxLife;

       
        if (actualLife == lastLifecheck) return;

        //overkill??
        Action x = actualLife > lastLifecheck ? HealTowards : DamageTowards;      
        x.Invoke();                     
    }

   
    void HealTowards()
    {
        if (!_isHealing) StartCoroutine(HealTowardsCoroutine());             
    }
    IEnumerator HealTowardsCoroutine()
    {
        mat.SetColor("_Color", _heal);
        _isHealing = true;
        while (LastLifecheck < actualLife)
        {
           yield return new WaitForEndOfFrame();
           LastLifecheck += Time.deltaTime;
          
        }
        _isHealing = false;
        LastLifecheck = actualLife;
        mat.SetColor("_Color", _damage);
        _debug.Log("Heal UI Effect Ended");
    }

  
    void DamageTowards()
    {
        if (!_isDamaging) StartCoroutine(DamageTowardsCoroutine());
    }
    IEnumerator DamageTowardsCoroutine()
    {
        _isDamaging= true;
        mat.SetColor("_Color", _damage);
        StopCoroutine(HealTowardsCoroutine());
        while (LastLifecheck > actualLife)
        {
            yield return new WaitForEndOfFrame();
            LastLifecheck -= Time.deltaTime;
            _debug.Log($"chequeo actual {LastLifecheck}, Vida actual {actualLife}");
        }
        _isDamaging = false;
        LastLifecheck = actualLife;
        _debug.Log("Damage UI Effect Ended");
    }

}
