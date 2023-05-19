using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cyan;

public class Player_UI : MonoBehaviour
{
    [SerializeField] Player_Movement player;
    LifeComponent playerLife;

    [SerializeField] Color _damage=Color.red,Heal=Color.green;
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
            lastLifecheck = Mathf.Clamp01(value);
            mat.SetFloat("_Life", lastLifecheck);
        }
    
    }

    private void Awake()
    {
        mat = _blitBlood.blitPass.blitMaterial;
        playerLife=player.GetComponent<LifeComponent>();
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
        mat.SetColor("_Color", Heal);
        while (LastLifecheck < actualLife)
        {
           yield return new WaitForEndOfFrame();
           LastLifecheck += Time.deltaTime;
        }
        LastLifecheck = actualLife;
    }

    IEnumerator DamageTowards(int actualLife)
    {
        mat.SetColor("_Color", _damage);
        while (LastLifecheck > actualLife)
        {
            yield return new WaitForEndOfFrame();
            LastLifecheck -= Time.deltaTime;
        }
        LastLifecheck = actualLife;

    }



}
