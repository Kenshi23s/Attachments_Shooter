using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    
    public GameObject settingsOBJ, menuOBJ, tutorialOBJ;
    public GameObject backButtonSettings, bbackButtonTuto;
    public static MenuManager instance;

    public enum MenuButtons
    {
        Esettings,
        EmainMenu,
        Etutorial
        

    }

    public enum BackButton
    {
        EsettingsBack,
        EtutoBack,
        

    }


    private void Awake()
    {
        instance = this;
        //StartCoroutine(LoadSceneManager.StartPreLoadingLevel(1));        
    }

  


    public void SelectBackbutton(BackButton var)
    {
        switch (var)
        {
            case BackButton.EsettingsBack:
                BackButtonSettings();
                break;
            case BackButton.EtutoBack:
                BackButtonTutorial();
                break;
           
        }
    }

    public void SelectButton(MenuButtons var)
    {
        switch (var)
        {
            case MenuButtons.Esettings:
                Settings();
                break;
            case MenuButtons.EmainMenu:
                MainMenu();
                break;
            case MenuButtons.Etutorial:
                Tutorial();
                break;

        }
    }

   #region ButtonsRegion
    public void Settings()
    {
        settingsOBJ.SetActive(true);
        menuOBJ.SetActive(false);
        
        tutorialOBJ.SetActive(false);   
    }

    public void MainMenu()
    {
        settingsOBJ.SetActive(false);
        menuOBJ.SetActive(true);
        
        tutorialOBJ.SetActive(false);
    }

    public void Tutorial()
    {
        settingsOBJ.SetActive(false);
        menuOBJ.SetActive(false);
        
        tutorialOBJ.SetActive(true);
       
    }

    public void BackButtonSettings()
    {
        settingsOBJ.SetActive(false);
        menuOBJ.SetActive(true);
        
        tutorialOBJ.SetActive(false);
    }

    public void BackButtonStore()
    {
        settingsOBJ.SetActive(false);
        menuOBJ.SetActive(true);
        
        tutorialOBJ.SetActive(false);
    }
     
    public void BackButtonTutorial()
    {
        settingsOBJ.SetActive(false);
        menuOBJ.SetActive(true);
        
        tutorialOBJ.SetActive(false);
    }

#endregion
    
   
}
