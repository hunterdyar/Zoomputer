using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ComponentGenerator.Editor.SystemGraph
{
	public class SystemGraphEditor : EditorWindow
	{
		[MenuItem("Tools/BehaviourTree Editor")]
		public static void OpenWindow()
		{
			SystemGraphEditor window = GetWindow<SystemGraphEditor>();
			window.titleContent = new GUIContent("System Editor");
		}

		public void CreateGUI()
		{
			// Each editor window contains a root VisualElement object
			VisualElement root = rootVisualElement;

			VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ComponentGenerator/SystemTree.uxml");
			var ui = uiAsset.CloneTree();
			root.Add(ui);
			//var styleSheet = PackageSaveAssetLoading.GetUSSAsset();
			//root.styleSheets.Add(styleSheet);

			// treeView = root.Q<BehaviourTreeView>();
			// inspectorView = root.Q<InspectorView>();
			// treeView.OnNodeSelected = OnNodeSelectionChanged;
			// OnSelectionChange();
		}
	}
}