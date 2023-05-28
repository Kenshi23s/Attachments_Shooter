using UnityEngine;

public class PerkHandler : MonoBehaviour
{

    //deberia de alguna manera poder obtener 2 perks randoms de todos los perks que existen
    public Perk[] perks = new Perk[2];
    Gun myGun;

  
    private void Start()
    {
        myGun = GetComponent<Gun>();
        for (int i = 0; i < perks.Length; i++)
        {
            if (perks[i] == null) 
            {
                perks[i] = myGun.GetRandomPerk();              
            }
            perks[i].InitializePerk(myGun);
        }
    }

    public void ChangePerk(Perk newPerk,int i)
    {
        if (perks.Length > i || i < 0) return;
        perks[i] = newPerk;
        perks[i].InitializePerk(myGun);

    }
}
