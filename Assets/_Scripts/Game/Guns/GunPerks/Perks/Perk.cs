using UnityEngine;
[System.Serializable]
public abstract class Perk : MonoBehaviour// deberia heredar de monobehaiviour??
                                          // porque la idea seria q las armas creen sus perks,
                                          // por lo que no estarian en escena
                                          // sino que las crearian sus armas por composicion
{
    //todos los perks deben de heredar de esta clase para su correcto funcionamiento
    protected Gun myGun;
    public abstract void InitializePerk(Gun gun);
    public string GetName() => perkName;
    [SerializeField] string perkName;
    [SerializeField] string perkDescription;

    public void testJSON()
    {

    }
    

   

}
