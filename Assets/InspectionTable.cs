using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(InteractableComponent))]
public class InspectionTable : MonoBehaviour
{
    [SerializeField] Transform desiredCamPos;

    public InteractableComponent InteractableComponent;

    public bool InsideInventory = false;

    private void Awake()
    {
        InteractableComponent = GetComponent<InteractableComponent>();
        InteractableComponent.SetFocusCondition(() => InsideInventory);
    }

    //si abro el inventario creo otra camara en la posicion del jugador? o muevo la camara del jugador hacia el inventario?
    async void OpenInventory()
    {
        InsideInventory = true;
        //
       await LerpCamera(desiredCamPos);
    }

    public async Task LerpCamera(Transform DesiredCamPos)
    {
        Vector3 InitialPos = Camera.main.transform.position;
        Quaternion InitialRot = Camera.main.transform.rotation;

        float t = Time.deltaTime;

        while (t > 1)
        {
            Camera.main.transform.position = Vector3.Lerp(InitialPos, DesiredCamPos.position,t);
            Camera.main.transform.rotation =Quaternion.Lerp(InitialRot,InitialRot,t);
            await Task.Yield();
            t += Time.deltaTime;
        }
        Camera.main.transform.position = Vector3.Lerp(InitialPos, DesiredCamPos.position, 1);
        Camera.main.transform.rotation = Quaternion.Lerp(InitialRot, InitialRot, 1);


    }




}
