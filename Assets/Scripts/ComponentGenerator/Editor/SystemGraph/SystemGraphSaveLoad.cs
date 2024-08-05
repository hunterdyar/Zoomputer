using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemGraphSaveLoad
	{
		private SystemGraphEditor _editor;
		private SystemGraphView _graphView;

		public SystemGraphSaveLoad(SystemGraphEditor editor, SystemGraphView graphView)
		{
			_editor = editor;
			_graphView = graphView;
		}
		
		//save all
		public void Save(ComponentGenerator gen)
		{
			//make sure positions and such are updated.
			//this is because I'm too lazy to save on dragging around with an event handler.
			foreach (var node in _graphView.nodes)
			{
				if (node is BaseNodeView nodeView)
				{
					nodeView.PreSaveDataPopulate();
				}
			}
			
			SaveInputOutput(gen);
			SaveNodes(gen);
			SaveEdges(gen);

			//save bounds
			gen.InnerSystem.Bounds = _graphView.GetAllNodesBounds();
			
			EditorUtility.SetDirty(gen);
			AssetDatabase.SaveAssetIfDirty(gen);
		}

		private void SaveEdges(ComponentGenerator gen)
		{
			gen.InnerSystem.Edges = _graphView.edges.Where(e => e.input.node != null && e.output.node != null)
				.Select(e => { return GraphEdgeToSystemEdge(e); }).ToArray();
		}

		private void SaveInputOutput(ComponentGenerator gen)
		{
			var inputNodeBase = _graphView.nodes.First(e => e is SystemInputNodeView);
			gen.InnerSystem.Input = ((inputNodeBase as BaseNodeView)!).Guid;
			gen.InnerSystem.InputPos = inputNodeBase.GetPosition();
			var outputNodeBase = _graphView.nodes.First(e => e is SystemOutputNodeView);
			gen.InnerSystem.Output = ((outputNodeBase as BaseNodeView)!).Guid;
			gen.InnerSystem.OutputPos = outputNodeBase.GetPosition();
		}
		private void SaveNodes(ComponentGenerator gen)
		{
			gen.InnerSystem.Nodes = _graphView.nodes.Select(x => x as SystemNodeView).Where(x => x != null)
				.Select(n => n.SystemNode).ToArray();
		}

		private SystemEdge GraphEdgeToSystemEdge(Edge edge)
		{
			if (edge.input.node is BaseNodeView input)
			{
				if (edge.output.node is BaseNodeView output)
				{
					return new SystemEdge()
					{
						ToNode = input.Guid,
						ToIndex = edge.input.parent.IndexOf(edge.input),

						FromNode = output.Guid,
						FromIndex = edge.output.parent.IndexOf(edge.output)
					};
				}
			}
			return null;
		}
	}
}