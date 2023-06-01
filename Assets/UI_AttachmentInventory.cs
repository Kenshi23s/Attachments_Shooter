using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using URandom = UnityEngine.Random;

public class UI_AttachmentInventory : MonoBehaviour
{
    [SerializeField]UI_Attachment_Button template;
   [SerializeField] GameObject SavedPanel, EquippedPanel;

    List<UI_Attachment_Button> buttons= new List<UI_Attachment_Button>();

    private void Awake()
    {
        int testNumber = URandom.Range(1,50);
        
        List<Attachment> testAttachments = new List<Attachment>();
        for (int i = 0; i < testNumber; i++)
        {

            Sight a = new Sight();
            a.TESTATTACH(URandom.Range(0,100)%2==0);
            a.TESTNAME = GenerateName(6);
            testAttachments.Add(a);
        }
        SetInventoryUI(testAttachments);
    }
    public void SetInventoryUI(IEnumerable<Attachment> Attachments)
    {
        buttons = Attachments.Aggregate(new FList<UI_Attachment_Button>(), (x, y) =>
        {

            Transform parent = y.isAttached ? EquippedPanel.transform : SavedPanel.transform;
            var fillTemplate = Instantiate(template, parent);
            fillTemplate.AssignAttachment(y);

            return x;
        }).ToList();


    }
    void UpdateCanvas()
    {

    }

    void DestroyButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
    }
    public static string GenerateName(int len)
    {
        System.Random r = new System.Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return Name;


    }
}
