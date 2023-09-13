using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractPanel : MonoBehaviour
{
    Animation activatePanelAnim;
    public Animator animatorPanel;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Doy play");
            animatorPanel.SetBool("ActivePanel", true);

            
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Cierro panel");
            animatorPanel.SetBool("ActivePanel", false);
        }
    }
}
