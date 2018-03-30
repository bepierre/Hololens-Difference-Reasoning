using UnityEngine;

[ExecuteInEditMode] //Allows preview in the editior.
public class RenderDepth : MonoBehaviour
{
    [Range(0f, 1f)] //assignes Range slider to the float depthLevel, later taken by the shader
    public float depthLevel = 1f;

    private Shader _shader;
    private Shader shader
    {
        get {return _shader != null ? _shader : (_shader = Shader.Find("Custom/RenderDepth"));}
    }

    private Material _material;
    private Material material
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
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
            print("Shader is null");
            enabled = false;
            return;
        }
        if (!shader.isSupported)
        {
            print("Shader is not supported");
            enabled = false;
            return;
        }

        Camera camera = GetComponent<Camera>();
        // turn on depth rendering for the camera so that the shader can access it via _CameraDepthTexture
        camera.depthTextureMode = DepthTextureMode.Depth; //Camera generates a screen-sized depth TEXTURE! It writes to the depth buffer and the shader is then possible to read it.
        //Texture does't neccessary have to be a color.
    }

    private void OnDisable()
    {
        if (_material != null)
            DestroyImmediate(_material);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) //Attached to all cameras
    {
        if (shader != null)
        {
            material.SetFloat("_DepthLevel", depthLevel);
            Graphics.Blit(src, dest, material); //Render this source using this material to this destination; Will render in this case to the render texture attached to the camera (otherwise to the screen).
                                                //Assignes the SOURCE to the main texture were using (_MainTex). Blit sets dest as the render target, sets source _MainTex property on the material, and draws a full - screen quad.
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}