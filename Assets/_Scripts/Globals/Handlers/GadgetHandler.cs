using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GadgetHandler : MonoBehaviour
{
    public Gadget_Scanner Scanner { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Scanner.UseGadgetScan(EggGameChaseMode.Instance.eggsEscaping.Select(x => x.transform));
        }
    }
}
