using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemGraphView : GraphView
	{
		private SystemGraphEditor _editor;
		private string styleName = "";

		private NodeSearchWindow _searchWindow;
		public ComponentGenerator ComponentGenerator => _systemParent;
        ComponentGenerator _systemParent;
        public SystemDescription System;

        private SystemInputNodeView _inputsNodeView;
        private SystemOutputNodeView _outputsNodeView;
        public SystemGraphView(ComponentGenerator parent, SystemGraphEditor graphEditorWindow)
        {
	        _editor = graphEditorWindow;
	        if (parent == null)
	        {
		        Debug.LogWarning("fuck");
	        }
	        
	        _systemParent = parent;
	        if (!string.IsNullOrEmpty(styleName))
	        {
		        StyleSheet style = Resources.Load<StyleSheet>(styleName);
		        styleSheets.Add(style);
	        }

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
			this.AddManipulator(new EdgeManipulator());
			
			GridBackground grid = new GridBackground();
			Insert(0,grid);
			grid.StretchToParentSize();

			AddSearchWindow();
			
			LoadNodeViewsFromData();
        }

        private void AddSearchWindow()
        {
	        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
	        _searchWindow.Configure(_editor,this);
	        nodeCreationRequest = context =>
		        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        /// <summary>
        /// Creates the graph from serialized data.
        /// </summary>
        void LoadNodeViewsFromData()
        {
	        if (_systemParent == null)
	        {
		        return;
	        }

	        _inputsNodeView = new SystemInputNodeView(ComponentGenerator);
	        _inputsNodeView.SetID(_systemParent.InnerSystem.Input);
	        _inputsNodeView.SetPosition(_systemParent.InnerSystem.InputPos);
	        AddElement(_inputsNodeView);

			_outputsNodeView= new SystemOutputNodeView(ComponentGenerator);
			_outputsNodeView.SetID(_systemParent.InnerSystem.Output);
			_outputsNodeView.SetPosition(_systemParent.InnerSystem.OutputPos);
	        AddElement(_outputsNodeView);

	        foreach (var sNode in _systemParent.InnerSystem.Nodes)
	        {
		        var node = RecreateSystemNodeView(sNode);
		        AddElement(node);
	        }

	        // FromNode = input.Guid,
	        // ToNode = output.Guid,
	        // FromIndex = edge.input.parent.IndexOf(edge.input),
	        // ToIndex = edge.output.parent.IndexOf(edge.output)
	        foreach (var edge in _systemParent.InnerSystem.Edges)
	        {
		        var from = GetNode(edge.FromNode);
		        var to = GetNode(edge.ToNode);
		        if (from != null && to != null)
		        {
			        var fromPort = from.Query<Port>().Where(p=>p.direction == Direction.Output).AtIndex(edge.FromIndex);
			        var toPort = to.Query<Port>().Where(p => p.direction == Direction.Input).AtIndex(edge.ToIndex);
			        if (fromPort == null || toPort == null)
			        {
				        Debug.LogWarning("Bad edge data. reopen the window and re-save the graph, please.");
				        return;
			        }
			        var e = fromPort.ConnectTo(toPort);
			        if (e != null)
			        {
				        AddElement(e);
			        }
		        }
	        }
        }

        public BaseNodeView GetNode(string guid)
        {
	        //todo:		not sure why this workaround is needed. the nodes list should have input and output that extend from baseNodeview.
	        if (_systemParent.InnerSystem.Input == guid)
	        {
		        return _inputsNodeView;
	        }else if (_systemParent.InnerSystem.Output == guid)
	        {
		        return _outputsNodeView;
	        }
	        
	        return nodes.Select(x => { return (x as BaseNodeView); }).Where(x => x != null).First(x => x.Guid == guid);
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
	        List<Port> compatiblePorts = new List<Port>();
	        foreach (var port in ports)
	        {
		        if (startPort.node != port.node && startPort.direction != port.direction)
		        {
			        compatiblePorts.Add(port);
		        }
	        }

	        return compatiblePorts;
        }

        public SystemNodeView CreateNewSystemNodeView(Vector2 pos, ComponentGenerator system)
        {
	        //create the data.
	        var systemNode = new SystemNode();
	        systemNode.System = system;
	        systemNode.NodeID = Guid.NewGuid().ToString();

	        //for size
	        systemNode.Position = pos;
	        systemNode.Size = new Vector2(300, 250);//todo: this conflates with default node size
	        
	        var node = new SystemNodeView(systemNode, ComponentGenerator);
	       // This happens when we save.
	       // ComponentGenerator.InnerSystem.Nodes.Add(systemNode);
	        return node;
        }

        public SystemNodeView RecreateSystemNodeView(SystemNode node)
        {
	        var n = new SystemNodeView(node, ComponentGenerator);
	        return n;
        }  
	}
}