using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class BaseNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public string Guid => guid;
        protected string guid;
        
        protected SystemGraphView _graphView;
        protected SystemGraphEditor _graphEditor;
        
        protected Vector2 defaultNodeSize = new Vector2(200, 250);
        
		public Action<BaseNodeView> OnNodeSelected;
        protected ComponentGenerator _parent;
        public BaseNodeView(ComponentGenerator parent)
        {
            _parent = parent;
            // style.left = node.position.x;
            // style.top = node.position.y;
            guid = GUID.Generate().ToString();
        }

        public Port AddPort(string name, Direction nodeDir, Port.Capacity capacity = Port.Capacity.Single)
        {
             var port = InstantiatePort(Orientation.Horizontal, nodeDir, capacity, typeof(byte));
             port.name = name;
             if (nodeDir == Direction.Output)
             {
                 outputContainer.Add(port);
             }
             else if(nodeDir == Direction.Input)
             {
                 inputContainer.Add(port);
             }

             return port;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            //node.position = newPos.position;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
	}
}