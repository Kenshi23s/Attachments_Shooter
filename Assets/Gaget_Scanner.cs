using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gadget_Scanner : Gadget
{
    [field: SerializeField] public float PreferedFieldSize { get; private set; } = 10;

    [field: SerializeField] public float IncreaseSpeed { get; private set; } = 1;

    [field: SerializeField] public float ActiveTime { get; private set; } = 3;

    [field: SerializeField] public float CD_waitingTime { get; private set; } = 5F;

    public bool IsActive { get; private set; } = false;

    Outline outline;

   

    public bool UseGadgetScan(IEnumerable<Transform> targets)
    {
        if (IsActive)
            return false;

        StopAllCoroutines();
        StartCoroutine(TurnOnRadar(targets));

        return true;
    }

    IEnumerator TurnOnRadar(IEnumerable<Transform> targets)
    {
        IsActive = true;
        while (transform.localScale.magnitude < PreferedFieldSize)
        {
            transform.localScale += Vector3.one * Time.deltaTime * IncreaseSpeed;

            if (targets.Where(IsNearby).Any())
            {
                var nearest = targets
                    .Minimum(x => Vector3.Distance(x.transform.position,transform.position));
                 SetOutline(nearest);
            }

            Debug.Log("Estoy aumentando la escala");

            yield return null;
        }

        yield return new WaitForSeconds(ActiveTime);

        transform.localScale = Vector3.one;

        Debug.Log("Volvio a la normalidad"); 
        yield return new WaitForSeconds(CD_waitingTime);
        IsActive = false;


        Debug.Log("Puedo volver a activar radar");

    }

    public bool IsNearby(Transform x)
    {
        var distance = Vector3.Distance(x.transform.position, transform.position);
        return transform.localScale.magnitude >= distance;
    }

    IEnumerator SetOutline(Transform target)
    {
        outline = target.gameObject.AddComponent<Outline>(); outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineAll;

        yield return new WaitForSeconds(ActiveTime);
        Destroy(outline);
    }

    public override bool UseGadget()
    {
        return false;
    }

    public override void AwakeContinue()
    {
        throw new System.NotImplementedException();
    }
}
