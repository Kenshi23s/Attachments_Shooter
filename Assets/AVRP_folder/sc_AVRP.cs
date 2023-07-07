using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class sc_AVRP : MonoBehaviour
{
    // A uto
    // V olumetric
    // R eflection
    // P robes

    [Header("Prefab")]
    public GameObject prefabProbe;

    [Header("Colors")]
    public Color colorWire;
    public Color colorGrid;
    public Color colorLimit;

    [Header("Grid and box sizes")]
    public Vector3 boxSize;
    public float distancePerPoint;
    public float blendProbes;

    [Header("Options")]
    public bool autoClean;
    [Tooltip("Se borran automaticamente los probes que esten dentro de un collider")]
    public bool autoDrownedClean;
    [Tooltip("Se borran automaticamente los probes que no tienen vision al cielo")]
    public bool autoSkySpaceClean;
    [Tooltip("Se borran automaticamente los probes que no tienen objetos cerca")]
    public bool autoNearObjectClean;

    [Header("Gizmos")]
    public bool showGrid;
    public bool showBox;
    public bool showPoints;

    //[Header("Lists")]
    [NonSerialized] public List<sc_RProbe> probes = new List<sc_RProbe>();

    void DrawGrid()
    {
        Gizmos.color = colorGrid;
        Vector3 midPoint = StepRound(transform.position,distancePerPoint) * distancePerPoint;

        //horizontal
        for (int i = StepRound(-boxSize.x * 0.5f, distancePerPoint); i < StepRound(boxSize.x * 0.5f, distancePerPoint) + 1; i++)
        {
            Gizmos.DrawLine(new Vector3(i * distancePerPoint, 0, StepRound(-boxSize.x*0.5f, distancePerPoint) * distancePerPoint) + midPoint, new Vector3(i * distancePerPoint, 0, StepRound(boxSize.x * 0.5f, distancePerPoint) * distancePerPoint) + midPoint);
        }

        //vertical
        for (int i = StepRound(-boxSize.x * 0.5f, distancePerPoint); i < StepRound(boxSize.x * 0.5f, distancePerPoint) + 1; i++)
        {
            Gizmos.DrawLine(new Vector3(StepRound(-boxSize.x * 0.5f, distancePerPoint) * distancePerPoint, 0, i * distancePerPoint) + midPoint, new Vector3(StepRound(boxSize.x * 0.5f, distancePerPoint) * distancePerPoint, 0, i * distancePerPoint) + midPoint);
        }
    }

    void DrawProbes()
    {
        Gizmos.color = colorWire;

        foreach (var item in probes)
        {
            if (item != null)
            {
                Gizmos.DrawWireSphere(item.transform.position, 1f);
            }          
        }
    }

    void DrawLimits(Vector3 p1, Vector3 p2)
    {
        Gizmos.color = colorLimit;

        Gizmos.DrawLine(p1, new Vector3(p2.x, p1.y, p1.z));
        Gizmos.DrawLine(p1, new Vector3(p1.x, p2.y, p1.z));
        Gizmos.DrawLine(p1, new Vector3(p1.x, p1.y, p2.z));

        Gizmos.DrawLine(p2, new Vector3(p1.x, p2.y, p2.z));
        Gizmos.DrawLine(p2, new Vector3(p2.x, p1.y, p2.z));
        Gizmos.DrawLine(p2, new Vector3(p2.x, p2.y, p1.z));

        Gizmos.DrawLine(new Vector3(p1.x, p2.y, p2.z), new Vector3(p1.x, p1.y, p2.z));
        Gizmos.DrawLine(new Vector3(p1.x, p2.y, p2.z), new Vector3(p1.x, p2.y, p1.z));

        Gizmos.DrawLine(new Vector3(p2.x, p1.y, p2.z), new Vector3(p2.x, p1.y, p1.z));
        Gizmos.DrawLine(new Vector3(p2.x, p1.y, p2.z), new Vector3(p1.x, p1.y, p2.z));

        Gizmos.DrawLine(new Vector3(p2.x, p2.y, p1.z), new Vector3(p1.x, p2.y, p1.z));
        Gizmos.DrawLine(new Vector3(p2.x, p2.y, p1.z), new Vector3(p2.x, p1.y, p1.z));

    }

    public void CreateProbes(Vector3 p1, Vector3 p2)
    {
        //limpiar todo para generar devuelta
        if (probes.Count > 0) { RemoveAllProbes(); }
        

        p1 = StepRound(p1, distancePerPoint);

        p2 = StepRound(p2, distancePerPoint);

        //crea los probes
        for (int i = (int)p1.x; i < p2.x; i++) //x
        {
            for (int j = (int)p1.y; j < p2.y; j++) //y
            {
                for (int k = (int)p1.z; k < p2.z; k++) //z
                {

                    sc_RProbe probe = Instantiate(prefabProbe,new Vector3(i + 0.5f,j + 0.5f, k + 0.5f) * distancePerPoint, Quaternion.identity).GetComponent<sc_RProbe>();

                    if (probe == null) { Debug.LogError("no existe un script de probe dentro del prefab"); return; }

                    probes.Add(probe);
                    probe.transform.parent = this.transform;
                    probe.ScaleProbe(distancePerPoint, blendProbes);

                }
            }
        }

        if (autoClean) { CleanAllProbes(); }
    }

    bool BetweenLimits(Vector3 myObj, Vector3 p1, Vector3 p2)
    {
        //conseguir los limites minimo y maximos
        Vector3 minPoint = new Vector3( Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), Mathf.Min(p1.z, p2.z));
        Vector3 maxPoint = new Vector3(Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y), Mathf.Max(p1.z, p2.z));

        if (myObj.x > minPoint.x && myObj.y > minPoint.y && myObj.z > minPoint.z  &&  myObj.x < maxPoint.x && myObj.y < maxPoint.y && myObj.z < maxPoint.z)
        {
            return true;
        }

        return false;
    }


    public void DetectExistingProbes()
    {
        probes.Clear();

        probes = this.GetComponentsInChildren<sc_RProbe>().ToList();
    }

    public void AddIndividualProbe()
    {
        sc_RProbe probe = Instantiate(prefabProbe, transform.position, Quaternion.identity).GetComponent<sc_RProbe>();

        probes.Add(probe);
        probe.transform.parent = this.transform;
        probe.ScaleProbe(distancePerPoint, blendProbes);
    }

    public void RemoveIndividualProbe(GameObject previousProbe)
    {
        var myRP = previousProbe.GetComponent<sc_RProbe>();

        if (myRP != null)
        {
            DestroyImmediate(previousProbe);
        }
    }


    #region StepRound
    /// <summary>
    /// Te dice los pasos que puede hacer, redondea lo mas alejado del 0
    /// </summary>
    /// <param name="value"></param>
    /// <param name="stepsDistance"></param>
    /// <returns></returns>
    public int StepRound(float value, float stepsDistance)
    {
        if(value > 0)
        {
            return (int)Mathf.Ceil(value / stepsDistance);
        }
        else if (value < 0)
        {
            return (int)Mathf.Floor(value / stepsDistance);
        }

        return 0;
    }

    public Vector3 StepRound(Vector3 value, float stepDistance)
    {
        value.x = StepRound(value.x, stepDistance);
        value.y = StepRound(value.y, stepDistance);
        value.z = StepRound(value.z, stepDistance);

        return value;
    }

    #endregion

    #region DetectDrowned
    public void DrownedAllProbes()
    {
        foreach (var item in probes)
        {
            if (item!=null)
            {
                DrownedProbe(item);
            }         
        }
    }

    /// <summary>
    /// Si este Probe esta dentro de un collider, no deberia de existir, se borra
    /// </summary>
    public void DrownedProbe(sc_RProbe myProbe)
    {
        if (myProbe.CheckDrowned())
        {
            DestroyImmediate(myProbe.gameObject);
        }
    }

    #endregion

    public void NearObjectsAllProbes()
    {
        foreach (var item in probes)
        {
            if (item != null)
            {
                NearObjectProbe(item);
            }
        }
    }

    /// <summary>
    /// Si este Probe no tiene vision al cielo, no deberia de existir, se borra
    /// </summary>
    public void NearObjectProbe(sc_RProbe myProbe)
    {
        if (myProbe.CheckNearObjects())
        {
            DestroyImmediate(myProbe.gameObject);
        }
    }

    #region DetectSky

    public void SkySpaceAllProbes()
    {
        foreach (var item in probes)
        {
            if (item != null)
            {
                SkySpaceProbe(item);
            }
        }
    }

    /// <summary>
    /// Si este Probe no tiene vision al cielo, no deberia de existir, se borra
    /// </summary>
    public void SkySpaceProbe(sc_RProbe myProbe)
    {
        if (myProbe.CheckSkySpace())
        {
            DestroyImmediate(myProbe.gameObject);
        }
    }

    #endregion


    public void RemoveAllProbes()
    {
        if (probes.Count < 1)
        {
            Debug.LogWarning("No existe ningun probe en este Box");
            return;
        }

        for (int i = 0; i < probes.Count; i++)
        {
            if (probes[i] != null)
            {
                DestroyImmediate(probes[i].gameObject);
            }
           
        }

        probes.Clear();
    }

    public void CleanAllProbes()
    {
        if (autoDrownedClean)
        {
            DrownedAllProbes();
        }

        if (autoSkySpaceClean)
        {
            SkySpaceAllProbes();
        }

        if (autoNearObjectClean)
        {
            NearObjectsAllProbes();
        }

        DetectExistingProbes();
    }

    private void OnDrawGizmos()
    {
        if (showBox)
        {
            DrawLimits(GetCornerBox(transform.position, -boxSize), GetCornerBox(transform.position, boxSize));
        }       

        if (showGrid)
        {
            DrawGrid();
        }

        if (showPoints)
        {
            DrawProbes();
        }
    }

    public Vector3 GetCornerBox(Vector3 mycenter, Vector3 size)
    {
        return mycenter + (size * 0.5f);
    }
}
