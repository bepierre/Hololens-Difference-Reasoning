using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrButton : MonoBehaviour {
    private GameObject MiniMap_;
    private GameObject Map_;
    private Camera MainCamera_;
    private RenderDepthDifference RDDscript_;
    private GameObject SpatialMapping_;

    // Use this for initialization
    void Start()
    {
        MiniMap_ = GameObject.Find("MiniMap");
        Map_ = GameObject.Find("Map");
        MainCamera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        RDDscript_ = MainCamera_.GetComponent<RenderDepthDifference>();
        SpatialMapping_ = GameObject.Find("SpatialMapping");
    }

    void OnSelect() {
        RDDscript_.enabled = true;
        Map_.layer = LayerMask.NameToLayer("ReferenceLayer");
        SpatialMapping_.SetActive(true);
    }
}
