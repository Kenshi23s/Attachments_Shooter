using UnityEngine;
[DisallowMultipleComponent]
public class VFX_Sign : MonoBehaviour
{
    LineRenderer sign;
    [SerializeField] Color _color = Color.white;
    [SerializeField] float length=5f;

    private void Start()
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
        sign.gameObject.SetActive(true);
        sign.SetPosition(0, transform.position);
        sign.SetPosition(1, transform.position + (Vector3.up * length));
    }

    public void DeactivateSign()
    {
        sign.gameObject.SetActive(false);
    }
}
