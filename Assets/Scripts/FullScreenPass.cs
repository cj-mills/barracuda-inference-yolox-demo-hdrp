using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class FullScreenPass : CustomPass
{
    //public LayerMask outlineLayer = 0;
    //[ColorUsage(false, true)]
    //public Color outlineColor = Color.black;
    //public float threshold = 1;

    public InferenceController inferenceController;



    // To make sure the shader ends up in the build, we keep a reference to it
    //[SerializeField, HideInInspector]
    //Shader outlineShader;

    //Material fullscreenOutline;
    RTHandle outlineBuffer;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        //outlineShader = Shader.Find("Hidden/Outline");
        //fullscreenOutline = CoreUtils.CreateEngineMaterial(outlineShader);

        // Define the outline buffer
        outlineBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices, dimension: TextureXR.dimension,
            colorFormat: GraphicsFormat.B10G11R11_UFloatPack32,
            // We don't need alpha for this effect
            useDynamicScale: true, name: "Outline Buffer"
        );
    }

    protected override void Execute(CustomPassContext ctx)
    {
        // Render meshes we want to apply the outline effect to in the outline buffer
        CoreUtils.SetRenderTarget(ctx.cmd, outlineBuffer, ClearFlag.Color);


        if (inferenceController)
        {
            inferenceController.Infernece(outlineBuffer.rt);
        }
        else
        {
            Debug.Log("inferenceController not assigned");
        }


        //CustomPassUtils.DrawRenderers(ctx, outlineLayer);

        // Set up outline effect properties
        //ctx.propertyBlock.SetColor("_OutlineColor", outlineColor);
        //ctx.propertyBlock.SetTexture("_OutlineBuffer", outlineBuffer);
        //ctx.propertyBlock.SetFloat("_Threshold", threshold);

        // Render the outline buffer fullscreen
        //CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer, ClearFlag.None);
        //CoreUtils.DrawFullScreen(ctx.cmd, fullscreenOutline, ctx.propertyBlock, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        //CoreUtils.Destroy(fullscreenOutline);
        outlineBuffer.Release();
    }
}