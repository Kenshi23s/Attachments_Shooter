using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void OnEnter();
    public void OnVirtualUpdate();
    public void OnExit();
}
