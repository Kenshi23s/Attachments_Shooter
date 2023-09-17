using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ContractDisplay : MonoBehaviour
{
    public enum ComentColor
    {
        Red, Green, Yellow
    }
    [Serializable]
    public struct ContractComentary
    {
        public TMP_Text comment;
        public Image icon;
       
    }
    public ContractSO ContractSO;

    [Header("Contract")]
    public TMP_Text ContractNameText, RewardText;

    [Header("Species")]
    public TMP_Text SpecieIdentifierText;
    public Image SpeciesImage;

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

    public void ScaleButton()
    {
        transform.localScale = Vector3.one *  1.25f;
    }
    public void UnScaleButton()
    {
        transform.localScale = Vector3.one;
    }



    [ContextMenu("SetChanges")]
    void SetContract()
    {
        if (ContractSO == null) return;

        Cross.SetActive(ContractSO.Available);
        SpeciesImage.sprite = ContractSO.SpeciesImage;
        SpecieIdentifierText.text = ContractSO.GenerateIdentifier();
        ContractNameText.text = ContractSO.contractName;
        RewardText.text = ContractSO.Reward.ToString();

        if (!ContractSO.Comments.Any()) return;
        
        for (int i = 0; i < comentaries.Length; i++)
        {
            if (i > ContractSO.Comments.Length) break;

            comentaries[i].comment.text = ContractSO.Comments[i].Comment;
            comentaries[i].icon.color = ColorFromEnum(ContractSO.Comments[i].color);
        }

    }

    Color ColorFromEnum(ComentColor Selectedcolor)
    {
        switch (Selectedcolor)
        {
            case ComentColor.Red:
                return Color.red;

            case ComentColor.Green:
                return Color.green;

            default:
                return Color.yellow;
           
        }
    } 

    public void SelectContract()
    {
        SceneManager.LoadScene(0);
    }

}
