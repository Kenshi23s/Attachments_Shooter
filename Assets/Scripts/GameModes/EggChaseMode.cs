using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGameChaseMode : GameModeBaseClass
{
    EggEscapeModel model;

    [SerializeField] int _eggsQuantity;

    public float interactRadius=> _interactRadius;
    float _interactRadius;

    [SerializeField,Tooltip("huevos en el mapa, solo lectura")]
    EggEscapeModel[] eggsEscaping;

    public Transform[] waypoints => _waypoints;
    [SerializeField] Transform[] _waypoints;

    public Vector3 playerPos => _player.position;
    [SerializeField] Transform _player;


    public override void InitializeMode()
    {
        // no sabia q se podia hacer esto,
        // preguntarle a algun profe o compañero si esto esta bien
        eggsEscaping = new EggEscapeModel[_eggsQuantity];

        for (int i = 0; i < eggsEscaping.Length; i++)
        {
            eggsEscaping[i] = GameObject.Instantiate(model);
            eggsEscaping[i].Initialize(this, _waypoints[Random.Range(0,waypoints.Length)].position) ;
          
        }
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
