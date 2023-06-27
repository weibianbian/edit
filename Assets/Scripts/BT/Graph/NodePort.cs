using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System.Linq.Expressions;
using System;
using GraphProcessor;

namespace BT.Graph
{
	public class PortData : IEquatable< PortData >
	{
		public string	identifier;
		public string	displayName;
		public Type		displayType;
		public bool		acceptMultipleEdges;
		public int		sizeInPixel;
		public string	tooltip;
		public bool		vertical;

        public bool Equals(PortData other)
        {
			return identifier == other.identifier
				&& displayName == other.displayName
				&& displayType == other.displayType
				&& acceptMultipleEdges == other.acceptMultipleEdges
				&& sizeInPixel == other.sizeInPixel
				&& tooltip == other.tooltip
				&& vertical == other.vertical;
        }

		public void CopyFrom(PortData other)
		{
			identifier = other.identifier;
			displayName = other.displayName;
			displayType = other.displayType;
			acceptMultipleEdges = other.acceptMultipleEdges;
			sizeInPixel = other.sizeInPixel;
			tooltip = other.tooltip;
			vertical = other.vertical;
		}
    }
	public class NodePort
	{
		public string				fieldName;
		public BehaviorGraphNode owner;
		public FieldInfo			fieldInfo;
		public PortData				portData;
		List< SerializableEdge >	edges = new List< SerializableEdge >();
		Dictionary< SerializableEdge, PushDataDelegate >	pushDataDelegates = new Dictionary< SerializableEdge, PushDataDelegate >();
		List< SerializableEdge >	edgeWithRemoteCustomIO = new List< SerializableEdge >();
		public object				fieldOwner;

		public delegate void PushDataDelegate();

		public NodePort(BehaviorGraphNode owner, string fieldName, PortData portData) : this(owner, owner, fieldName, portData) {}

