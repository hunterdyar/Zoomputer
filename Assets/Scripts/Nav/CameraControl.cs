using System;
using System.Collections;
using System.Xml.Schema;
using UnityEditor.Splines;
using UnityEngine;

namespace Zoompy.Nav
{
	//This will be someone's ... face.. in VR. SO no need to implement cinemachine or do anything 'good' beyond what communicates the prototype effectively.
	public class CameraControl : MonoBehaviour
	{
		private Camera _cam;
		private static CameraControl Instance;
		private Coroutine _camMoveRoutine;
		private Vector3 _startPos;
		private Quaternion _startRot;
		
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(this);
			}

			_startPos = transform.position;
			_startRot = transform.rotation;
		}

		private void Start()
		{
			_cam = Camera.main;
		}

		public static void MoveToStartPosition()
		{
			if (Instance._camMoveRoutine != null)
			{
				Instance.StopCoroutine(Instance._camMoveRoutine);
			}

			Instance.StartCoroutine(Instance.MoveTo(Instance._startPos, Instance._startRot, 0.5f));
		}
		public static void FrameBoundingBox(Bounds bounds)
		{
			var distance = bounds.size.magnitude * 2;//trig is annoying let's just ballpark it.
			var destinationPosition = bounds.center+Vector3.up*distance;
			var destinationRotation = Quaternion.LookRotation(bounds.center - destinationPosition,Vector3.up);
			if (Instance._camMoveRoutine != null)
			{
				Instance.StopCoroutine(Instance._camMoveRoutine);
			}

			Instance.StartCoroutine(Instance.MoveTo(destinationPosition, destinationRotation,0.5f));
		}

		IEnumerator MoveTo(Vector3 pos, Quaternion rot, float totalTime)
		{
			var start = transform.position;
			var end = pos;
			var startRot = transform.rotation;
			var endRot = rot;
			var t = 0f;
			while (t < 1)
			{
				transform.position = Vector3.Lerp(start, end, t);
				transform.rotation = Quaternion.Lerp(startRot,endRot,t);
				t += Time.deltaTime / totalTime;
				yield return null;
			}

			transform.position = end;
			transform.rotation = endRot;
		}
	}
}