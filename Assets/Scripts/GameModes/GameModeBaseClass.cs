using UnityEngine;

public abstract class GameModeBaseClass : MonoSingleton<GameModeBaseClass> 
{
    

    [Header("GameModeBaseClass")]    
    [SerializeField]
    public float points;



    [SerializeField]protected float pointsToWin=5;

    protected abstract void ModeFinish();

    public abstract void InitializeMode();

    protected override void ArtificialAwake()
    {
        base.ArtificialAwake();
        InitializeMode();

    }

    public void AddPoints(float value)
    {
        points += Mathf.Abs(value);
        points = Mathf.Clamp(points, 0f, pointsToWin);

        if (points >= pointsToWin)
        {
            ModeFinish();
        }
    }

  

   
   

    //void GoToMenus() => LoadSceneManager.LoadASyncLevel(0);
}
