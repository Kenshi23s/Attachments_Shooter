using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGameChaseMode : GameModeBaseClass
{
    EggEscapeModel model;
    [SerializeField] int _eggsQuantity;
    [SerializeField,Tooltip("huevos en el mapa, solo lectura")]
    EggEscapeModel[] eggsEscaping;
    public Node[] nodes => _nodes;
    [SerializeField]Node[] _nodes; 
    


    public override void InitializeMode()
    {
        eggsEscaping = new EggEscapeModel[_eggsQuantity];

        for (int i = 0; i < eggsEscaping.Length; i++)
        {
            eggsEscaping[i] = GameObject.Instantiate(model);
            //eggsEscaping[i].Initialize();
        }
    }

    protected override void ModeFinish()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
