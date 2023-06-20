using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static EggEscapeModel;
using System.Linq;
public class EggGameChaseMode : GameModeBaseClass
{

    [Header("EggGameMode")]
    [SerializeField] EggEscapeModel model; 

    public float interactRadius => _egg_interactRadius;
    [SerializeField] float _egg_interactRadius;

    [SerializeField, Tooltip("huevos en el mapa, solo lectura")]
    public List<EggEscapeModel> eggsEscaping { get; private set; }

    [SerializeField]
    Material[] eggMaterials;
    [SerializeField] Transform IncubatorFather;

    public Egg_Incubator[] incubators { get; private set; }
    public Transform[] waypoints => _waypoints;
    [SerializeField] Transform[] _waypoints;

    public Vector3 playerPos => _player.position;
    [SerializeField] Transform _player;

    [SerializeField]EggStats eggStats;

    public EggEscapeModel[] alreadySpawned;


    public override void InitializeMode()
    {     
        eggStats.gameMode = this;
        

        // preguntarle a algun profe o compañero si esto esta bien
        incubators = IncubatorFather.GetComponentsInChildren<Egg_Incubator>();
       EggEscapeModel[] localArrayEggsEscaping = new EggEscapeModel[incubators.Length-alreadySpawned.Length];
        for (int i = 0; i < localArrayEggsEscaping.Length; i++)
        {
            localArrayEggsEscaping[i]= Instantiate(model);
            localArrayEggsEscaping[i].Initialize(eggStats, _waypoints[Random.Range(0, waypoints.Length-1)].position);
            localArrayEggsEscaping[i].GetComponentInChildren<Renderer>().material = eggMaterials[i];//tendria que ser random, pero como justo coinciden
                                                                                          //cantidad de materiales y huevos...         
        }

        if (alreadySpawned.Any())
        {
            for (int i = 0; i < alreadySpawned.Length; i++)
            {
                alreadySpawned[i].Initialize(eggStats, alreadySpawned[i].transform.position);
                alreadySpawned[i].GetComponentInChildren<Renderer>().material = eggMaterials[2];
            }

            eggsEscaping = localArrayEggsEscaping.Concat(alreadySpawned).ToList();


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


    [SerializeField]ScrollingText winPanel;
    [SerializeField] ObjectiveTextSO winText;
    protected override void ModeFinish()
    {
        // que deberia pasar aca¿?
        //cerrar escena?
        //menu principal?
        //pantalla de victoria?
        //en duda, consultar con equipo
         ScreenManager.PauseGame();
        winPanel.gameObject.SetActive(true);
        winPanel.ActivateText(winText);
        Debug.Log("EL JUEGO TERMINO");
        
    }
}
