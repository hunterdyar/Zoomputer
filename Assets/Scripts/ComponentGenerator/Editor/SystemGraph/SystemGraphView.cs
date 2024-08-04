using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemGraphView : GraphView
	{
		private SystemGraphEditor _editor;
		private string styleName = "";
        ComponentGenerator _systemParent;
        public SystemDescription System;
    
        public SystemGraphView(SystemGraphEditor graphEditorWindow)
        {
	        _editor = graphEditorWindow;

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

			// StyleSheet styleSheet = PackageSaveAssetLoading.GetUSSAsset();
			// styleSheets.Add(styleSheet);
        }
	}
}