using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [SerializeField] float _frequency = 1.8f;
    [SerializeField] float _maxAngle = 20f;
    [SerializeField] float _phase;
    [SerializeField] float _speed = 5f;
    public float SpeedMultiplier = 1f;

    [SerializeField] bool _sidewaysMovement = true;
    [SerializeField] bool _verticalMovement = false;

    [SerializeField] bool _preview = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        _boneData = wormTailSO.boneData;
        // Conseguir/actualizar el desfase de la sine wave
        _phase += Time.deltaTime * _speed * SpeedMultiplier;
        for (int i = 0; i < TailBones.Length; i++)
        {
            // Conseguir el desfase del hueso individual
            float bonePhase = _boneData[i].distanceFromRoot + _phase;
            // Conseguir el angulo a rotar
            float angle = _maxAngle * Mathf.Sin(bonePhase * _frequency);

            Quaternion rotation = _boneData[i].originalRotation;
            // A partir del angulo, generar una rotacion

            if (_sidewaysMovement)
                rotation *= Quaternion.AngleAxis(angle, Vector3.up);
            if (_verticalMovement)
                rotation *= Quaternion.AngleAxis(angle, Vector3.right);

            TailBones[i].localRotation = rotation;
        }
    }

    void OnDrawGizmos()
    {
        // Your gizmo drawing thing goes here if required...

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
