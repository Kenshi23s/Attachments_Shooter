using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static EggEscapeModel;

public class EggGameChaseMode : GameModeBaseClass
{
    [Header("EggGameMode")]
    [SerializeField] EggEscapeModel model; 

    public float interactRadius => _egg_interactRadius;
    [SerializeField] float _egg_interactRadius;

    [SerializeField, Tooltip("huevos en el mapa, solo lectura")]
    public EggEscapeModel[] eggsEscaping { get; private set; }

    [SerializeField]
    Material[] eggMaterials;
    [SerializeField] Transform IncubatorFather;

    public Egg_Incubator[] incubators { get; private set; }
    public Transform[] waypoints => _waypoints;
    [SerializeField] Transform[] _waypoints;

    public Vector3 playerPos => _player.position;
    [SerializeField] Transform _player;

    [SerializeField]EggStats eggStats;

    public event Action<EggEscapeModel> OnEggGrabed;


    public override void InitializeMode()
    {     
        eggStats.gameMode = this;
        // preguntarle a algun profe o compañero si esto esta bien
        incubators = IncubatorFather.GetComponentsInChildren<Egg_Incubator>();
        eggsEscaping = new EggEscapeModel[incubators.Length];
        for (int i = 0; i < eggsEscaping.Length; i++)
        {
            eggsEscaping[i]= Instantiate(model);
            eggsEscaping[i].Initialize(eggStats, _waypoints[Random.Range(0, waypoints.Length-1)].position);
            eggsEscaping[i].GetComponentInChildren<Renderer>().material = eggMaterials[i];//tendria que ser random, pero como justo coinciden
                                                                                          //cantidad de materiales y huevos...         
        }
    }

    //Vector3 GetMeshPath()
    //{
    //    Transform pos = _waypoints[Random.Range(0, waypoints.Length)];
    //    RaycastHit hit;
    //    if(Physics.Raycast(pos.position,-pos.up,out hit))
    //    {
    //        return hit.point;
    //    }
    //    return pos.position;
    //}

    protected override void ModeFinish()
    {
        // que deberia pasar aca¿?
        //cerrar escena?
        //menu principal?
        //pantalla de victoria?
        //en duda, consultar con equipo
        Debug.Log("EL JUEGO TERMINO");
        Application.Quit();
    }
}
