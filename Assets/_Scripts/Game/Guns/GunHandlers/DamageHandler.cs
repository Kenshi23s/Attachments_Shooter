using System.ComponentModel;
using UnityEngine;
[DisallowMultipleComponent]
public class DamageHandler : MonoBehaviour
{
    //esta clase maneja el daño del arma, le falta trabajo
   
    public void Awake()
    {
        _currentDamage = _baseDamage;
    }

    [SerializeField]
    int _baseDamage;
    [SerializeField,ReadOnly(true)]
    int _currentDamage;

    public int baseDamage => _baseDamage;
    public int currentDamage => _currentDamage;

    public void IncreaseDamage(int damageIncrease)=> 
        _currentDamage += Mathf.Abs(damageIncrease);

    public void DecraseDamage(int damageIncrease) => 
        _currentDamage = Mathf.Clamp(_currentDamage, 1, _currentDamage - damageIncrease);
   
}
