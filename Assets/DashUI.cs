using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public Color Availabe, Unable;
    public Movement_AirDash Owner { get; private set; }
    [SerializeField]Image DashIcon;

    public void SetOwner(Movement_AirDash NewOwner)
    {
        Owner = NewOwner;
        transform.parent = Owner.MovementOwner.HandlerOwner.AbilitiesPanel;
        var x = GetComponent<RectTransform>();
        x.anchoredPosition3D = Vector3.zero;
        x.localScale= Vector3.one;
        x.rotation = Quaternion.identity;

    }

    private void Start()
    {
        if (Owner == null) Destroy(gameObject);

        Owner.OnDash.AddListener(SetCooldownColor);
        Owner.OnDashReady.AddListener(SetAvailableColor);
    }

    void SetAvailableColor()
    {
        StopAllCoroutines();
        StartCoroutine(LerpColor(Availabe));
    }

    void SetCooldownColor()
    {
        StopAllCoroutines();
        StartCoroutine(LerpColor(Unable));
    }

    IEnumerator LerpColor(Color NewColor)
    {
        float t = 0;
        var currentColor = DashIcon.color;
        while (t < 1)
        {
            DashIcon.color = Color.Lerp(currentColor,NewColor,t);
            t += Time.deltaTime;
            yield return null;
        }
        DashIcon.color = NewColor;
    }

}
