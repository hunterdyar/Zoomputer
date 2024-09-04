using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

//This object is attached to the Panel Prefab
public class PanelModelController : MonoBehaviour
{
    [Header("Rig Transforms")] [SerializeField]
    private Transform canvasPos;
    [SerializeField] private Transform RigTopRight;
    [SerializeField] private Transform RigTopLeft;
    [SerializeField] private Transform RigMidRight;
    [SerializeField] private Transform RigMidLeft;
    [SerializeField] private Transform RigBottomRight;
    [SerializeField] private Transform RigBottomLeft;
   
    [Header("UI")]
    [SerializeField] private TMP_Text HeaderText;

    [Header("Gen Prefabs")]
    //Move these to keep port localPositions simple. (0.5)
    [SerializeField] private Transform PortParentLeft;
    [SerializeField] private Transform PortParentRight;

    [SerializeField] private GameObject PortCapR;
    [SerializeField] private GameObject PortCapL;

    [Header("Settings")]
    public float minWidth = 1f;
    public float portScale;
    public float portGap;
    [Tooltip("Height of area when no ports at all.")]
    public float minPortAreaHeight = 0.2f;
    [Tooltip("Extra Port Area Padding")] public float portAreaRegularSize;
    public float minHeaderHeight = 0.5f;
    public float portYPosition = 0.25f;
    [Header("Configurationn")]
    public float width;
    [Min(0)]
    public int numLeftPorts;
    [Min(0)]
    public int numRightPorts;

    private float _drawnWidth;
    public void Redraw(){
        
        //Calculate Height needeed for Title
        //...search header string for line breaks?
        float requiredHeaderHeight = 0;//todo
        float headerHeight = Mathf.Max(requiredHeaderHeight, minHeaderHeight);
        
        //Calculate Height needeed for Ports
        int maxPorts = Mathf.Max(numLeftPorts, numRightPorts);
        float portHeight = maxPorts * portScale + ((maxPorts - 1) * portGap);
        portHeight = Mathf.Max(portHeight, minPortAreaHeight)+portAreaRegularSize;
        
        float realWidth = Mathf.Max(width, minWidth);
        float horizontalExtent = realWidth / 2;
        
        float zOffset = (headerHeight + portHeight)/2f;
        
        //calculatioins are in world space because of stupid Armature rotations from blender.
        //Slightly better if we use the transform.worldtolocal?
        
        RigTopLeft.position = transform.position + new Vector3(-horizontalExtent, 0, zOffset);
        RigTopRight.position = transform.position + new Vector3(horizontalExtent, 0, zOffset);
        canvasPos.position = transform.position + new Vector3(0, 0, zOffset);
        
        //mid is top-headerHeight.
        RigMidLeft.position = transform.position + new Vector3(-horizontalExtent, 0, zOffset - headerHeight);
        RigMidRight.position = transform.position + new Vector3(horizontalExtent, 0, zOffset - headerHeight);
        
        //bottom is mid-portHeight
        RigBottomLeft.position = transform.position + new Vector3(-horizontalExtent, 0, zOffset - headerHeight-portHeight);
        RigBottomRight.position = transform.position + new Vector3(horizontalExtent, 0, zOffset - headerHeight-portHeight);

        //Instantiate ports, parent appropritely, set to positions.
        
        //clear current port children.
        foreach (Transform child in PortParentLeft)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in PortParentRight)
        {
            Destroy(child.gameObject);
        }
        
        //Create new portsies
        float x=portScale/2f;
        for (int i = 0; i < numLeftPorts; i++)
        {
            var t =Instantiate(PortCapL, transform.position+new Vector3(-horizontalExtent, portYPosition, zOffset - headerHeight - x), Quaternion.identity,PortParentLeft);
            x += portScale+portGap;
            t.transform.localScale = new Vector3(portScale, portScale, portScale);
        }

        x = portScale / 2f;
        for (int i = 0; i < numRightPorts; i++)
        {
            var t = Instantiate(PortCapR, transform.position+new Vector3(horizontalExtent, portYPosition, zOffset - headerHeight - x), Quaternion.identity,
                PortParentRight);
            t.transform.localScale = new Vector3(portScale, portScale, portScale);
            x += (portScale+portGap);
        }

        _drawnWidth = width;
    }

    private void Update()
    {
        if (ConfigurationDidChange())
        {
            Redraw();
        }
    }

    private bool ConfigurationDidChange()
    {
        if (numLeftPorts != PortParentLeft.childCount || numRightPorts != PortParentRight.childCount)
        {
            return true;
        }

        return !Mathf.Approximately(_drawnWidth, width); 
    }
}
