
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    static bool isPaused;

    public static bool IsPaused() => isPaused;

    public static void AddPausable(IPausable item)
    { if (!pausables.Contains(item)) pausables.Add(item);}

    public static void RemovePausable(IPausable item) 
    { if (!pausables.Contains(item)) pausables.Add(item);}
 


    public static void PauseGame()
    {
        pausables = pausables.Where(x => x!= null).ToList();


        time = 0 ;
        isPaused=true ;
        string ItemsPaused = "";
        foreach (IPausable item in pausables)
        {
            item.Pause();
            ItemsPaused += $"{item} ";
        }
        Debug.Log("Se PAUSARON los items" + ItemsPaused);


    }

    public static void ResumeGame()
    {
        time = 1 ;
        isPaused = false;
        string ItemsPaused = "";
        foreach (IPausable item in pausables) 
        {
            item.Resume();
            ItemsPaused += $" {item}, ";
        }
        Debug.Log("Se DES PAUSARON los items" + ItemsPaused);
    }
}



