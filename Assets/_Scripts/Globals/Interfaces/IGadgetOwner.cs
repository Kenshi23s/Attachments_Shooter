using System;
using UnityEngine;

public interface IGadgetOwner
{
    int OwnerLife { get; }

    Vector3 Velocity { get; }

    Vector3 OwnerPosition { get; }

    Quaternion OwnerRotation { get; }

    public GameObject OwnerGameObject { get; }
    Type OwnerType { get; }

    //se podra serializar esto y tirarle los tipos en editor?
    //(algo como los enum digamos)
    //por lo que averigue existe algo, pero no esta tan bueno
    //porque tengo que declarar los tipos en codigo, y desp los puedo ver en editor pero no editarlos desde ahi.
    //preguntar el martes
    Type[] TargetTypes { get; }
}
