using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryOBJ, part1OBJ, part2OBJ, part3OBJ;
    public GameObject backButtonPart1, backButtonPart2, backButtonPart3;

    public enum InventoryButtons
    {
        Epart1,
        Epart2,
        Epart3
    }

    public enum BackButton
    {
        Epart1Back,
        Epart2Back,
        Epart3Back
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void SelectBackbutton(BackButton var)
    {
        switch (var)
        {
            case BackButton.Epart1Back:
                BackButtonPart1();
                break;
            case BackButton.Epart2Back:
                BackButtonPart2();
                break;
            case BackButton.Epart3Back:
                BackButtonPart3();
                break;

        }
    }

    public void SelectButton(InventoryButtons var)
    {
        switch (var)
        {
            case InventoryButtons.Epart1:
                Part1();
                break;
            case InventoryButtons.Epart2:
                Part2();
                break;
            case InventoryButtons.Epart3:
                Part3();
                break;
        }
    }

    public void Part1()
    {
        part1OBJ.SetActive(true);
        part2OBJ.SetActive(false);
        part3OBJ.SetActive(false);
    }

    public void Part2()
    {
        part1OBJ.SetActive(false);
        part2OBJ.SetActive(true);
        part3OBJ.SetActive(false);
    }

    public void Part3()
    {
        part1OBJ.SetActive(false);
        part2OBJ.SetActive(false);
        part3OBJ.SetActive(true);
    }

    public void BackButtonPart1()
    {
        part1OBJ.SetActive(false);
        part2OBJ.SetActive(false);
        part3OBJ.SetActive(false);
        inventoryOBJ.SetActive(true);
    }

    public void BackButtonPart2()
    {
        part1OBJ.SetActive(false);
        part2OBJ.SetActive(false);
        part3OBJ.SetActive(false);
        inventoryOBJ.SetActive(true);
    }

    public void BackButtonPart3()
    {
        part1OBJ.SetActive(false);
        part2OBJ.SetActive(false);
        part3OBJ.SetActive(false);
        inventoryOBJ.SetActive(true);
    }
}
