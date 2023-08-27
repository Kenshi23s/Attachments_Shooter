using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabableHandler : MonoBehaviour
{
    public List<IGrabable> Inventory { get; private set; } = new List<IGrabable>();
    public IGrabable CurrentlyEquipped { get; private set; }

    [SerializeField] Transform DesiredPosition;

    [field: SerializeField]
    public int InventoryCapacity { get; private set; }

    [Range(0, 10f), SerializeField] float ThrowForce;

    public bool SomethingInHand => CurrentlyEquipped != null;


    private void Awake()
    {


    }

    private void Start()
    {
        if (Inventory.Where(x => x != null).Any() && CurrentlyEquipped == null)
            CurrentlyEquipped = Inventory.Where(x => x != null).First();
    }


    public void GrabItem(IGrabable item)
    {
        Inventory.Add(item);
        item.Unequip();
    }

    public void Equip(IGrabable item)
    {
        UnEquipCurrent();

        item.Equip();
        CurrentlyEquipped = item;

        CurrentlyEquipped.Transform.parent = DesiredPosition;
        CurrentlyEquipped.Transform.position = Vector3.zero;
        CurrentlyEquipped.Transform.forward = DesiredPosition.forward;
    }

    public void UnEquipCurrent()
    {
        if (!SomethingInHand) return;

        CurrentlyEquipped.Unequip();
        CurrentlyEquipped.Transform.gameObject.SetActive(false);
        CurrentlyEquipped.Transform.parent = null;
        CurrentlyEquipped = null;
    }

    public void InspectCurrent()
    {
        if (!SomethingInHand) return;
        CurrentlyEquipped.Inspect();
    }

    //usa el item actualmente equipado
    public void UseCurrent()
    {
        if (!SomethingInHand) return;
        CurrentlyEquipped.Use();
    }

    /// <summary>
    /// Tira con una fuerza un IGrabable que este en el inventario
    /// si el item tiene rigidbody, se le añade una fuerza a ese rigidbody
    /// sino, se le crea uno temporal
    /// </summary>
    /// <param name="item"></param>
    void Throw(IGrabable item)
    {
        if (!Inventory.Contains(item)) return;

        Inventory.Remove(item);

        item.Unequip();

        item.Transform.gameObject.SetActive(true);
        Rigidbody rb = null;
        if (item.Transform.root.TryGetComponent(out Rigidbody x))
            rb = x;
        else
        {
            rb = item.Transform.gameObject.AddComponent<Rigidbody>();
            DestroyRigidbody(rb);
        }
        rb.AddForce(Camera.main.transform.forward * ThrowForce, ForceMode.Impulse);
    }


    IEnumerator DestroyRigidbody(Rigidbody tempRB)
    {
        // re cabeza hacer esto, es para prototipar
        // despues lo cambiare
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => tempRB.velocity == Vector3.zero);
        Destroy(tempRB);
    }


}
