using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Splines;

namespace Zoompy.Panels
{
	public class WireCreator : MonoBehaviour, IConnectionListener
	{
		//Generate Connections
		//Connection -> 90turn to height of other connection, 90turn, connect. 
		
		//1. Figure out how to create knots that do the correct radius
		//2. Figure out how to decide where to place these knots to make it good. 
        
        [Min(0)]
        public float curve = 0.4f;

        public Material OnMaterial;
        public Material OffMaterial;
        
        [Header("Runtime")]
		public Transform _portA;
        public Transform _portB;
        
        
        
        private MeshRenderer _meshRenderer;
        private SplineContainer _splineContainer;
        private SplineExtrude _splineExtrude;

        private float _lastDrawCurve;
        private Vector3 _lastADrawPoint;
        private Vector3 _lastBDrawPoint;

        //runtime reference
        private ZConnection _connection;
        private ConnectionHub _hub;

        private void Awake()
        {
	        _meshRenderer = GetComponent<MeshRenderer>();
	        _splineContainer = GetComponent<SplineContainer>();
	        _splineExtrude = GetComponent<SplineExtrude>();
	        var f = GetComponent<MeshFilter>();
	        f.mesh = new Mesh();
        }

        void Update()
        {
	        if (_portA != null && _portB != null)
	        {
		        if (_portA.position != _lastADrawPoint || _portB.position != _lastBDrawPoint || _lastDrawCurve != curve)
		        {
			        RedrawWire();
		        }
	        }
        }

        public void SetConnection(ConnectionHub hub, ZConnection connection)
        {
	        _hub = hub;
	        _connection = connection;
	        hub.RegisterChangeListener(connection, this);
        }
        
        public void OnConnectionDidChange(ZConnection connection, byte data)
        {
	        if (data > 0)
	        {
		        _meshRenderer.material = OnMaterial;
	        }
	        else
	        {
		        _meshRenderer.material = OffMaterial;
	        }
        }

        private void Start()
        {
	        RedrawWire();
        }

        [ContextMenu("Redraw Wire")]
        public void RedrawWire()
        {
	        float curveTight = 2;
	        if (_portA == null || _portB == null)
	        {
		        _meshRenderer.enabled = false;
		        return;
	        }
	        else
	        {
		        _meshRenderer.enabled = true;
	        }
			_lastADrawPoint = _portA.position;
			_lastBDrawPoint = _portB.position;
			_lastDrawCurve = curve;
			
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
            float step = curve;
	        //First and second are leaving the first port at it's Z
	        //third and fourth are entering the second port at it's Z
	        Vector3 a = _portA.position;
	        Vector3 d = _portB.position;

	        //shrink the curve for smaller distances.
	        if (Mathf.Abs(a.z - d.z) < curve * 2)
	        {
		        float dis = Mathf.Abs(a.z - d.z);
		        step = dis / 2 - Mathf.Epsilon;
	        }
	        
	        int zdir = a.z > d.z ? -1 : 1;//dir from a to d
	        
	        Vector3 half = Vector3.Lerp(a, d, 0.5f);
	        Vector3 ba = new Vector3(half.x-step, a.y, a.z);
	        Vector3 bb = new Vector3(half.x, a.y, a.z+step*zdir);
	        Vector3 ca = new Vector3(half.x, d.y, d.z+step*zdir*-1);
	        Vector3 cb = new Vector3(half.x+step, d.y, d.z);
		        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(a), Vector3.left*step/curveTight, Vector3.right*step/curveTight));
	        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(ba), Vector3.left*step/curveTight, Vector3.right*step/curveTight));

	        //Tangent out.
	        float verticalDistance = Vector3.Distance(bb, ca);
	        
	        if (verticalDistance > _splineExtrude.Radius*2)
	        {

		        spline.Add(new BezierKnot(transform.InverseTransformPoint(bb), Vector3.back * step / curveTight * zdir,
			        Vector3.forward * step / curveTight * zdir)); //
		        spline.Add(new BezierKnot(transform.InverseTransformPoint(ca),
			        Vector3.back * step / curveTight * zdir, Vector3.forward * step / curveTight * zdir));
	        }
	        else
	        {
		        //Push ba and cb away from each other.
	        }

	        spline.Add(new BezierKnot(transform.InverseTransformPoint(cb), Vector3.left * step / curveTight, Vector3.right * step /
		        curveTight));
	        
	        spline.Add(new BezierKnot(transform.InverseTransformPoint(d), Vector3.left * step / curveTight, Vector3.right * step /
		        curveTight));

	        
	        //
	        _splineExtrude.Rebuild();

			gameObject.name = $"Wire [{_portA.gameObject.name}] -> [{_portB.gameObject.name}]";
        }

        private void OnDestroy()
        {
	        _hub.DeregisterChangeListener(_connection, this);
        }
	}
}
