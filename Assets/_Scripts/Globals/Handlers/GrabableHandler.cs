using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public class GrabableHandler : MonoBehaviour
{
    public List<IGrabable> Inventory { get; private set; } = new List<IGrabable>();
    public IGrabable CurrentlyEquipped { get; private set; }
    DebugableObject _debug;
    [SerializeField] Transform DesiredPosition;

    [field: SerializeField]
    public int InventoryCapacity { get; private set; }

    [Range(0, 10f), SerializeField] float _throwForce = 2;

    public bool HasSomethingInHand => CurrentlyEquipped != null || CurrentlyEquipped != default;


    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
    }

    private void Start()
    {
        if (Inventory.Where(x => x != null).Any() && CurrentlyEquipped == null)
            CurrentlyEquipped = Inventory.Where(x => x != null).First();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Tiro Objeto");
            Throw(CurrentlyEquipped);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Uso Objeto");
            UseCurrent();
        }
    }


    public void GrabItem(IGrabable item)
    {
        _debug.Log($"Añado el item {(item as MonoBehaviour).name}");
        Inventory.Add(item);
        
        if (!HasSomethingInHand)
        {
            Equip(item);
            item.SetOwner(this);
        }
        else
        {
            item.Unequip();
        }

    }

    public void Equip(IGrabable item)
    {
        UnEquipCurrent();

        item.Equip();
        CurrentlyEquipped = item;

        CurrentlyEquipped.Transform.position = DesiredPosition.position;
        CurrentlyEquipped.Transform.parent = DesiredPosition;
        CurrentlyEquipped.Transform.forward = DesiredPosition.forward;
    }

    public void UnEquipCurrent()
    {
        if (!HasSomethingInHand) return;

        CurrentlyEquipped.Unequip();
        CurrentlyEquipped.Transform.gameObject.SetActive(false);
        CurrentlyEquipped.Transform.parent = null;
        CurrentlyEquipped = null;
    }

    public void InspectCurrent()
    {
        if (!HasSomethingInHand) return;
        CurrentlyEquipped.Inspect();
    }

    //usa el item actualmente equipado
    public void UseCurrent()
    {
        if (!HasSomethingInHand) 
        {
            Debug.Log("No tengo nada en la mano");
            return;
        }
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
        if (!Inventory.Contains(item)) 
        {
            Debug.Log("No tiro nada");
            return;
        } 

        Debug.Log("Tiro Objeto");
        Inventory.Remove(item);

        UnEquipCurrent();
        item.Release();
        item.Transform.gameObject.SetActive(true);
        Rigidbody rb = null;
        if (item.Transform.root.TryGetComponent(out Rigidbody x))
            rb = x;
        else
        {
            rb = item.Transform.gameObject.AddComponent<Rigidbody>();
            StartCoroutine(DestroyRigidbody(rb)); 
        }
        rb.AddForce(Camera.main.transform.forward * _throwForce, ForceMode.Impulse);
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
