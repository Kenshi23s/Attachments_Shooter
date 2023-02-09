using UnityEngine;
[System.Serializable]
public abstract class Perk : MonoBehaviour
{
    protected GunFather myGun;
    internal abstract void InitializePerk(GunFather gun);
    [SerializeField] string perkName;
    [SerializeField] string perkDescription;

}
