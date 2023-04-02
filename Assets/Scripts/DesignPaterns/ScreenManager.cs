
using System.Collections.Generic;
public interface IPausable
{
    void Pause();

    void Resume();
}

public static class ScreenManager 
{
    static List<IPausable> pausables =  new List<IPausable>();
    //multiplico todas las velocidades del juego por "Time"
    public static float time = 1;
    public static bool isPaused;

    public static void AddPausable(IPausable item)
    {
        if (!pausables.Contains(item))
        {
            pausables.Add(item);
        }
    }

    public static void RemovePausable(IPausable item)
    {
        if (!pausables.Contains(item)) pausables.Add(item);
       
    }


    public static void PauseGame()
    {
        time = 0 ;
        isPaused=true ;
        foreach (IPausable item in pausables) item.Pause();
        
           
        
    }

    public static void ResumeGame()
    {
        time = 1 ;
        isPaused = false;
        foreach (IPausable item in pausables) item.Resume();
      
    }
}



