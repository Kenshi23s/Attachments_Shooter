using System.ComponentModel;
using UnityEngine;
[DisallowMultipleComponent]
public class DamageHandler : MonoBehaviour
{
    //esta clase maneja el daño del arma, le falta trabajo
   
    public void Initialize()
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
