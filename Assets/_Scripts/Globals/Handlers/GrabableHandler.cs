using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DebugableObject))]
public class GrabableHandler : MonoBehaviour, IGrabableOwner
{
    //[field: SerializeReference]
    public List<IGrabable> Inventory { get; private set; } = new();

    //[field: SerializeReference]
    public IGrabable CurrentlyEquipped { get; private set; }
    DebugableObject _debug;
    [SerializeField] Transform DesiredPosition;

    [field: SerializeField]
    public int InventoryCapacity { get; private set; }

    [Range(0, 10f), SerializeField] float _throwForce = 2;

    public int EquippedIndex => Inventory.IndexOf(CurrentlyEquipped);

    public bool HasSomethingInHand => CurrentlyEquipped != null || CurrentlyEquipped != default;

    #region GadgetOwner 
    public Player_Handler Owner { get; private set; }

    public int OwnerLife => Owner.Health.life;

    public Vector3 Velocity => Owner.Rigidbody.velocity;

    public Vector3 OwnerPosition => transform.position;

    public Quaternion OwnerRotation => transform.rotation;

    public Type OwnerType => GetType();

    public Type[] TargetTypes => new Type[] { typeof(EggEscapeModel), typeof(GrabableObject), typeof(TestClass) };

    public GameObject OwnerGameObject => gameObject;

    #endregion

    public UnityEvent OnEquip, OnGrab, OnUnEquip, onThrow;

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();
        Owner = GetComponent<Player_Handler>();
    }

    private void Start()
    {
        if (Inventory.Where(x => x != null).Any() && CurrentlyEquipped == null)
            CurrentlyEquipped = Inventory.Where(x => x != null).First();
    }

    private void Update()
    {
        if (ScreenManager.IsPaused()) return;

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
        float scrlwheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrlwheel != 0)
            SwapGadget(scrlwheel);


    }


    void SwapGadget(float wheelValue)
    {
        if (Inventory.Count <= 1) return;
        Debug.Log(Inventory.Count);

        int newIndex = Inventory.IndexOf(CurrentlyEquipped);
        // evaluo el indice siguiente al que deberia acceder
        if (wheelValue > 0)
            newIndex = newIndex + 1 >= Inventory.Count ? 0 : ++newIndex;
        else if (wheelValue < 0)
            newIndex = newIndex - 1 < 0 ? Inventory.Count - 1 : --newIndex;
        //hacer --x te devuelve el resultado DESPUES de realizar la operacion
        //si haces x-- te devuelve el resultado ANTES de la operacion
        //ejemplo
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/arithmetic-operators#code-try-0

        Equip(Inventory[newIndex]);
    }


    public void GrabItem(IGrabable item)
    {
        _debug.Log($"Añado el item {(item as MonoBehaviour).name}");
        Inventory.Add(item);
        item.SetOwner(this);
        if (!HasSomethingInHand)       
            Equip(item);       
        else
            UnEquipItem(item);

    }

    public void Equip(IGrabable item)
    {
        UnEquipCurrent();

        item.Equip();
        CurrentlyEquipped = item;

        CurrentlyEquipped.Transform.position = DesiredPosition.position;
        CurrentlyEquipped.Transform.parent = DesiredPosition;
        CurrentlyEquipped.Transform.forward = DesiredPosition.forward;
        OnEquip?.Invoke();
    }

    public void UnEquipCurrent()
    {
        if (!HasSomethingInHand) return;

        UnEquipItem(CurrentlyEquipped);
      
    }

    public void UnEquipItem(IGrabable item)
    {
        item.Unequip();
        item.Transform.gameObject.SetActive(false);
        item.Transform.parent = null;
        OnUnEquip?.Invoke();
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
        CurrentlyEquipped.Use(this);
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
        float scalar = Velocity.magnitude + 1;
        rb.AddForce(Camera.main.transform.forward * _throwForce * scalar, ForceMode.Impulse);
        onThrow?.Invoke();
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
