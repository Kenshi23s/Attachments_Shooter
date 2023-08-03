using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class InteractablesManager : MonoSingleton<InteractablesManager>
{
    // En el futuro... Si no voy a necesitar un orden 
    HashSet<IInteractable> _database = new HashSet<IInteractable>();
    List<IInteractable> _interactables = new List<IInteractable>();
    IInteractable _currentInteractable;

    public LayerMask _sightBlock;

    public ReadOnlyCollection<IInteractable> Interactables { get; private set; }

    protected override void SingletonAwake()
    { 
        Interactables = _interactables.AsReadOnly();
    }


    public void AddInteractableObject(IInteractable interactable)
    {

     
        if (_database.Contains(interactable))
        {
            Debug.LogWarning("Trying to add interactable which is already on the list. Returning...");
            return;
        }

        _database.Add(interactable);
        _interactables.Add(interactable);
       
    }

    public void RemoveInteractableObject(IInteractable interactable)
    {
        if (!_database.Contains(interactable))
        {
            Debug.LogWarning("Trying to remove interactable which is not already on the list. Returning...");
            return;
        }

        _database.Remove(interactable);
        _interactables.Remove(interactable);

        if (interactable == _currentInteractable)
        {
            // No se si hace falta esto en realidad. Creo que 'UpdateInteractions' 
            // 

            _currentInteractable.Unfocus();
            _currentInteractable = null;
        }
    }

   
    public void UpdateInteractions(Player_Handler player) 
    {
        IInteractable newInteractable = null;
        float highestPriority = float.NegativeInfinity;

        foreach (IInteractable interactableObject in _interactables)
        {
            
            if (interactableObject.CanInteract(player.InteractFov, out float priority))
            {
                if (priority > highestPriority)
                {
                    highestPriority = priority;
                    newInteractable = interactableObject;
                }
            }
        }

        #region test
        //newInteractable = _interactables.OrderBy(x =>
        //{
        //    if (x.CanInteract(player.InteractFov, out float priority))
        //        return priority;

        //    return float.NegativeInfinity;


        //}).First();
        #endregion

        //Debug.Log(_currentInteractable);
        if (newInteractable != _currentInteractable)
        {
            // Hide any visual feedback from the previous interactable object
            if (_currentInteractable != null)
            {
                _currentInteractable.Unfocus();
                // Show visual feedback for the new interactable object
                // E.g., highlight the object, display a tooltip, etc.
            }

            _currentInteractable = newInteractable;

            if (_currentInteractable != null)
            {
                _currentInteractable.Focus();
                // Show visual feedback for the new interactable object
                // E.g., highlight the object, display a tooltip, etc.
            }
        }

        if (_currentInteractable != null && Input.GetKey(KeyCode.E)) // Example key to trigger interaction
        {
            _currentInteractable.Interact();
        }
    }

}