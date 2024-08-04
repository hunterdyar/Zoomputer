using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Zoompy.Generator.Editor.SystemGraph;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
	private SystemGraphEditor _graphEditor;
	private SystemGraphView _graphView;
	

	public void Configure(SystemGraphEditor editor, SystemGraphView view)
	{
		_graphEditor = editor;
		_graphView = view;
	}
	public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
	{
		List<SearchTreeEntry> tree = new List<SearchTreeEntry>()
		{
			new SearchTreeGroupEntry(new GUIContent("Logic"), 0),
			//todo: why does this create a node for the context? It could be a string or - better - the scriptableObject!
			AddNodeSearch("Nand",1,new SystemNodeView(Vector2.zero, _graphView.ComponentGenerator))
		};

		return tree;
	}

	private SearchTreeEntry AddNodeSearch(string name, int level, BaseNodeView baseNodeView)
	{
		SearchTreeEntry ste = new SearchTreeEntry(new GUIContent(name))
		{
			level = level,
			userData = baseNodeView
		};
		return ste;
	}
	
	public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
	{
		Vector2 mousePosition = _graphEditor.rootVisualElement.ChangeCoordinatesTo(
			_graphEditor.rootVisualElement.parent, context.screenMousePosition - _graphEditor.position.position
			);
		Vector2 graphMousePos = _graphView.contentViewContainer.WorldToLocal(mousePosition);

		return CheckForNodeType(SearchTreeEntry, graphMousePos);
		return false;
	}

	private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 pos)
	{
		switch (searchTreeEntry.userData)
		{
			case SystemNodeView:
				var node = _graphView.CreateSystemNodeView(pos);
				_graphView.AddElement(node);
				return true;
		}

		return false;
	}
}
