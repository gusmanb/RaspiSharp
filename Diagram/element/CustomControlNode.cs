using Dalssoft.DiagramNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Diagram.NET.element
{
	[Serializable]
	public class CustomControlNode<T> : SolidRectangleElement where T : Control, new()
	{
		[field: NonSerialized]
		T control;

		[field: NonSerialized]
		Document document;

		public T Control
		{

			get { return control; }
			set 
			{ 
				if (control != null)control.Parent.Controls.Remove(control); 
				control = value; 

				if (document != null)
					document.Designer.Controls.Add(value); 

				OnAppearanceChanged(EventArgs.Empty); 
			}
		}

		Size padding = new Size(0, 0);

		public Size Padding
		{
			get { return padding; }
			set { padding = value; OnAppearanceChanged(EventArgs.Empty); }
		}

		public CustomControlNode()
			: this(0, 0, 100, 100)
		{ }

		public CustomControlNode(Rectangle rec)
			: this(rec.Location, rec.Size)
		{ }

		public CustomControlNode(Point l, Size s)
			: this(l.X, l.Y, s.Width, s.Height)
		{ }

		public CustomControlNode(int top, int left, int width, int height)
			: base(top, left, width, height)
		{

			Control = new T();
		
		}

		private void UpdateControl()
		{
			if (control == null || document == null)
				return;

			Point p = new Point(
				(int)((location.X + padding.Width) * document.Zoom) - document.designer.DocumentOffset.Width + BorderWidth,
				(int)((location.Y + padding.Height) * document.Zoom) - document.designer.DocumentOffset.Height + BorderWidth);

			control.Location = p; 

			Size sz = new System.Drawing.Size(
				(int)((size.Width - padding.Width * 2) * document.Zoom), 
				(int)((size.Height - padding.Height * 2) * document.Zoom));

			control.Size = sz;


			control.Visible = visible;
		}

		internal override void OnAppearanceChanged(EventArgs e)
		{
			UpdateControl();
			base.OnAppearanceChanged(e);
		}

		internal override void OnAddedToDocument(Dalssoft.DiagramNet.Document.DocumentEventArgs e)
		{
			base.OnAddedToDocument(e);
			document = e.Document;
			if (control != null)
			{
				e.Document.Designer.Controls.Add(control);
				UpdateControl();
			}
		}

		void designer_Scroll(object sender, ScrollEventArgs e)
		{
			UpdateControl();
		}

		internal override void OnRemovedFromDocument(Dalssoft.DiagramNet.Document.DocumentEventArgs e)
		{
			base.OnRemovedFromDocument(e);
			document = null;

			if (control != null)
				e.Document.Designer.Controls.Remove(control);
		}

		internal override void Draw(Graphics g)
		{
			base.Draw(g);
			UpdateControl();
		}
	}
}
