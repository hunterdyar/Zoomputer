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
        _spline = GetComponent<SplineContainer>();
        _splineExtrude = GetComponent<SplineExtrude>();
            
        var se = GetComponent<SplineExtrude>();
        se.RebuildOnSplineChange = false;
    }

    public void SetWire(Wire wire)
    {
        _wire = wire;
    }

    private void Start()
    {
        if (_wire != null)
        {
            RebuildSpline();
        }
    }

    private void OnEnable()
    {
        // RebuildSpline();
    }

    public void RebuildSpline()
    {
        var mf = GetComponent<MeshFilter>();
        //clone the mesh so we aren't using the same asset (from the prefab). yes, this bug was a pain, thanks for asking.
        mf.mesh = Instantiate(mf.sharedMesh);
        //lazy init because lots of non-play mode testing 
        if (_spline == null)
        {
            _spline = GetComponent<SplineContainer>();
        }

        if (_splineExtrude == null)
        {
            _splineExtrude = GetComponent<SplineExtrude>();
        }
        
        //reset splines.
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
