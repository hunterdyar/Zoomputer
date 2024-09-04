using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This object is attached to the Panel Prefab
public class PanelModelController : MonoBehaviour
{
    [Header("Rig Transforms")]
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

    [Header("Settings")] public float minWidth = 1f;
    public float portScale;
    public float portGap;
    public float minHeaderHeight = 0.5f;
    
    [Header("Configurationn")]
    public float width;
    public int numLeftPorts;
    public int numRightPorts;
    
    public void Redraw(){
        //Calculate Height needeed for Title
        //...search header string for line breaks?
        float requiredHeaderHeight = 0;//todo
        float headerHeight = Mathf.Max(requiredHeaderHeight, minHeaderHeight);
        
        //Calculate Height needeed for Ports
        int maxPorts = Mathf.Max(numLeftPorts, numRightPorts);
        float portHeight = maxPorts * portScale + ((maxPorts - 1) * portGap);
       
        //top is 0
        //mid is top-headerHeight.
        //bottom is mid-portHeight
        //Adjust the top, mid, bottom transforms.
        
        //Instantiate ports, parentn appropritely, set to positions.
    }
}