		public NodePort(BehaviorGraphNode owner, object fieldOwner, string fieldName, PortData portData)
		{
			this.fieldName = fieldName;
			this.owner     = owner;
			this.portData  = portData;
			this.fieldOwner = fieldOwner;

			fieldInfo = fieldOwner.GetType().GetField(
				fieldName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			//customPortIOMethod = CustomPortIO.GetCustomPortMethod(owner.GetType(), fieldName);
		}

		/// <summary>
		/// Connect an edge to this port
		/// </summary>
		/// <param name="edge"></param>
		public void Add(SerializableEdge edge)
		{
			if (!edges.Contains(edge))
				edges.Add(edge);
			PushDataDelegate edgeDelegate = CreatePushDataDelegateForEdge(edge);

			if (edgeDelegate != null)
				pushDataDelegates[edge] = edgeDelegate;
		}

		PushDataDelegate CreatePushDataDelegateForEdge(SerializableEdge edge)
		{
			try
			{
				//Creation of the delegate to move the data from the input node to the output node:
				FieldInfo inputField = edge.inputNode.GetType().GetField(edge.inputFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				FieldInfo outputField = edge.outputNode.GetType().GetField(edge.outputFieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				Type inType, outType;
				Expression inputParamField = Expression.Field(Expression.Constant(edge.inputNode), inputField);
				Expression outputParamField = Expression.Field(Expression.Constant(edge.outputNode), outputField);
				inType = edge.inputPort.portData.displayType ?? inputField.FieldType;
				outType = edge.outputPort.portData.displayType ?? outputField.FieldType;

				// If there is a user defined convertion function, then we call it
				if (TypeAdapter.AreAssignable(outType, inType))
				{
					// We add a cast in case there we're calling the conversion method with a base class parameter (like object)
					var convertedParam = Expression.Convert(outputParamField, outType);
					outputParamField = Expression.Call(TypeAdapter.GetConvertionMethod(outType, inType), convertedParam);
					// In case there is a custom port behavior in the output, then we need to re-cast to the base type because
					// the convertion method return type is not always assignable directly:
					outputParamField = Expression.Convert(outputParamField, inputField.FieldType);
				}
				else // otherwise we cast
					outputParamField = Expression.Convert(outputParamField, inputField.FieldType);

				BinaryExpression assign = Expression.Assign(inputParamField, outputParamField);
				return Expression.Lambda< PushDataDelegate >(assign).Compile();
			} catch (Exception e) {
				Debug.LogError(e);
				return null;
			}
		}

		/// <summary>
		/// Disconnect an Edge from this port
		/// </summary>
		/// <param name="edge"></param>
		public void Remove(SerializableEdge edge)
		{
			if (!edges.Contains(edge))
				return;

			pushDataDelegates.Remove(edge);
			edgeWithRemoteCustomIO.Remove(edge);
			edges.Remove(edge);
		}

		/// <summary>
		/// Get all the edges connected to this port
		/// </summary>
		/// <returns></returns>
		public List< SerializableEdge > GetEdges() => edges;

		/// <summary>
		/// Push the value of the port through the edges
		/// This method can only be called on output ports
		/// </summary>
		public void PushData()
		{
			foreach (var pushDataDelegate in pushDataDelegates)
				pushDataDelegate.Value();

			if (edgeWithRemoteCustomIO.Count == 0)
				return ;

			//if there are custom IO implementation on the other ports, they'll need our value in the passThrough buffer
			object ourValue = fieldInfo.GetValue(fieldOwner);
			foreach (var edge in edgeWithRemoteCustomIO)
				edge.passThroughBuffer = ourValue;
		}

		/// <summary>
		/// Reset the value of the field to default if possible
		/// </summary>
		public void ResetToDefault()
		{
			// Clear lists, set classes to null and struct to default value.
			if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
				(fieldInfo.GetValue(fieldOwner) as IList)?.Clear();
			else if (fieldInfo.FieldType.GetTypeInfo().IsClass)
				fieldInfo.SetValue(fieldOwner, null);
			else
			{
				try
				{
					fieldInfo.SetValue(fieldOwner, Activator.CreateInstance(fieldInfo.FieldType));
				} catch {} // Catch types that don't have any constructors
			}
		}
		public void PullData()
		{
			// check if this port have connection to ports that have custom output functions
			if (edgeWithRemoteCustomIO.Count == 0)
				return ;

			// Only one input connection is handled by this code, if you want to
			// take multiple inputs, you must create a custom input function see CustomPortsNode.cs
			if (edges.Count > 0)
			{
				var passThroughObject = edges.First().passThroughBuffer;

				// We do an extra convertion step in case the buffer output is not compatible with the input port
				if (passThroughObject != null)
					if (TypeAdapter.AreAssignable(fieldInfo.FieldType, passThroughObject.GetType()))
						passThroughObject = TypeAdapter.Convert(passThroughObject, fieldInfo.FieldType);

				fieldInfo.SetValue(fieldOwner, passThroughObject);
			}
		}
	}
	
	public abstract class NodePortContainer : List< NodePort >
	{
		protected BehaviorGraphNode node;

		public NodePortContainer(BehaviorGraphNode node)
		{
			this.node = node;
		}

		public void Remove(SerializableEdge edge)
		{
			ForEach(p => p.Remove(edge));
		}

		public void Add(SerializableEdge edge)
		{
			string portFieldName = (edge.inputNode == node) ? edge.inputFieldName : edge.outputFieldName;
			string portIdentifier = (edge.inputNode == node) ? edge.inputPortIdentifier : edge.outputPortIdentifier;

			if (String.IsNullOrEmpty(portIdentifier))
				portIdentifier = null;

			var port = this.FirstOrDefault(p =>
			{
				return p.fieldName == portFieldName && p.portData.identifier == portIdentifier;
			});

			if (port == null)
			{
				Debug.LogError("The edge can't be properly connected because it's ports can't be found");
				return;
			}

			port.Add(edge);
		}
	}

	public class NodeInputPortContainer : NodePortContainer
	{
		public NodeInputPortContainer(BehaviorGraphNode node) : base(node) {}

		public void PullDatas()
		{
			ForEach(p => p.PullData());
		}
	}

	public class NodeOutputPortContainer : NodePortContainer
	{
		public NodeOutputPortContainer(BehaviorGraphNode node) : base(node) {}

		public void PushDatas()
		{
			ForEach(p => p.PushData());
		}
	}
}