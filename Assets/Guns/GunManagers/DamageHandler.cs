using System.ComponentModel;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
   
    public void initialize()
    {
        _actualDamage = _baseDamage;
    }
    [SerializeField]
    int _baseDamage;
    [SerializeField,ReadOnly(true)]
    int _actualDamage;

    public int baseDamage => _baseDamage;
    public int actualDamage => _actualDamage;

    public void IncreaseDamage(int damageIncrease)=> 
        _actualDamage += Mathf.Abs(damageIncrease);

    public void DecraseDamage(int damageIncrease) => 
        _actualDamage = Mathf.Clamp(_actualDamage, 1, _actualDamage - damageIncrease);
   
}
