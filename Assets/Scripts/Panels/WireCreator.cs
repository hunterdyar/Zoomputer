using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Splines;

namespace Zoompy.Panels
{
	public class WireCreator : MonoBehaviour
	{
		//Generate Connections
		//Connection -> 90turn to height of other connection, 90turn, connect. 
		
		//1. Figure out how to create knots that do the correct radius
		//2. Figure out how to decide where to place these knots to make it good. 
        
        public Transform _portA;
        public Transform _portB;
        public float curve = 0.4f;
        private MeshRenderer _meshRenderer;
        private SplineContainer _splineContainer;
        private SplineExtrude _splineExtrude;

        private void Awake()
        {
	        _meshRenderer = GetComponent<MeshRenderer>();
	        _splineContainer = GetComponent<SplineContainer>();
	        _splineExtrude = GetComponent<SplineExtrude>();
        }

        private void Start()
        {
	        RedrawWire();
        }

        [ContextMenu("Redraw Wire")]
        public void RedrawWire()
        {
	        Spline spline;
	        if (_splineContainer.Splines.Count != 1)
	        {
		        //Clear existing spline...(s)

		        while (_splineContainer.Splines.Count > 0)
		        {
			        _splineContainer.RemoveSplineAt(0);
		        }

		        spline = new Spline();
		        _splineContainer.AddSpline(spline);
	        }
	        else
	        {
		        spline = _splineContainer.Splines[0];
	        }

	        spline.Clear();
	        //First and second are leaving the first port at it's Z
	        //third and fourth are entering the second port at it's Z
	        Vector3 a = _portA.position;
	        Vector3 d = _portB.position;
	        int zdir = a.z > d.z ? -1 : 1;//dir from a to d
	        
	        Vector3 half = Vector3.Lerp(a, d, 0.5f);
	        Vector3 ba = new Vector3(half.x-curve, a.y, a.z);
	        Vector3 bb = new Vector3(half.x, a.y, a.z+curve*zdir);
	        Vector3 ca = new Vector3(half.x, d.y, d.z+curve*zdir*-1);
	        Vector3 cb = new Vector3(half.x+curve, d.y, d.z);
	        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(a), Vector3.left*curve, Vector3.right*curve));
	        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(ba), Vector3.left*curve, Vector3.right*curve));

	        //Tangent out.
	        float verticalDistance = Vector3.Distance(bb, ca);
	        if (verticalDistance > curve * 2)
	        {

		        spline.Add(new BezierKnot(transform.InverseTransformPoint(bb), Vector3.back * curve * zdir,
			        Vector3.forward * curve * zdir)); //
		        spline.Add(new BezierKnot(transform.InverseTransformPoint(ca),
			        Vector3.back * curve * zdir, Vector3.forward * curve * zdir));
	        }
	        else
	        {
		        // var newBBCA = Vector2.Lerp(bb, ca, 0.5f);
		        // spline.Add(new BezierKnot(transform.InverseTransformPoint(newBBCA), Vector3.back * curve * zdir, Vector3.forward * curve * zdir)); //
	        }

	        spline.Add(new BezierKnot(transform.InverseTransformPoint(cb), Vector3.left * curve, Vector3.right * curve));
	        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(d), Vector3.left * curve, Vector3.right * curve));

	        
	        //
	        _splineExtrude.Rebuild();
        }
	}
}