using System;
using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public abstract class GameModeBaseClass : MonoBehaviour
{
    [Header("GameModeBaseClass")]    
    [SerializeField]
    public int points;
    [SerializeField]
    protected int pointsToWin=5;

    protected DebugableObject _debug;

    public static int actualPoints;

    protected abstract void ModeFinish();

    public abstract void InitializeMode();

    private void Awake()
    {
        _debug = GetComponent<DebugableObject>();      
    }

    public static event Action<int> onPointsChange;
    public static event Action OnModeEnd;
    public void AddPoints(int value)
    {    
        points = Mathf.Clamp(points, points + Mathf.Abs(value), pointsToWin);
        actualPoints = points;
        if (points >= pointsToWin) ModeFinish();
    }

}
