using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;

[System.Serializable]
public struct BoneData
{
    public Quaternion originalRotation;
    public float distanceFromRoot;
    public BoneData(Quaternion originalRotation, float distanceFromRoot)
    {
        this.originalRotation = originalRotation;
        this.distanceFromRoot = distanceFromRoot;
    }
}

public class WormTail : MonoBehaviour
{
    [SerializeField] Transform[] TailBones = new Transform[0];

    [Header("Sideways Movement")]
    [SerializeField] bool _sidewaysMovement = true;
    [SerializeField] float _sidewaysFrequency = 1.8f;
    [SerializeField] float _maxSidewaysAngle = 20f;
    [SerializeField, NotKeyable] float _sidewaysPhase;
    [SerializeField] float _sidewaysSpeed = 1f;

    [Header("Vertical Movement")]
    [SerializeField] bool _verticalMovement = false;
    [SerializeField] float _verticalFrequency = 1.8f;
    [SerializeField] float _maxVerticalAngle = 20f;
    [SerializeField, NotKeyable] float _verticalPhase;
    [SerializeField] float _verticalSpeed = 0f;


    [SerializeField, NotKeyable] bool _preview = false;

    [SerializeField] WormTailSO wormTailSO;

    BoneData[] _boneData;


    private void OnValidate()
    {
#if UNITY_EDITOR
        runInEditMode = _preview;
#endif
    }

    private void Start()
    {
        LoadBoneData();
    }

    void LoadBoneData()
    {
        // Copiar data de los huesos desde el scriptable object.
        _boneData = wormTailSO.boneData;
    }

    [ContextMenu("SaveBoneData")]
    void SaveBoneData() 
    {
        wormTailSO.boneData = new BoneData[TailBones.Length];

        Vector3 rootPos = TailBones[0].transform.position;
        for (int i = 0; i < TailBones.Length; i++)
        {
            Transform bone = TailBones[i];
            wormTailSO.boneData[i] = new BoneData(bone.localRotation, Vector3.Distance(rootPos, bone.position));
        }

        wormTailSO.SetDirty();
    }

    // Update is called once per frame
    void Update()
    {
        _boneData = wormTailSO.boneData;
        // Conseguir/actualizar el desfase de la sine wave
        for (int i = 0; i < TailBones.Length; i++)
        {
            float bonePhase;
            float angle;
            Quaternion rotation = _boneData[i].originalRotation;

            if (_sidewaysMovement) 
            {
                _sidewaysPhase += Time.deltaTime * _sidewaysSpeed;

                // Conseguir el desfase del hueso individual
                bonePhase = _boneData[i].distanceFromRoot + _sidewaysPhase;
                // Conseguir el angulo a rotar
                angle = _maxSidewaysAngle * Mathf.Sin(bonePhase * _sidewaysFrequency);

                // A partir del angulo, generar una rotacion
                rotation *= Quaternion.AngleAxis(angle, Vector3.up);
            }


            if (_verticalMovement) 
            {
                _verticalPhase += Time.deltaTime * _verticalSpeed;

                // Conseguir el desfase del hueso individual
                bonePhase = _boneData[i].distanceFromRoot + _verticalPhase;
                // Conseguir el angulo a rotar
                angle = _maxVerticalAngle * Mathf.Sin(bonePhase * _verticalFrequency);

                // A partir del angulo, generar una rotacion
                rotation *= Quaternion.AngleAxis(angle, Vector3.right);
            }

            Debug.Log("PREV ROTATION: " + TailBones[i].localRotation);
            TailBones[i].localRotation = rotation;
            Debug.Log("POST ROTATION: " + TailBones[i].localRotation);
        }
    }


    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // Ensure continuous Update calls.
        if (_preview && !Application.IsPlaying(gameObject))
        {
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
#endif
    }
}
