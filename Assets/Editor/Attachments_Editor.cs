using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attachments_Editor : EditorWindow
{
    

    

    public GunFather SelectedGun;
    public Attachment attachmentSelected;

    [MenuItem("Window/Editor/AttachmentsDebug")]
    public static void ShowWindow()
    {
        GetWindow<Attachments_Editor>();
    }
    // gui layout se usa cuando queremos hacer espacios, botones y etiquetas
    Action availableButtons;
    //editor gui layout se usa cuando queremos editar campos y propiedades
    private void OnGUI()
    {
        GUILayout.Label("Attachments", EditorStyles.boldLabel);

        if (GUILayout.Button("Selec Attachment"))
        {
           //Attachment AuxattachmentSelected = SelectAttachment();
            if (AuxattachmentSelected!=null)
            {
                attachmentSelected = AuxattachmentSelected;
                //availableButtons = attachmentSelected.isAttached ? 
            }
            
        }
        if (attachmentSelected != null)
        {
            EditorGUILayout.TextField("AttachmentSelected", attachmentSelected.gameObject.name);
        }

    }

    void Detach()
    {
        if (GUILayout.Button("Detach"))
        {
          
        }
    } 

     T Select<T>() 
     {
        foreach (GameObject selected in Selection.gameObjects)
        {
            T item = selected.GetComponent<T>();

            if (item!=null)
            {
                return item;
            }


        }

        return null;
     }


    Attachment SelectAttachment()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            Attachment attachment=item.GetComponent<Attachment>();
            if (attachment!=null && !attachment.isAttached)
                return attachment;


        }

        return null;
    }

    Attachment SelectAttachment()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            Attachment attachment = item.GetComponent<Attachment>();
            if (attachment != null && !attachment.isAttached)
                return attachment;


        }

        return null;
    }
}
