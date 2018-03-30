using UnityEngine;

[ExecuteInEditMode] //Allows preview in the editior.
public class LoadDepth : MonoBehaviour
{
    //    [Range(0f, 1f)] //assignes Range slider to the float depthLevel, later taken by the shader
    //    public float depthLevel = 1f;

    private Shader _shader;
    private Shader shader
    {
        get { return _shader != null ? _shader : (_shader = Shader.Find("Custom/RenderDepth")); }
    }

    private void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            print("System doesn't support image effects");
            enabled = false;
            return;
        }
        if (shader == null)
        {
            enabled = false;
            print("Shader is null");
            return;
        }
        if (!shader.isSupported)
        {
            enabled = false;
            print("Shader is not supported");
            return;
        }
        Camera camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) //Attached to all cameras
    {}
}