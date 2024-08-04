using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ComponentGenerator.Editor.SystemGraph
{
	public class NodeView : UnityEditor.Experimental.GraphView.Node
	{
        
		public Action<NodeView> OnNodeSelected;
        public Node node;
        public Port input;
        public Port output;

        public NodeView(Node node)
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.viewDataKey;
            //
            // style.left = node.position.x;
            // style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }
        private void CreateInputPorts()
        {
            
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));


            if (input != null)
            {
                input.portName = "In";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            // switch (node)
            // {
            //     case ActionNode:
            //         break;
            //     case CompositeNode:
            //         output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            //         break;
            //     case DecoratorNode:
            //         output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            //         break;
            //     case RootNode:
            //         output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            //         break;
            // }
            //
            // if (output != null)
            // {
            //     output.portName = "Out";
            //     outputContainer.Add(output);
            // }
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