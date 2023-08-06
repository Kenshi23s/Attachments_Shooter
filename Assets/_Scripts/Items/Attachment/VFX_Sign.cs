using UnityEngine;
[DisallowMultipleComponent]
public class VFX_Sign : MonoBehaviour
{
    LineRenderer sign;
    [SerializeField] Color _color = Color.blue;
    [SerializeField] float length=5f;

    public Vector3 InitialPos { get; private set; }


    public Vector3 NewInitialPos
    {
        set 
        {
            InitialPos = value;
        } 
    }


    private void Awake()
    {
        InitialPos = transform.position;
    }

   
    private void CreateSign()
    {
        sign = Instantiate(GameManager.instance.sampleLineSign, transform);
        sign.transform.parent = transform;

        Material mat = sign.GetComponent<Renderer>().material;
        mat.SetColor("_Color", _color);
        //sign.GetComponent<Renderer>().material = mat;

        sign.name = gameObject.name + " HelpSign";
       
    }

    public void ActivateSign()
    {
        if (sign == null)
        {
            CreateSign();
        }
       
        sign.gameObject.SetActive(true);
        sign.SetPosition(0, InitialPos);
        sign.SetPosition(1, InitialPos +(Vector3.up * length));
    }

    public void DeactivateSign()
    {
        if (sign!=null)
        {
            sign.gameObject.SetActive(false);
        }
       
    }
}
