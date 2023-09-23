using UnityEngine;
using UnityEngine.Rendering;
using Unity.Barracuda;

[System.Serializable]
public class InferenceManagerParameter : VolumeParameter<InferenceManager>
{
    public InferenceManagerParameter(InferenceManager value, bool overrideState = false)
        : base(value, overrideState)
    {

    }
}