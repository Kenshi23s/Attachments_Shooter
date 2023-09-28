using System;
using UnityEngine;
// genera una ventana en el menu de create
//me base en este video para hacerlo : https://www.youtube.com/watch?v=aPXvoWVabPY
[CreateAssetMenu(fileName = "Contract", menuName = "Card/Contract")]

public class ContractSO : ScriptableObject
{
    [Serializable]
    public struct CommentaryFill
    {
        public string Comment;
    }
    public new string contractName, speciesName;
    public new int Reward = 300;

    public string SceneToLoad = default;
    public Sprite SpeciesImage;
    public Texture sadtex;

    public CommentaryFill[] Comments;

    public bool Available = true;



    //genera un 
    public string GenerateIdentifier()
    {
        var identifier = speciesName + "#";
        for (int i = 0; i < 4; i++)
            identifier += UnityEngine.Random.Range(0, 10);
        return identifier;
    }
}
