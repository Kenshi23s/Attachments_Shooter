using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool Interact();
    void Focus();
    void Unfocus();
    bool CanInteract(float viewRadius,out float priority);
    public Func<bool> CanFocus { get; }
}
