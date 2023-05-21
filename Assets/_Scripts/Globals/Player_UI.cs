using System.Collections;
using UnityEngine;
using Cyan;
[RequireComponent(typeof(DebugableObject))]
public class Player_UI : MonoBehaviour
{
    [SerializeField] Player_Movement player;
    LifeComponent playerLife;
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
            lastLifecheck = value;
            mat.SetFloat("_Life", lastLifecheck);
        }
    
    }

    private void Awake()
    {
        mat = _blitBlood.blitPass.blitMaterial;
        _debug=GetComponent<DebugableObject>();
        playerLife =player.GetComponent<LifeComponent>();
        playerLife.OnHealthChange += UpdateDamageShader;
    }

    void UpdateDamageShader(int life,int maxLife)
    {
        int actualLife = life / maxLife;
        if (actualLife == lastLifecheck) return;
     

        if (actualLife>lastLifecheck)        
            StartCoroutine(HealTowards(actualLife));        
        else        
            StartCoroutine(DamageTowards(actualLife));     
    }

    IEnumerator HealTowards(int actualLife)
    {
        mat.SetColor("_Color", _heal);
        while (LastLifecheck < actualLife)
        {
           yield return new WaitForEndOfFrame();
           LastLifecheck += Time.fixedDeltaTime;
        }
        LastLifecheck = actualLife;
        _debug.Log("Heal UI Effect Ended");
    }

    IEnumerator DamageTowards(int actualLife)
    {
        mat.SetColor("_Color", _damage);
        while (LastLifecheck > actualLife)
        {
            yield return new WaitForEndOfFrame();
            LastLifecheck -= Time.fixedDeltaTime;
        }
        LastLifecheck = actualLife;
        _debug.Log("Damage UI Effect Ended");
    }

}
