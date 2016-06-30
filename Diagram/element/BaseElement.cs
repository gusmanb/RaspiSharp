using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This is the base for all element the will be draw on the
	/// document.
	/// </summary>
	[Serializable]
	public abstract class BaseElement
	{

		[NonSerialized]
		public object tag;

		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}

		internal Point location;
		internal Size size;
		internal bool visible = true;
		internal Color borderColor = Color.Black;
		internal int borderWidth = 1;
		internal int opacity = 100;
		internal protected Rectangle invalidateRec = Rectangle.Empty;
		internal protected bool IsInvalidated = true;

		protected BaseElement()
		{
		}

		protected BaseElement(int top, int left, int width, int height)
		{
			location  = new Point(top, left);
			size = new Size(width, height);
		}

		internal Point oldLocation;

		internal bool ignore = false;

		public virtual Point Location
		{
			get
			{
				return location;
			}
			set
			{

				if (ignore)
					return;

				location = value;
				OnAppearanceChanged(new EventArgs());

			}
		}

		internal Size oldSize;

		public virtual Size Size
		{
			get
			{
				return size;
			}
			set
			{

				if (ignore)
				{

					OnAppearanceChanged(new EventArgs());
					return;
				
				}

				size = value;
				
				if (size.Height < 5)
					size.Height = 5;
				if (size.Width < 5)
					size.Width = 5;

				OnAppearanceChanged(new EventArgs());
				
				if (this is ILabelElement)
					(this as ILabelElement).Label.PositionBySite(this);
			}
		}

		internal bool oldVisible;

		public virtual bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				oldVisible = visible;
				visible = value;
				OnAppearanceChanged(new EventArgs());
				oldVisible = visible;
			}
		}

		internal Color oldBorderColor;

		public virtual Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				oldBorderColor = borderColor; 
				borderColor = value;
				OnAppearanceChanged(new EventArgs());
				oldBorderColor = borderColor; 
			}
		}

		internal int oldBorderWidth;

		public virtual int BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				oldBorderWidth = borderWidth;
				borderWidth = value;
				OnAppearanceChanged(new EventArgs());
				oldBorderWidth = borderWidth;
			}
		}

		internal int oldOpacity;

		public virtual int Opacity 
		{
			get
			{
				return opacity;
			}
			set
			{
				if ((value >= 0) || (value <= 100))
				{
					oldOpacity = opacity;
					opacity = value;
				}
				else
					throw new Exception("'" + value + "' is not a valid value for 'Opacity'. 'Opacity' should be between 0 and 100.");

				OnAppearanceChanged(new EventArgs());
				oldOpacity = opacity;
			}
		}

		[field: NonSerialized]
		protected Document document;

		public Document Document
		{
			get { return document; }
		}

		internal virtual void Draw(Graphics g)
		{
			IsInvalidated = false;
		}
		
		public virtual void Invalidate()
		{
			if (IsInvalidated)
				invalidateRec = Rectangle.Union(invalidateRec, GetUnsignedRectangle());
			else
				invalidateRec = GetUnsignedRectangle();

			IsInvalidated = true;
		}

		public virtual Rectangle GetRectangle()
		{
			return new Rectangle(this.Location, this.Size);
		}

		public virtual Rectangle GetUnsignedRectangle()
		{
			
			return GetUnsignedRectangle(GetRectangle());
		}

		internal static Rectangle GetUnsignedRectangle(Rectangle rec)
		{
			Rectangle retRectangle = rec;
			if (rec.Width < 0)
			{
				retRectangle.X = rec.X + rec.Width;
				retRectangle.Width = - rec.Width;
			}
			
			if (rec.Height < 0)
			{
				retRectangle.Y = rec.Y + rec.Height;
				retRectangle.Height = - rec.Height;
			}

			return retRectangle;
		}

		#region Events
		[field: NonSerialized]
		public event EventHandler AppearanceChanged;

		internal virtual void OnAppearanceChanged(EventArgs e)
		{
			if (AppearanceChanged != null)
				AppearanceChanged(this, e);
		}

		[field: NonSerialized]
		public event EventHandler<Dalssoft.DiagramNet.Document.DocumentEventArgs> AddedToDocument;

		internal virtual void OnAddedToDocument(Dalssoft.DiagramNet.Document.DocumentEventArgs e)
		{
			document = e.Document;
			if (AddedToDocument != null)
				AddedToDocument(this, e);
		}

		[field: NonSerialized]
		public event EventHandler<Dalssoft.DiagramNet.Document.DocumentEventArgs> RemovedFromDocument;

		internal virtual void OnRemovedFromDocument(Dalssoft.DiagramNet.Document.DocumentEventArgs e)
		{
			document = null;

			if (RemovedFromDocument != null)
				RemovedFromDocument(this, e);
		}
		#endregion

	}
}
