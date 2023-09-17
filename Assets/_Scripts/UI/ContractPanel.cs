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
            ActivatePanel(true);
        

        if (Input.GetKeyDown(KeyCode.L))        
            ActivatePanel(false);
        
    }

    void ActivatePanel(bool arg)
    {
        animatorPanel.SetBool("ActivePanel", arg);
    }

}
