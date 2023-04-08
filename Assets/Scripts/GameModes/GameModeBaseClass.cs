using UnityEngine;

public abstract class GameModeBaseClass : MonoBehaviour
{
    [Header("GameModeBaseClass")]    
    [SerializeField]
    public float points;

    [SerializeField]
    protected float pointsToWin=5;

    protected abstract void ModeFinish();

    public abstract void InitializeMode();

    private void Awake() => InitializeMode();

    public void AddPoints(int value)
    {    
        points = Mathf.Clamp(points, points + Mathf.Abs(value), pointsToWin);

        if (points >= pointsToWin)
            ModeFinish();
        
    }

}
