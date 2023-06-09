using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_QuadTreeViewCamera : MonoBehaviour
{
    Player_Movement myGuide;
    [SerializeField] MeshRenderer myRender;

    void Start()
    {
        myGuide = FindObjectOfType<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("si");

        if (myRender.isVisible)
        {
            Vector3 myvec = (transform.position - myGuide.transform.position);
            myRender.transform.forward = new Vector3(myvec.x, 0, myvec.y).normalized;
        }
    }
}
