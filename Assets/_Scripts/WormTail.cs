using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTail : MonoBehaviour
{
    [SerializeField] Transform[] TailBones = new Transform[0];
    [SerializeField] float frequency = 1f;
    [SerializeField] float maxAngle = 20f;
    [SerializeField] float phase;
    [SerializeField] float speed = 1f;


    Dictionary<Transform, BoneData> boneData = new Dictionary<Transform, BoneData>();

    private struct BoneData 
    {
        public Quaternion originalRotation;
        public float distanceFromRoot;
        public BoneData(Quaternion originalRotation, float distanceFromRoot) 
        {
            this.originalRotation = originalRotation;
            this.distanceFromRoot = distanceFromRoot;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rootPos = TailBones[0].transform.position;
        for (int i = 0; i < TailBones.Length; i++)
        {
            Transform bone = TailBones[i];
            boneData.Add(bone, new BoneData(bone.localRotation, Vector3.Distance(rootPos, bone.position)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        phase = Time.realtimeSinceStartup * speed;
        foreach (var bone in TailBones)
        {
            bone.localRotation = boneData[bone].originalRotation * Quaternion.AngleAxis(maxAngle * Mathf.Sin((boneData[bone].distanceFromRoot + phase) * frequency), Vector3.up);
        }
    }
}
