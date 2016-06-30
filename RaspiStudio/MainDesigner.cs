using Dalssoft.DiagramNet;
using Diagram.NET.element;
using Newtonsoft.Json;
using RaspiImporter;
using RaspiImporter.VisualElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaspiImporter
{
	public partial class MainDesigner : Form
	{

		ElementImporter importer = new ElementImporter();

		Dictionary<string, BaseElementLink> linkedIOS = new Dictionary<string, BaseElementLink>();
		List<RaspiImporter.BaseElement> elements = new List<RaspiImporter.BaseElement>();

		public MainDesigner()
		{
			InitializeComponent();

			foreach (var key in importer.Keys)
			{ 
			
				var item = importer[key];

				TreeNode node = new TreeNode(key);
				node.ImageKey = node.SelectedImageKey = "folder_brick.png";
				foreach (var subitem in item)
				{

					var nnode = node.Nodes.Add(subitem.ClassType.Name);
					nnode.ImageKey = nnode.SelectedImageKey = "brick.png";
					nnode.Tag = subitem;
				
				}

				tvElementTypes.Nodes.Add(node);
			
			}

			designer1.ElementSelection += designer1_ElementSelection;
			designer1.Document.Action = DesignerAction.Connect;
			designer1.ElementsWillLink += designer1_ElementsWillConnect;
			designer1.WillDelete += designer1_WillDelete;
			
		}

		void designer1_WillDelete(object sender, EventArgs e)
		{
			if (sender is RectangleGroup)
				DeleteItem(sender as RectangleGroup);
			else
				DelteLink(sender as BaseLinkElement);
		}

		private void DelteLink(BaseLinkElement baseLinkElement)
		{
			string toDelete = null;
			foreach (var v in linkedIOS)
			{

				if (v.Value == baseLinkElement.Tag as BaseElementLink)
				{

					toDelete = v.Key;
					break;
				
				}
			
			}

			if(toDelete != null)
				linkedIOS.Remove(toDelete);
		}

		private void DeleteItem(RectangleGroup rectangleGroup)
		{
			var item = rectangleGroup.Tag as RaspiImporter.BaseElement;

			List<string> linksToDelete = new List<string>();

			foreach (var link in linkedIOS)
			{

				if (link.Value.Input.Parent == item || link.Value.Output.Parent == item)
					linksToDelete.Add(link.Key);
			
			}

			foreach (var v in linksToDelete)
				linkedIOS.Remove(v);
		}

		void designer1_ElementsWillConnect(object sender, Designer.ElementsLinkedArgs e)
		{

			NodeElement node1 = e.Element1 as NodeElement;
			NodeElement node2 = e.Element2 as NodeElement;

			ConnectorElement connector1 = e.Connector1;
			ConnectorElement connector2 = e.Connector2;

			
			ElementInputType input = null;
			ElementOutputType output = null;

			bool inputOnRight = false;
			bool outputOnRight = false;

			BaseLinkElement outputLink = null;

			if (e.Element1.Tag is ElementInputType)
			{
				inputOnRight = node1.Connectors[1] == connector1;
				input = (ElementInputType)e.Element1.Tag;
			}

			if (e.Element2.Tag is ElementInputType)
			{
				inputOnRight = node2.Connectors[1] == connector2;
				input = (ElementInputType)e.Element2.Tag;
			}

			if (e.Element1.Tag is ElementOutputType)
			{
				outputOnRight = node1.Connectors[1] == connector1;
				output = (ElementOutputType)e.Element1.Tag;
			}

			if (e.Element2.Tag is ElementOutputType)
			{
				outputOnRight = node2.Connectors[1] == connector2;
				output = (ElementOutputType)e.Element2.Tag;
			}

			if (input == null)
			{

				e.Cancel = true;
				MessageBox.Show("Cannot connect two outputs");
				return;
			
			}

			if (output == null)
			{

				e.Cancel = true;
				MessageBox.Show("Cannot connect two inputs");
				return;

			}

			if (input.InputType != output.OutputType)
			{

				e.Cancel = true;
				MessageBox.Show("Cannot connect an input of type " + input.InputType + " to an output of type " + output.OutputType);
				return;		
			
			}

			Guid g = Guid.NewGuid();

			var lnk = new BaseElementLink { Input = input, InputOnLeft = !inputOnRight,  Output = output, OutputOnLeft = !outputOnRight };

			linkedIOS.Add(g.ToString(), lnk);

			e.NewLink.Tag = lnk;

		}

		private void tvElementTypes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag != null)
				pgSelectedElement.SelectedObject = e.Node.Tag;
		}

		private void tvElementTypes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag != null)
			{
				var genElem = GenericElement.GetGenericElement(e.Node.Tag as RaspiImporter.BaseElement);
				designer1.Document.AddElement(genElem);
				elements.Add(genElem.Tag as RaspiImporter.BaseElement);
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			designer1.Document.Zoom *= 1.1f;
			
		}

		void designer1_ElementSelection(object sender, ElementSelectionEventArgs e)
		{
			pgSelectedElement.SelectedObjects = e.Elements.Cast<Dalssoft.DiagramNet.BaseElement>().Where(es => es.Tag != null).Select(es => es.Tag).ToArray();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			designer1.Document.Zoom /= 1.1f;
		}

		private void pgSelectedElement_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{

		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			SaveFileDialog sf = new SaveFileDialog();
			sf.Filter = "RaspiDevice Files (*.rd) | *.rd";

			if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{ 
				var file = sf.OpenFile();

				RaspiDeviceFile rd = new RaspiDeviceFile
				{
					Elements = elements.ToArray(),
					Links = linkedIOS.Values.Select(li => new RaspLink
					{
						InputDevice = li.Input.Parent.Name,
						InputName = li.Input.InputName,
						InputLinkOnLeft = li.InputOnLeft,
						OutputDevice = li.Output.Parent.Name,
						OutputName = li.Output.OutputName,
						OutputLinkOnLeft = li.OutputOnLeft

					}).ToArray(),
					Positions = designer1.Document.Elements
					.Cast<Dalssoft.DiagramNet.BaseElement>()
					.Where(ed => ed is RectangleGroup)
					.Select(eds => new RaspPosition { ElementName = ((eds as RectangleGroup).Tag as RaspiImporter.BaseElement).Name, X = eds.Location.X, Y = eds.Location.Y })
					.ToArray()

				};

				StreamWriter sw = new StreamWriter(file);
				sw.Write(importer.SerializeDeviceFile(rd));
				sw.Close();
			
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog();
			of.Filter = "RaspiDevice Files (*.rd) | *.rd";

			if (of.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				RaspiDeviceFile data = importer.DeserializeDeviceFile(File.ReadAllText(of.FileName));

				foreach (var v in designer1.Document.Elements.OfType<Dalssoft.DiagramNet.BaseElement>().ToArray())
					designer1.Document.DeleteElement(v);

				linkedIOS.Clear();
				elements.Clear();

				Dictionary<string, Dalssoft.DiagramNet.BaseElement> groups = new Dictionary<string, Dalssoft.DiagramNet.BaseElement>();

				foreach (var item in data.Elements)
				{

					var elem = importer.ElementsByType[item.InternalClassName];
					var rect = GenericElement.GetGenericElement(elem);
					elem = rect.Tag as RaspiImporter.BaseElement;
					elem.Name = item.Name;
					groups.Add(elem.Name, rect);
					elements.Add(rect.Tag as RaspiImporter.BaseElement);
					designer1.Document.AddElement(rect);

					var props = item.GetType().GetProperties();

					foreach (var prop in props)
					{

						prop.SetValue(elem, prop.GetValue(item));
					
					}

				}

				foreach (var pos in data.Positions)
					groups[pos.ElementName].Location = new Point(pos.X, pos.Y);

				foreach (var link in data.Links)
				{

					var inputElement = groups[link.InputDevice] as RectangleGroup;
					var outputElement = groups[link.OutputDevice] as RectangleGroup;

					var input = (inputElement.Tag as RaspiImporter.BaseElement).Inputs.Where(i => i.InputName == link.InputName).FirstOrDefault();
					var output = (outputElement.Tag as RaspiImporter.BaseElement).Outputs.Where(o => o.OutputName == link.OutputName).FirstOrDefault();
				
					var inputLinkNode = (inputElement.Children[input.InputName] as NodeElement).Connectors[link.InputLinkOnLeft ?  1 : 0];
					var outputLinkNode = (outputElement.Children[output.OutputName] as NodeElement).Connectors[link.OutputLinkOnLeft ? 1 : 0];

					var lnk = designer1.Document.AddLink(inputLinkNode, outputLinkNode);

					Guid g = Guid.NewGuid();

					var lnkI = new BaseElementLink { Input = input, Output = output };

					linkedIOS.Add(g.ToString(), lnkI);

					lnk.Tag = lnkI;

				}
			
			}
		}
	}
}
