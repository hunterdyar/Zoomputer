using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Zoompy;
using Zoompy.Extensions;
[CustomEditor(typeof(Zoompy.Generator.ComponentGenerator))]
public class ComponentGeneratorEditor : Editor
{
	public override VisualElement CreateInspectorGUI()
	{
		VisualElement container = new VisualElement();

		container.Add(new Label("Settings Reference"));
		var genSettingsProperty = serializedObject.FindProperty("_genSettings");
		var genSettingsElements = new PropertyField(genSettingsProperty);
		container.Add(genSettingsElements);
		
		container.Add(new Label("Inputs"));

		var numInputsProperty = serializedObject.FindProperty("numberInputs");
		var numInputsElements= new PropertyField(numInputsProperty);
		container.Add(numInputsElements);

		var numOutputsProperty = serializedObject.FindProperty("numberOutputs");
		var numOuputsElements = new PropertyField(numOutputsProperty);
		container.Add(numOuputsElements);

		var types = GetSignalHookImplementors();
		container.Add(new Label("Logic"));
		var props = types.Where(prop => Attribute.IsDefined(prop, typeof(Zoompy.LogicAttribute))).Select(p=>p.Name).ToList();
		var logicBaseClassNameProperty = serializedObject.FindProperty("baseLogicClassName");
		int index = props.IndexOf(logicBaseClassNameProperty.stringValue);
		if (index < 0)
		{
			//no correct name set yet.
			index = 0;
		}
		
		var logicNameDropdown = new DropdownField(props,index,
			Zoompy.Generator.ComponentGenerator.StripLogicSuffix,
			Zoompy.Generator.ComponentGenerator.StripLogicSuffix);
		logicNameDropdown.RegisterValueChangedCallback(e =>
		{	
			((Zoompy.Generator.ComponentGenerator)target).baseLogicClassName = e.newValue;
			serializedObject.ApplyModifiedProperties();
		});
		container.Add(logicNameDropdown);
		
		//material
		container.Add(new Label("Visuals"));
		var overrideMatProperty = serializedObject.FindProperty("_overrideContainerMaterial");
		var overrideMatElements = new PropertyField(overrideMatProperty);
		container.Add(overrideMatElements);

		container.Add(new Label("Debug"));
		var innerSystemProp = serializedObject.FindProperty("InnerSystem");
		var innserSystemElement = new PropertyField(innerSystemProp);
		container.Add(innserSystemElement);
		
		return container;
	}

	private IEnumerable<Type> GetSignalHookImplementors()
	{
		var asm = Assembly.GetAssembly(typeof(ISignalHook));
		var it = typeof(ISignalHook);
		return asm.GetLoadableTypes().Where(it.IsAssignableFrom).ToList();
	}
}