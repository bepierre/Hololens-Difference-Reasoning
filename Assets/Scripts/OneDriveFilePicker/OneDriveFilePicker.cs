using UnityEngine;
using System;
using System.Collections;

#if WINDOWS_UWP

using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Threading;
using System.Threading.Tasks;

public class OneDriveFilePicker : MonoBehaviour
{
    FileOpenPicker openPicker;

    public void Start()
    {
        Debug.Log("Started (UWP)");
    }

    public void OpenFile()
    {
        OpenFileAsync();
    }

    private async void OpenFileAsync()
    {
        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Objects3D;
            openPicker.FileTypeFilter.Add(".fbx");
            openPicker.FileTypeFilter.Add(".obj");
            openPicker.FileTypeFilter.Add(".mtl");

            StorageFile file = await openPicker.PickSingleFileAsync();
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {

                if (file != null)
                {
                    // Application now has read/write access to the picked file
                    Debug.Log("Picked file: " + file.DisplayName);
                    //GameObject obj = new GameObject("loadedModel");
                    //MeshFilter mf = obj.AddComponent<MeshFilter>();
                    //MeshRenderer mr = obj.AddComponent<MeshRenderer>();

                    //ObjImporter objImporter = new ObjImporter();
                    //mf.mesh = objImporter.ImportFile(file.Path);

                    //Material[] temp = new Material[1];
                    //temp[0] = new Material(Shader.Find("Legacy Shaders/Diffuse"));
                    //temp[0].name = "default";
                    //mr.materials = temp;

                    //obj.transform.position = new Vector3(0, 0, 2);
                    //obj.transform.localScale = new Vector3(1, 1, 1);

                    //obj.SetActive(true);
                    //obj.GetComponent<Renderer>().enabled = true;
                    GameObject manager = GameObject.Find("Managers");
                    OBJ obj_script = manager.GetComponent<OBJ>();

                    var uri = new System.Uri(file.Path);
                    var converted_fp = uri.AbsoluteUri;
                    obj_script.objPath = converted_fp;
                    obj_script.enabled = true;

                    //GameObject obj = OBJLoader.LoadOBJFile(file.Path);
                    //obj.transform.position = new Vector3(0, 0, 0);
                    //obj.transform.localScale = new Vector3(1, 1, 1);
                    //if (obj.activeSelf)
                    //{
                    //    obj.SetActive(true);
                    //    for (int i = 0; i < obj.transform.childCount; ++i)
                    //    {
                    //        obj.transform.GetChild(i).gameObject.SetActive(true);
                    //    }
                    //}
                    //obj.GetComponent<Renderer>().enabled = true;
                    //Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                    //foreach (Renderer r in renderers)
                    //{
                    //    r.enabled = true;
                    //}
                }
                else
                {
                    // The picker was dismissed with no selected file
                    Debug.Log("File picker operation cancelled");
                }
                 

            }, true);


        }, false);

    }
}

#else
public class OneDriveFilePicker : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("Started (Editor)");
    }

    public void OpenFile()
    {
    }
}

#endif