using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg_VFXHandler : MonoBehaviour
{
    public EggEscapeModel Owner { get; private set; }
    TextAndFiller InteractUI;
    [SerializeField]LineRenderer _playerLinking;


   
    public void Initialize(EggEscapeModel newOwner)
    {
        Owner = newOwner;
        InteractUI = GetComponentInChildren<TextAndFiller>();

      
        

        Owner.InteractComponent.OnStartInteracting.AddListener(InteractUI.TurnSliderTextOn);
        Owner.InteractComponent.OnInteractAbort.AddListener(InteractUI.TurnSliderTextOff);
        Owner.InteractComponent.onTryingToInteract.AddListener(UpdateInteractUI);



     

        Owner.OnGrab.AddListener(EnableRopeLink);
        Owner.OnRelease.AddListener(DisableRopeLink);
    }

    
    void UpdateInteractUI()
    {  
        InteractUI.SetSliderValue(Owner.InteractComponent.NormalizedProgress);
    }

    #region Rope
    public void EnableRopeLink() 
    {
        Debug.LogError(_playerLinking);
        _playerLinking.gameObject.SetActive(true);
        Owner.OnUpdate += UpdateLinkRope;
    } 

    public void DisableRopeLink()
    {
        Owner.OnUpdate -= UpdateLinkRope;
        _playerLinking.gameObject.SetActive(false);
    } 


    void UpdateLinkRope()
    {
        _playerLinking.SetPosition(0, transform.position);
        var playerPos = Owner.CurrentEggStats.gameMode.playerPos;
        _playerLinking.SetPosition(1, Vector3.Slerp(transform.position, playerPos, 0.5f));
        _playerLinking.SetPosition(2, playerPos);
    }
    #endregion
}
