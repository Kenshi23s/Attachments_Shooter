using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Contract", menuName = "Card/Contract")]

public class ContractSO : ScriptableObject
{
    [Serializable]
    public struct CommentaryFill
    {
        public ContractDisplay.ComentColor color;
        public string Comment;
    }
    public new string contractName, speciesName;
    public new int Reward = 300;

    public string SceneToLoad = default;
    public Sprite SpeciesImage;

    public CommentaryFill[] Comments;

    public bool Available = true;


    public string GenerateIdentifier()
    {
        var identifier = speciesName + "#";
        for (int i = 0; i < 4; i++)
            identifier += UnityEngine.Random.Range(0, 10);
        return identifier;
    }
}
