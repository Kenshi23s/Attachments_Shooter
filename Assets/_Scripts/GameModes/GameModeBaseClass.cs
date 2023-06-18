using System;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public abstract class GameModeBaseClass : MonoBehaviour
{
    [Header("GameModeBaseClass")]    
    [SerializeField]
    public int points=0;
    [SerializeField]
    protected int pointsToWin=5;
    public int maxPoints => pointsToWin;

    public event Action<int> onPointsChange;
    public event Action OnModeEnd;
    protected DebugableObject _debug;


    protected abstract void ModeFinish();

    public abstract void InitializeMode();

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();      
    }


 
    public void AddPoints(int value)
    {    
        points = Mathf.Clamp(points, points + Mathf.Abs(value), pointsToWin);
        onPointsChange?.Invoke(points);


        if (points >= pointsToWin) 
        {
            ModeFinish(); OnModeEnd?.Invoke();
        } 
    }

}
