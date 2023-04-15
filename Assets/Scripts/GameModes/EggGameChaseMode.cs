using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EggEscapeModel;

public class EggGameChaseMode : GameModeBaseClass
{
    [Header(" EggGameMode")]
    [SerializeField]EggEscapeModel model;

    [SerializeField] int _eggsQuantity;

    public float interactRadius=> _egg_interactRadius;
    [SerializeField]float _egg_interactRadius;

    [SerializeField,Tooltip("huevos en el mapa, solo lectura")]
    EggEscapeModel[] eggsEscaping;

    public Transform[] waypoints => _waypoints;
    [SerializeField] Transform[] _waypoints;

    public Vector3 playerPos => _player.position;
    [SerializeField] Transform _player;

    [SerializeField]EggStats eggStats;


    public override void InitializeMode()
    {
        eggStats.gameMode = this;
     
        // preguntarle a algun profe o compañero si esto esta bien
        eggsEscaping = new EggEscapeModel[_eggsQuantity];

        for (int i = 0; i < eggsEscaping.Length; i++)
        {
            eggsEscaping[i] = Instantiate(model).GetComponent<EggEscapeModel>();
            eggsEscaping[i].Initialize(eggStats, GetMeshPath());
          
        }
    }

    Vector3 GetMeshPath()
    {
        Transform pos = _waypoints[Random.Range(0, waypoints.Length)];
        RaycastHit hit;
        if(Physics.Raycast(pos.position,-pos.up,out hit))
        {
            return hit.point;
        }
        return pos.position;
    }
    protected override void ModeFinish()
    {
        // que deberia pasar aca¿?
        //cerrar escena?
        //menu principal?
        //pantalla de victoria?
        //en duda, consultar con equipo
    }
}
