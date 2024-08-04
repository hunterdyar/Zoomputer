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
			
			AddNodes();
        }

        private void AddSearchWindow()
        {
	        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
	        _searchWindow.Configure(_editor,this);
	        nodeCreationRequest = context =>
		        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
	        
        }
        void AddNodes()
        {
	        if (_systemParent == null)
	        {
		        return;
	        }

	        var inputs = new SystemInputNodeView(ComponentGenerator);
	        AddElement(inputs);

	        var outputs = new SystemOutputNodeView(ComponentGenerator);
	        AddElement(outputs);

	        foreach (var sNode in _systemParent.InnerSystem.Nodes)
	        {
		        var node = RecreateSystemNodeView(sNode);
	        }

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
	        ComponentGenerator.InnerSystem.Nodes.Add(systemNode);
	        return node;
        }

        public SystemNodeView RecreateSystemNodeView(SystemNode node)
        {
	        var n = new SystemNodeView(node, ComponentGenerator);
	        return n;
        }  
	}
}