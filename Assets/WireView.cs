using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Zoompy;

public class WireView : MonoBehaviour
{
    private SplineContainer _spline;

    private Wire _wire;
    // Start is called before the first frame update
    void Awake()
    {
        _wire = GetComponentInParent<Wire>();
        _spline = GetComponent<SplineContainer>();

        var se = GetComponent<SplineExtrude>();
        se.RebuildOnSplineChange = true;
    }

    private void Start()
    {
        var s = _spline.AddSpline();
        s.Add(new BezierKnot(_wire.From.transform.position));
        s.Add(new BezierKnot(transform.InverseTransformPoint(_wire.To.transform.position)));

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
