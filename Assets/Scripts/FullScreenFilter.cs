using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/FullScreenFilter")]
public sealed class FullScreenFilter : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("")]
    public InferenceManagerParameter inferenceManagerParameter = new InferenceManagerParameter(null);
    

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    RTHandle rtHandle;
    RenderTexture rTex;

    private InferenceController inferenceController;

    public bool IsActive() => inferenceController != null && Application.isPlaying;


    public override void Setup()
    {

        inferenceController = inferenceManagerParameter.value.inferenceController;

        rtHandle = RTHandles.Alloc(
            scaleFactor: Vector2.one,
            filterMode: FilterMode.Point,
            wrapMode: TextureWrapMode.Clamp,
            dimension: TextureDimension.Tex2D,
            enableRandomWrite: true
            );

        // Assign a temporary RenderTexture with the new dimensions
        rTex = RenderTexture.GetTemporary(rtHandle.rt.width, rtHandle.rt.height, 24, RenderTextureFormat.ARGBHalf);
    }



    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        cmd.Blit(source, destination, 0, 0);


        if (inferenceController)
        {
            inferenceController.Infernece(source);
        }
        else
        {
            Debug.Log("inferenceController not assigned");
        }
    }

    public override void Cleanup()
    {
        // Release the resources allocated for the inference engine
        rtHandle.Release();
        rTex.Release();
    }
}
