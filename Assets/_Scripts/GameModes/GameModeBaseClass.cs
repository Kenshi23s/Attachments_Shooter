using UnityEngine;
[RequireComponent(typeof(DebugableObject))]
public abstract class GameModeBaseClass : MonoBehaviour
{
    [Header("GameModeBaseClass")]    
    [SerializeField]
    public float points;
    [SerializeField]
    protected float pointsToWin=5;

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

        if (points >= pointsToWin) ModeFinish();
    }

}
