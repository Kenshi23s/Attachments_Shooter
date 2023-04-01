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
        if (GUILayout.Button("Selec Gun"))
        {
            GunFather AuxGunSeleceted = Select<GunFather>();
            if (AuxGunSeleceted != null)
            {
                SelectedGun = AuxGunSeleceted;
            }
        }
        if (SelectedGun != null)
        {
            EditorGUILayout.TextField("Gun Selected", SelectedGun.gameObject.name);
        }

        if (GUILayout.Button("Selec Attachment"))
        {
            Attachment AuxattachmentSelected = Select<Attachment>();
            if (AuxattachmentSelected != null)
            {
                attachmentSelected = AuxattachmentSelected;
                availableButtons = attachmentSelected.isAttached ? Detach : Attach;
            }

            
            

        }
        
        if (attachmentSelected != null)
        {
            EditorGUILayout.TextField("Attachment Selected", attachmentSelected.gameObject.name);
        }

       
        availableButtons?.Invoke();
    }

    void Attach()
    {
        if (GUILayout.Button("Attach"))
        {
            if (SelectedGun == null)
            {
                Debug.Log("el arma es == a null");
            }
            //SelectedGun._myAttachMents.AssignGun(SelectedGun);
            SelectedGun._myAttachMents.AddAttachment(attachmentSelected.myType, attachmentSelected);
            availableButtons = Attach;

        }
    }
    void Detach()
    {
        if (GUILayout.Button("Detach"))
        {
            if (SelectedGun==null)
            {
                Debug.Log("el arma es == a null");
            }
            //SelectedGun._myAttachMents.AssignGun(SelectedGun);
            SelectedGun._myAttachMents.RemoveAttachment(attachmentSelected.myType);
            availableButtons = Detach;
        }
    }

    T Select<T>() where T : MonoBehaviour
     {
        foreach (GameObject selected in Selection.gameObjects)
        {
            T item = selected.GetComponent<T>();

            if (item!=null)
            {
                return item;
            }


        }

        return (default);
    }


    //Attachment SelectAttachment()
    //{
    //    foreach (GameObject item in Selection.gameObjects)
    //    {
    //        Attachment attachment=item.GetComponent<Attachment>();
    //        if (attachment!=null && !attachment.isAttached)
    //            return attachment;


    //    }

    //    return null;
    //}

    //Attachment SelectAttachment()
    //{
    //    foreach (GameObject item in Selection.gameObjects)
    //    {
    //        Attachment attachment = item.GetComponent<Attachment>();
    //        if (attachment != null && !attachment.isAttached)
    //            return attachment;


    //    }

    //    return null;
    //}
}
