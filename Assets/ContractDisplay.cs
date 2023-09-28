using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class ContractDisplay : InteractableCard
{
   
    [Serializable]
    public struct ContractComentary
    {
        public TMP_Text comment;     
    }

    public ContractSO ContractSO;

    [Header("Contract")]
    public TMP_Text ContractNameText, RewardText;

    [Header("Species")]
    public TMP_Text SpecieIdentifierText;
    public Image SpeciesImage;

    //para saber 
    [Header("Availability")]
    public GameObject Cross;
    public Tuple<Image, string> SpecieImage;

    [SerializeField]
    ContractComentary[] comentaries;
   

    private void Awake()
    {
        SetContract();
    }

    private void OnValidate()
    {
        SetContract();
    }

    


    //setea los cambios en base al Scriptable object del contrato pasado por referencia
    [ContextMenu("SetChanges")]
    void SetContract()
    {
        if (ContractSO == null) return;

        Cross.SetActive(!ContractSO.Available);
        SpeciesImage.sprite = ContractSO.SpeciesImage;
        SpecieIdentifierText.text = ContractSO.GenerateIdentifier();
        ContractNameText.text = ContractSO.contractName;
        RewardText.text = ContractSO.Reward.ToString();

        if (!ContractSO.Comments.Any()) return;
        
        for (int i = 0; i < comentaries.Length; i++)
        {
            if (i > ContractSO.Comments.Length) break;

            comentaries[i].comment.text = ContractSO.Comments[i].Comment;
            //comentaries[i].icon.color = ColorFromEnum(ContractSO.Comments[i].color);
        }

    }

   
    public void SelectContract()
    {
        if (!ContractSO.Available) return;

        if (SceneManager.GetActiveScene().name == ContractSO.SceneToLoad)
        {
            //logica de iniciar mision?
        }
        else
        {
            SceneManager.LoadScene(ContractSO.SceneToLoad);
           
        }
       
    }

}
