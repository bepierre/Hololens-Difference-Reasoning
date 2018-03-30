using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowMiniMapTransformations : MonoBehaviour {
    private GameObject MiniMap_;

    //private Vector3 LastScale_;
    private Vector3 ScaleFactor_;
    

    private Vector3 LastMiniMapPos_;

	// Use this for initialization
	void Start () {
        MiniMap_ = GameObject.Find("MiniMap");

        //LastScale_ = MiniMap_.transform.localScale;
        ScaleFactor_ = ElementWiseDivision(transform.localScale, MiniMap_.transform.localScale);

        LastMiniMapPos_ = MiniMap_.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        // Remove if mini map was removed
        //if (MiniMap_ == null)
        //    Destroy(this.gameObject);

        // follow rotation
        transform.rotation = MiniMap_.transform.rotation;

        // follow dragging
        transform.position += MiniMap_.transform.position - LastMiniMapPos_;
        LastMiniMapPos_ = MiniMap_.transform.position;

        // --- old version where the dragging was multuplied by the scaling (caused minimap to not stay in the center of the bigger map)
        // (vector.scale is element wise multiplication)
        // transform.position += Vector3.Scale(ScaleFactor_, MiniMap_.transform.position - LastMiniMapPos_);
        // LastMiniMapPos_ = MiniMap_.transform.position;

        // follow scaling
        transform.localScale = Vector3.Scale(ScaleFactor_, MiniMap_.transform.localScale);
    }

    Vector3 ElementWiseDivision(Vector3 a, Vector3 b)
    {
        if (b[0] == 0 || b[1] == 0 || b[2] == 0)
            throw new Exception("Trying to divide by zero in element wise division");

        return new Vector3(a[0] / b[0], a[1] / b[1], a[2] / b[2]);
    }
}