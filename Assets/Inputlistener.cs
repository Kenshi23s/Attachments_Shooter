using UnityEngine;


public class Inputlistener : MonoBehaviour
{

    //Dictionary<KeyCode,Action> inputs= new Dictionary<KeyCode,Action>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Input");
            GunManager.instance.TriggerActualGun();
        }
    }
}
