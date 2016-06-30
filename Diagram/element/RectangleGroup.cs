using Dalssoft.DiagramNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Diagram.NET.element
{
	[Serializable]
	public class RectangleGroup : RectangleElement
	{
		Dictionary<string, BaseElement> children = new Dictionary<string, BaseElement>();

		public Dictionary<string, BaseElement> Children
		{
			get { return children; }
		}
		public BaseElement this[string Key]
		{

			get { return children[Key]; }
			set { children[Key] = value; }
		}


		public override System.Drawing.Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				var dist = new Point(value.X - base.location.X, value.Y - base.location.Y);

				base.Location = value;

				if (ignoreEvents)
					return;

				ignoreEvents = true;

				foreach (var child in children.Values)
				{
					child.ignore = false;
					var pos = child.Location;
					pos.Offset(dist);
					child.Location = pos;

					if (child is ILabelElement)
						(child as ILabelElement).Label.PositionBySite(child);
					child.ignore = true;
				}

				ignoreEvents = false;
			}
		}

		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				var rs = value;

				int maxX = 0;
				int maxY = 0;

				foreach (var v in children.Values)
				{
					maxX = Math.Max((v.size.Width + v.location.X) - location.X, maxX);
					maxY = Math.Max((v.size.Height + v.location.Y) - location.Y, maxY);
				}

				rs.Width = Math.Max(maxX, rs.Width);
				rs.Height = Math.Max(maxY, rs.Height);

				base.Size = rs;
			}
		}

		bool ignoreEvents = false;

		public RectangleGroup()
			: this(0, 0, 100, 100)
		{ }

		public RectangleGroup(Rectangle rec)
			: this(rec.Location, rec.Size)
		{ }

		public RectangleGroup(Point l, Size s)
			: this(l.X, l.Y, s.Width, s.Height)
		{ }

		public RectangleGroup(int top, int left, int width, int height)
			: base(top, left, width, height)
		{ }

		public void Add(string Key, BaseElement Element)
		{

			children.Add(Key, Element);
			Element.ignore = true;
			//Element.AppearanceChanged += Element_AppearanceChanged;

		}

		//void Element_AppearanceChanged(object sender, EventArgs e)
		//{
		//	if (ignoreEvents)
		//		return;

		//	var elem = sender as BaseElement;

		//	if(elem.location != elem.oldLocation)
		//		elem.Location = elem.oldLocation;

		//	if(elem.size != elem.oldSize && elem.oldSize != Size.Empty)
		//		elem.Size = elem.oldSize;

		//	ignoreEvents = true;
		//	elem.OnAppearanceChanged(EventArgs.Empty);
		//	ignoreEvents = false;

		//	//elem.OnAppearanceChanged(EventArgs.Empty);

		//	/*
		//	bool changes = false;
		//	if (elem.oldLocation != elem.location)
		//	{
		//		if(elem.location.X < location.X || elem.location.X + elem.size.Width > location.X + size.Width ||
		//			elem.location.Y < location.Y || elem.location.Y + elem.size.Height > location.Y + size.Height)
		//		{
		//			elem.Location = elem.oldLocation;
		//			changes = true;
		//		}
		//	}

		//	if (elem.oldSize != elem.size)
		//	{
		//		if (elem.location.X < location.X || elem.location.X + elem.size.Width > location.X + size.Width ||
		//			elem.location.Y < location.Y || elem.location.Y + elem.size.Height > location.Y + size.Height)
		//		{
		//			elem.Size = elem.oldSize;
		//			changes = true;
		//		}

		//	}

		//	if (changes)
		//	{
		//		elem.OnAppearanceChanged(EventArgs.Empty);
		//	}
		//	*/

			
		//	/*
		//	if (ignoreEvents)
		//		return;

		//	var elem = sender as BaseElement;

		//	Point pt = new Point(elem.Location.X - elem.oldLocation.X, elem.Location.Y - elem.oldLocation.Y);

		//	if (pt == Point.Empty)
		//		return;

		//	Rectangle invalidRect = new Rectangle(location, size);

		//	ignoreEvents = true;



		//	var rPos = Location;
		//	rPos.Offset(pt);
		//	Location = rPos;

		//	foreach (var cElem in children.Values)
		//	{

		//		if (cElem == elem)
		//			continue;

		//		var cPos = cElem.Location;
		//		cPos.Offset(pt);
		//		cElem.Location = cPos;
			
		//	}

		//	ignoreEvents = false;

		//	Invalidate();
		//	invalidateRec = Rectangle.Union(invalidateRec, invalidRect);*/
		//}

		public void Remove(string Key)
		{
			//children[Key].AppearanceChanged -= Element_AppearanceChanged;
			children.Remove(Key);
		}

		internal override void OnAddedToDocument(Document.DocumentEventArgs e)
		{
			base.OnAddedToDocument(e);

			foreach (var child in children.Values)
			{
				e.Document.AddElement(child);

				if (child is ILabelElement)
					(child as ILabelElement).Label.PositionBySite(child);

				if (child is NodeElement)
					(child as NodeElement).UpdateConnectorsPosition();
				
				
			}

			Label.PositionBySite(this);
		}

		internal override void OnRemovedFromDocument(Document.DocumentEventArgs e)
		{
			base.OnRemovedFromDocument(e);

			foreach (var child in children.Values)
				e.Document.DeleteElement(child);
		}


		
	}
}
