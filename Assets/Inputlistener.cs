using UnityEngine;


public class Inputlistener : MonoBehaviour
{

    //Dictionary<KeyCode,Action> inputs= new Dictionary<KeyCode,Action>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GunManager.instance.TriggerActualGun();
        }
    }
}
