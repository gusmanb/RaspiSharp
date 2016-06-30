using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class SolidRectangleElement : BaseElement, IControllable
	{
		protected Color backgroundColor = Color.Blue;

		[NonSerialized]
		private RectangleController controller;

		public SolidRectangleElement()
			: this(0, 0, 100, 100)
		{ }

		public SolidRectangleElement(Rectangle rec)
			: this(rec.Location, rec.Size)
		{ }

		public SolidRectangleElement(Point l, Size s)
			: this(l.X, l.Y, s.Width, s.Height)
		{ }

		public SolidRectangleElement(int top, int left, int width, int height)
		{
			location = new Point(top, left);
			size = new Size(width, height);
		}

		public override Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
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
				base.Size = value;
			}
		}

		public virtual Color BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				backgroundColor = value;
				OnAppearanceChanged(new EventArgs());
			}
		}


		protected virtual Brush GetBrush(Rectangle r)
		{
			//Fill rectangle
			Color fill1;
			Brush b;

			if (opacity == 100)
			{
				fill1 = backgroundColor;
			}
			else
			{
				fill1 = Color.FromArgb((int)(255.0f * (opacity / 100.0f)), backgroundColor);
			}

			b = new SolidBrush(fill1);
			
			return b;
		}

		protected virtual void DrawBorder(Graphics g, Rectangle r)
		{
			//Border
			Pen p = new Pen(borderColor, borderWidth);
			g.DrawRectangle(p, r);
			p.Dispose();
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;

			Rectangle r = GetUnsignedRectangle();
			Brush b = GetBrush(r);
			g.FillRectangle(b, r);

			DrawBorder(g, r);
			b.Dispose();
		}

		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new RectangleController(this);
			return controller;
		}

	}
}
