using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Zoompy;

public class WireView : MonoBehaviour
{
    private SplineContainer _spline;
    private SplineExtrude _splineExtrude;
    private Wire _wire;
    // Start is called before the first frame update
    void Awake()
    {
        _wire = GetComponentInParent<Wire>();
        _spline = GetComponent<SplineContainer>();
        _splineExtrude = GetComponent<SplineExtrude>();
            
        var se = GetComponent<SplineExtrude>();
        se.RebuildOnSplineChange = true;
    }

    private void Start()
    {
        RebuildSpline();
    }

    private void OnEnable()
    {
        // RebuildSpline();
    }

    void RebuildSpline()
    {
        while (_spline.Splines.Count > 0)
        {
            _spline.RemoveSplineAt(0);
        }

        var points = new List<float3>()
        {
            _wire.From.transform.position, _wire.To.transform.position,
        };
        var s = SplineFactory.CreateCatmullRom(points);
        _spline.AddSpline(s);
        _spline.Warmup();
        _splineExtrude.Rebuild();
    }
}
