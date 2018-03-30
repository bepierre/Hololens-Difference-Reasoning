using UnityEngine;
using System.IO;

public class RenderDepthDifference : MonoBehaviour
{
 //   [Range(0f, 1f)]
 //   public float depthLevel = 0.5f;
    [Range(0f, 2f)]
    public float depthDifferenceThreshold = 0.1f; // Threshold on the difference due to sensor imprecision
    [Range(0f, 1f)]
    public float spatialMappingIntensity = 1f;    // Intensity in color of the texture of the spatial mapping
    private Camera ReferenceCamera_;              // Pre-loaded model
    private Camera MainCamera_;                   // Live scan (spatial mapping)

    private Shader _shader;
    private Shader shader
    {      
        get {
            return _shader != null ? _shader : (_shader = Shader.Find("Custom/RenderDepthDifference"));
        }
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
        MainCamera_ = GetComponent<Camera>();
        MainCamera_.depthTextureMode = DepthTextureMode.Depth;
        ReferenceCamera_ = GameObject.FindWithTag("ReferenceCamera").GetComponent<Camera>();
        //ReferenceCamera_ = GameObject.Find("ReferenceCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        ReferenceCamera_.transform.position = MainCamera_.transform.position;
        ReferenceCamera_.transform.rotation = MainCamera_.transform.rotation;
    }

    private void OnDisable()
    {
        if (_material != null)
        {
            DestroyImmediate(_material);
        }

    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (shader != null)
        {
            //         material.SetFloat("_DepthLevel", depthLevel);
            material.SetFloat("_DepthDifferenceThreshold", depthDifferenceThreshold);
            material.SetFloat("_SpatialMappingIntensity", spatialMappingIntensity);

            //ReferenceCamera_.depth = MainCamera_.depth - 1;
            ReferenceCamera_.Render();


            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}