using Dalssoft.DiagramNet;
using Diagram.NET.element;
using RaspiImporter;
using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RaspiImporter.VisualElements
{
    [Serializable]
	public class GenericElement
	{

		const int itemHeight = 16;
		const int itemWidth = 96;

		const int padding = 5;

		const int containerWidth = 256;
		const int containerHeight = 128;

		const int labelHeight = 40;
        const int labelNameHeight = 24;
        const int labelClassHeight = 16;

        static Dictionary<Type, int> itemsPerType = new Dictionary<Type, int>();

		public static Dalssoft.DiagramNet.BaseElement GetGenericElement(RaspiImporter.BaseElement Element)
		{
			Type elemType = Element.GetType();
			int cuantos = itemsPerType.ContainsKey(elemType) ? itemsPerType[elemType] : 0;

			var old = Element;

			Element = (RaspiImporter.BaseElement)Activator.CreateInstance(Element.GetType());

			Element.Name = old.Name + (++cuantos);
			Element.ClassType = old.ClassType;
			Element.InternalClassName = old.InternalClassName;

			itemsPerType[elemType] = cuantos;

			List<ElementInputType> inputs = new List<ElementInputType>();
			List<ElementOutputType> outputs = new List<ElementOutputType>();
			List<BaseElementFunctionInfo> functions = new List<BaseElementFunctionInfo>();
			List<ElementPropertyType> props = new List<ElementPropertyType>();

			foreach (var input in old.Inputs)
			{
				inputs.Add(new ElementInputType { InputName = input.InputName, InputType = input.InputType, Parent = Element });
			}
			foreach (var output in old.Outputs)
				outputs.Add(new ElementOutputType { OutputName = output.OutputName, OutputType = output.OutputType, Parent = Element });

			foreach (var func in old.Functions)
			{
				var newFunc = (BaseElementFunctionInfo)Activator.CreateInstance(func.GetType());
				newFunc.Parent = Element;
				newFunc.FunctionName = func.FunctionName;
				functions.Add(newFunc);
			}

			foreach (var prop in old.Properties)
				props.Add(new ElementPropertyType {  PropertyName = prop.PropertyName, PropertyType = prop.PropertyType, Parent = Element });

			Element.Inputs = inputs.ToArray();
			Element.Outputs = outputs.ToArray();
			Element.Functions = functions.ToArray();
			Element.Properties = props.ToArray();

			int addedCols = 0;//(int)Math.Ceiling(Element.Properties.Length / 2F);

			int sInputs = (Element.Inputs.Length + addedCols) * (itemHeight + padding) + padding;
			int sOutputs = (Element.Outputs.Length + addedCols) * (itemHeight + padding) + padding;
			//int sProperties = Element.Properties.Length * (itemHeight + padding) + padding;



			int maxSize = Math.Max(sOutputs, sInputs);

			RectangleGroup group = new RectangleGroup(0, 0, containerWidth, maxSize + labelHeight);
            group.FillColor1 = Color.FromArgb(255,80,80,80);
			group.FillColor2 = Color.DarkGray;

			LabelElement elem = new LabelElement(0, 0, containerWidth, labelNameHeight);
			elem.Text = Element.Name;
            elem.ForeColor1 = Color.White;
			elem.Font = new Font(new FontFamily("Arial"), 12, FontStyle.Bold);

			group.Add("label", elem);

            LabelElement elemC = new LabelElement(0, labelNameHeight, containerWidth, 16);
            elemC.Text = Element.ClassName;
            elemC.ForeColor1 = Color.White;
            elemC.Font = new Font(new FontFamily("Arial"), 8, FontStyle.Bold);

            group.Add("class", elemC);

            int sPos = sInputs + addedCols >= maxSize ? padding + labelHeight : ((maxSize - sInputs) / 2) + labelHeight;

			foreach (var input in Element.Inputs)
			{

				RectangleNode node = new RectangleNode(sPos, padding, itemWidth, itemHeight);
				node.Label.Text = input.InputName;
				node.Label.Font = new Font(node.Label.Font.FontFamily, 7);

                node.FillColor1 = input.InputType == IOType.Integer ? Color.Coral : input.InputType == IOType.Buffer ? Color.FromArgb(255, 125, 229, 255) : input.InputType == IOType.Byte ? Color.Yellow : Color.GreenYellow;
                node.FillColor2 = Color.Empty;
				node.Tag = input;

				group.Add(input.InputName, node);

				sPos += itemHeight + padding;
			}

			sPos = sOutputs + addedCols >= maxSize ? padding + labelHeight : ((maxSize - sOutputs) / 2) + labelHeight;

			foreach (var output in Element.Outputs)
			{

				RectangleNode node = new RectangleNode(sPos, containerWidth - (itemWidth + padding), itemWidth, itemHeight);
				node.Label.Text = output.OutputName;
				node.Label.Font = new Font(node.Label.Font.FontFamily, 7);
				node.FillColor1 = output.OutputType == IOType.Integer ? Color.Coral : output.OutputType == IOType.Buffer ? Color.FromArgb(255, 125, 229, 255) : output.OutputType == IOType.Byte ? Color.Yellow : Color.GreenYellow;
                node.FillColor2 = Color.Empty;
				node.Tag = output;

				group.Add(output.OutputName, node);

				sPos += itemHeight + padding;
			
			}

			/*

			sPos = (maxSize - (addedCols - 1) * (itemHeight + padding)) + padding / 2;
			bool left = true;

			foreach (var prop in Element.Properties)
			{

				RectangleNode node = new RectangleNode(sPos, left ? padding : containerWidth - (itemWidth + padding), itemWidth, itemHeight);
				node.Label.Text = prop.PropertyName;
				node.Label.Font = new Font(node.Label.Font.FontFamily, 7);
				node.FillColor1 = Color.NavajoWhite;//peachpuff
				node.FillColor2 = Color.Empty;
				node.Tag = prop;

				group.Add(prop.PropertyName, node);

				if(!left)
					sPos += itemHeight + padding;

				left = !left;

			}
			*/

			group.Tag = Element;

			Element.NameChanged += (o, e) => {

				if (Element.ClassType == typeof(RaspBuffer))
				{
					BufferConverter.bufferList.Remove(elem.Text);
					BufferConverter.bufferList.Add(Element.Name, Element);
				}
				
				elem.Text = Element.Name;
				
			
			};

			if (Element.ClassType == typeof(RaspBuffer))
				BufferConverter.bufferList.Add(Element.Name, Element);
			
			group.RemovedFromDocument += (o, e) => { 
			
				
				if (Element.ClassType == typeof(RaspBuffer))
					BufferConverter.bufferList.Remove(elem.Text);
			
			};
            
            return group;

		}

	}
}


