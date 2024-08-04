using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zoompy.Generator.Editor.SystemGraph
{
	public class SystemGraphEditor : EditorWindow
	{
		private ComponentGenerator _currentComponentContainer;
		private SystemGraphView _graphView;

		//toolbar
		private Toolbar _toolbar;
		private Label _systemNameLabel;
		
		[OnOpenAsset(1)]
		public static bool ShowWindow(int instanceID, int line)
		{
			UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceID);
			if (item is ComponentGenerator)
			{
				SystemGraphEditor window = (SystemGraphEditor)GetWindow(typeof(SystemGraphEditor));
				window.titleContent = new GUIContent("Component System Editor");
				window._currentComponentContainer = item as ComponentGenerator;
				window.minSize = new Vector2(150, 150);
				window.Load();
			}

			return false;
		}

		private void OnEnable()
		{
			ConstructGraphView();
			GenerateToolbar();
			Load();               
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_graphView);
		}

		private void GenerateToolbar()
		{
			_toolbar = new Toolbar();
			Button saveBtn = new Button()
			{
				text = "Save",
			};
			saveBtn.clicked += () => { Save(); };
			_toolbar.Add(saveBtn);
			if (_systemNameLabel == null)
			{
				_systemNameLabel = new Label();
			}
			_toolbar.Add(_systemNameLabel);
			rootVisualElement.Add(_toolbar); 
		}

		void Load()
		{
			if (_systemNameLabel == null)
			{
				_systemNameLabel = new Label();
			}

			//
			if (_currentComponentContainer != null)
			{
				_systemNameLabel.text = _currentComponentContainer.name;
			}
		}
		void Save()
		{
			
		}

		private void ConstructGraphView()
		{
			_graphView = new SystemGraphView(this);
			rootVisualElement.Add(_graphView);
			_graphView.StretchToParentSize();
			_graphView.style.width = new StyleLength(Length.Percent(100));
			_graphView.style.height = new StyleLength(Length.Percent(100));
		}
		
	}
}