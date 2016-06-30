using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using Diagram.NET.element;

namespace Dalssoft.DiagramNet
{
	public class Designer : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;

		#region Designer Control Initialization
		//Document
		private Document document;

		// Drag and Drop
		MoveAction moveAction = null;

		// Selection
		BaseElement selectedElement;
		private bool isMultiSelection = false;
		private RectangleElement selectionArea = new RectangleElement(0,0,0,0);
		private IController[] controllers;
		private BaseElement mousePointerElement;
	
		// Resize
		private ResizeAction resizeAction = null;

		// Add Element
		private bool isAddSelection = false;
		
		// Link
		private bool isAddLink = false;
		private ConnectorElement connStart;
		private ConnectorElement connEnd;
		private BaseLinkElement linkLine;

		//Undo
		[NonSerialized]
		private UndoManager undo = new UndoManager(5);
		private HScrollBar horScroll;
		private VScrollBar verScroll;
		private Diagram.NET.RenderArea renderArea1;
		private bool changed = false;

		public Size DocumentOffset {

			get { return new Size((int)(horScroll.Value * document.Zoom), (int)(verScroll.Value * document.Zoom)); }
		
		}

		public Size ScrollOffset
		{

			get { return new Size(horScroll.Value, verScroll.Value); }

		}

		public Designer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			document = new Document(this);

			// This change control to not flick
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			
			// Selection Area Properties
			selectionArea.Opacity = 40;
			selectionArea.FillColor1 = SystemColors.Control;
			selectionArea.FillColor2 = Color.Empty;
			selectionArea.BorderColor = SystemColors.Control;

			// Link Line Properties
			//linkLine.BorderColor = Color.FromArgb(127, Color.DarkGray);
			//linkLine.BorderWidth = 4;

			//EventsHandlers
			RecreateEventsHandlers();
		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.horScroll = new System.Windows.Forms.HScrollBar();
			this.verScroll = new System.Windows.Forms.VScrollBar();
			this.renderArea1 = new Diagram.NET.RenderArea();
			this.SuspendLayout();
			// 
			// horScroll
			// 
			this.horScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.horScroll.LargeChange = 50;
			this.horScroll.Location = new System.Drawing.Point(0, 143);
			this.horScroll.Name = "horScroll";
			this.horScroll.Size = new System.Drawing.Size(156, 17);
			this.horScroll.SmallChange = 5;
			this.horScroll.TabIndex = 0;
			this.horScroll.Visible = false;
			this.horScroll.ValueChanged += new System.EventHandler(this.horScroll_ValueChanged);
			// 
			// verScroll
			// 
			this.verScroll.Dock = System.Windows.Forms.DockStyle.Right;
			this.verScroll.LargeChange = 50;
			this.verScroll.Location = new System.Drawing.Point(139, 0);
			this.verScroll.Name = "verScroll";
			this.verScroll.Size = new System.Drawing.Size(17, 143);
			this.verScroll.SmallChange = 5;
			this.verScroll.TabIndex = 1;
			this.verScroll.Visible = false;
			this.verScroll.ValueChanged += new System.EventHandler(this.verScroll_ValueChanged);
			// 
			// renderArea1
			// 
			this.renderArea1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderArea1.Location = new System.Drawing.Point(0, 0);
			this.renderArea1.Name = "renderArea1";
			this.renderArea1.Size = new System.Drawing.Size(139, 143);
			this.renderArea1.TabIndex = 2;
			// 
			// Designer
			// 
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.renderArea1);
			this.Controls.Add(this.verScroll);
			this.Controls.Add(this.horScroll);
			this.Name = "Designer";
			this.Size = new System.Drawing.Size(156, 160);
			this.ResumeLayout(false);


			renderArea1.DoPaint += renderArea1_DoPaint;
			renderArea1.DoPaintBackground += renderArea1_DoPaintBackground;
			renderArea1.MouseMove += renderArea1_MouseMove;
			renderArea1.MouseUp += renderArea1_MouseUp;
			renderArea1.MouseDown += renderArea1_MouseDown;
			renderArea1.KeyDown += renderArea1_KeyDown;
		}

		void renderArea1_KeyDown(object sender, KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		void renderArea1_MouseDown(object sender, MouseEventArgs e)
		{
			OnMouseDown(e);
		}

		void renderArea1_MouseUp(object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		}

		void renderArea1_MouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		void renderArea1_DoPaintBackground(object sender, PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;
			mtx = g.Transform;
			gc = g.BeginContainer();

			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);

			document.DrawGrid(g, clipRectangle);

			g.EndContainer(gc);
			g.Transform = mtx;
		}

		void renderArea1_DoPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;

			//Point scrollPoint = this.AutoScrollPosition;
			g.TranslateTransform(-DocumentOffset.Width, -DocumentOffset.Height);  //scrollPoint.X, scrollPoint.Y);

			//Zoom
			mtx = g.Transform;
			gc = g.BeginContainer();

			g.SmoothingMode = document.SmoothingMode;
			g.PixelOffsetMode = document.PixelOffsetMode;
			g.CompositingQuality = document.CompositingQuality;

			g.ScaleTransform(document.Zoom, document.Zoom);

			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);

			document.DrawElements(g, clipRectangle);

			if (!((resizeAction != null) && (resizeAction.IsResizing)))
				document.DrawSelections(g, e.ClipRectangle);

			if ((isMultiSelection) || (isAddSelection))
				DrawSelectionRectangle(g);

			if (isAddLink)
			{
				linkLine.CalcLink();
				linkLine.Draw(g);
			}
			if ((resizeAction != null) && (!((moveAction != null) && (moveAction.IsMoving))))
				resizeAction.DrawResizeCorner(g);

			if (mousePointerElement != null)
			{
				if (mousePointerElement is IControllable)
				{
					IController ctrl = ((IControllable)mousePointerElement).GetController();
					ctrl.DrawSelection(g);
				}
			}

			g.EndContainer(gc);
			g.Transform = mtx;
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			renderArea1.Invalidate();
			base.OnInvalidated(e);
		}

		#endregion

		public new void Invalidate()
		{

			base.Invalidate();

			if (document.Elements.Count > 0)
			{
				for (int i = 0; i <= document.Elements.Count - 1; i++)
				{
					BaseElement el = document.Elements[i];

					Invalidate(el);

					if (el is ILabelElement)
						Invalidate(((ILabelElement) el).Label);
				}
			}

			//if ((moveAction != null) && (moveAction.IsMoving))
			//	this.AutoScrollMinSize = new Size((int) ((document.Location.X + document.Size.Width) * document.Zoom), (int) ((document.Location.Y + document.Size.Height) * document.Zoom));

		}

		private void Invalidate(BaseElement el)
		{
			this.Invalidate(el, false);
		}

		private void Invalidate(BaseElement el, bool force)
		{
			if (el == null) return;

			if ((force) || (el.IsInvalidated))
			{
				Rectangle invalidateRec = Goc2Gsc(el.invalidateRec);
				invalidateRec.Inflate(10, 10);
				base.Invalidate(invalidateRec);
			}			
		}

		
		#region Events Overrides
		
		/*
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;

			//Point scrollPoint = this.AutoScrollPosition;
			g.TranslateTransform(-DocumentOffset.Width, -DocumentOffset.Height);  //scrollPoint.X, scrollPoint.Y);

			//Zoom
			mtx = g.Transform;
			gc = g.BeginContainer();
			
			g.SmoothingMode = document.SmoothingMode;
			g.PixelOffsetMode = document.PixelOffsetMode;
			g.CompositingQuality = document.CompositingQuality;
			
			g.ScaleTransform(document.Zoom, document.Zoom);

			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);

			document.DrawElements(g, clipRectangle);

			if (!((resizeAction != null) && (resizeAction.IsResizing)))
				document.DrawSelections(g, e.ClipRectangle);

			if ((isMultiSelection) || (isAddSelection))
				DrawSelectionRectangle(g);
 
			if (isAddLink)
			{
				linkLine.CalcLink();
				linkLine.Draw(g);
			}
			if ((resizeAction != null) && ( !((moveAction != null) && (moveAction.IsMoving))))
				resizeAction.DrawResizeCorner(g);

			if (mousePointerElement != null)
			{
				if (mousePointerElement is IControllable)
				{
					IController ctrl = ((IControllable) mousePointerElement).GetController();
					ctrl.DrawSelection(g);
				}
			}

			g.EndContainer(gc);
			g.Transform = mtx;

			base.OnPaint(e);

		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground (e);

			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;
			mtx = g.Transform;
			gc = g.BeginContainer();
			
			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);
			
			document.DrawGrid(g, clipRectangle);

			g.EndContainer(gc);
			g.Transform = mtx;

		}

		*/

		protected override void OnKeyDown(KeyEventArgs e)
		{
			//Delete element
			if (e.KeyCode == Keys.Delete)
			{
				DeleteSelectedElements();
				EndGeneralAction();
				base.Invalidate();
			}

			//Undo
			if (e.Control && e.KeyCode == Keys.Z)
			{
				if (undo.CanUndo)
					Undo();
			}

			//Copy
			if ((e.Control) && (e.KeyCode == Keys.C))
			{
				this.Copy();
			}

			//Paste
			if ((e.Control) && (e.KeyCode == Keys.V))
			{
				this.Paste();
			}

			//Cut
			if ((e.Control) && (e.KeyCode == Keys.X))
			{
				this.Cut();
			}

			base.OnKeyDown (e);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			if (document != null)
			{
				document.WindowSize = this.renderArea1.Size;
				CheckSize();
			}
		}

		#region Mouse Events
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Point mousePoint;

			//ShowSelectionCorner((document.Action==DesignerAction.Select));

			switch (document.Action)
			{
				// SELECT
				case DesignerAction.Connect:
				case DesignerAction.Select:
					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						
						//Verify resize action
						StartResizeElement(mousePoint);
						if ((resizeAction != null) && (resizeAction.IsResizing)) break;

						// Search element by click
						selectedElement = document.FindElement(mousePoint);	
						
						if (selectedElement != null)
						{
							//Events
							ElementMouseEventArgs eventMouseDownArg = new ElementMouseEventArgs(selectedElement, e.X, e.Y);
							OnElementMouseDown(eventMouseDownArg);

							// Element selected
							if (selectedElement is ConnectorElement)
							{
								StartAddLink((ConnectorElement) selectedElement, mousePoint);
								selectedElement = null;
							}
							else
								StartSelectElements(selectedElement, mousePoint);
						}
						else
						{
							// If click is on neutral area, clear selection
							document.ClearSelection();
							Point p = Gsc2Goc(new Point(e.X, e.Y));;
							isMultiSelection = true;
							selectionArea.Visible = true;
							selectionArea.Location = p;
							selectionArea.Size = new Size(0, 0);
							
							if (resizeAction != null)
								resizeAction.ShowResizeCorner(false);
						}
						base.Invalidate();
					}
					break;

				// ADD
				case DesignerAction.Add:

					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						StartAddElement(mousePoint);
					}
					break;

				// DELETE
				case DesignerAction.Delete:
					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						DeleteElement(mousePoint);
					}					
					break;
			}
			
			base.OnMouseDown (e);
		
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{

			if (e.Button == MouseButtons.None)
			{
				this.Cursor = Cursors.Arrow;
				Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));

				if ((resizeAction != null)
					&& ((document.Action == DesignerAction.Select)				
						|| ((document.Action == DesignerAction.Connect)
							&& (resizeAction.IsResizingLink))))
				{
					this.Cursor = resizeAction.UpdateResizeCornerCursor(mousePoint);
				}
				
				if (document.Action == DesignerAction.Connect)
				{
					BaseElement mousePointerElementTMP = document.FindElement(mousePoint);
					if (mousePointerElement != mousePointerElementTMP)
					{
						if (mousePointerElementTMP is ConnectorElement)
						{
							mousePointerElement = mousePointerElementTMP;
							mousePointerElement.Invalidate();
							this.Invalidate(mousePointerElement, true);
						}
						else if (mousePointerElement != null)
						{
							mousePointerElement.Invalidate();
							this.Invalidate(mousePointerElement, true);
							mousePointerElement = null;
						}
						
					}
				}
				else
				{
					this.Invalidate(mousePointerElement, true);
					mousePointerElement = null;
				}
			}			

			if (e.Button == MouseButtons.Left)
			{
				Point dragPoint = Gsc2Goc(new Point(e.X, e.Y));

				if ((resizeAction != null) && (resizeAction.IsResizing))
				{
					resizeAction.Resize(dragPoint);
					this.Invalidate();					
				}

				if ((moveAction != null) && (moveAction.IsMoving))
				{
					moveAction.Move(dragPoint);
					this.Invalidate();
				}
				
				if ((isMultiSelection) || (isAddSelection))
				{
					Point p = Gsc2Goc(new Point(e.X, e.Y));
					selectionArea.Size = new Size (p.X - selectionArea.Location.X, p.Y - selectionArea.Location.Y);
					selectionArea.Invalidate();
					this.Invalidate(selectionArea, true);
				}
				
				if (isAddLink)
				{
					selectedElement = document.FindElement(dragPoint);
					if ((selectedElement is ConnectorElement) 
						&& (document.CanAddLink(connStart, (ConnectorElement) selectedElement)))
						linkLine.Connector2 = (ConnectorElement) selectedElement;
					else
						linkLine.Connector2 = connEnd;

					IMoveController ctrl = (IMoveController) ((IControllable) connEnd).GetController();
					ctrl.Move(dragPoint);
					
					//this.Invalidate(linkLine, true); //TODO
					base.Invalidate();
				}
			}

			base.OnMouseMove (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Rectangle selectionRectangle = selectionArea.GetUnsignedRectangle();
			
			if ((moveAction != null) && (moveAction.IsMoving))
			{
				ElementEventArgs eventClickArg = new ElementEventArgs(selectedElement);
				OnElementClick(eventClickArg);

				moveAction.End();
				moveAction = null;

				ElementMouseEventArgs eventMouseUpArg = new ElementMouseEventArgs(selectedElement, e.X, e.Y);
				OnElementMouseUp(eventMouseUpArg);
				
				if (changed)
					AddUndo();

				CheckSize();

			}

			// Select
			if (isMultiSelection)
			{
				EndSelectElements(selectionRectangle);
			}
			// Add element
			else if (isAddSelection)
			{
				EndAddElement(selectionRectangle);
			}
			
			// Add link
			else if (isAddLink)
			{
				Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));
				EndAddLink();
				
				AddUndo();
			}
			
			// Resize
			if (resizeAction != null)
			{
				if (resizeAction.IsResizing)
				{
					Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));
					resizeAction.End(mousePoint);
				
					AddUndo();
				}
				resizeAction.UpdateResizeCorner();
			}

			RestartInitValues();

			base.Invalidate();

			base.OnMouseUp (e);
		}

		private void CheckSize()
		{

			int height = 0;
			int width = 0;

			int elemsHeight = 0;
			int elemsWidth = 0;

			foreach (var elem in document.elements.GetArray())
			{
				elemsHeight = Math.Max(elemsHeight, elem.Location.Y + elem.Size.Height + 10);
				elemsWidth = Math.Max(elemsWidth, elem.Location.X + elem.Size.Width + 10);
			}

			height = Math.Max((int)(this.renderArea1.Height / document.Zoom), elemsHeight);
			width = Math.Max((int)(this.renderArea1.Width / document.Zoom), elemsWidth);

			bool redo = false;

			if (height > this.renderArea1.Height / document.Zoom)
			{
				

				verScroll.Visible = true;

				height = Math.Max((int)(this.renderArea1.Height / document.Zoom), elemsHeight);
				width = Math.Max((int)(this.renderArea1.Width / document.Zoom), elemsWidth);

				verScroll.Maximum = (int)(height - this.renderArea1.Height / document.Zoom) + verScroll.LargeChange - 1;

				
			}
			else
			{

				if (verScroll.Visible == true)
					redo = true;

				verScroll.Visible = false;
				verScroll.Value = 0;

				height = Math.Max((int)(this.renderArea1.Height / document.Zoom), elemsHeight);
				width = Math.Max((int)(this.renderArea1.Width / document.Zoom), elemsWidth);
			}

			if (width > this.renderArea1.Width / document.Zoom)
			{
				if (horScroll.Visible == false)
					redo = true;

				horScroll.Visible = true;

				height = Math.Max((int)(this.renderArea1.Height / document.Zoom), elemsHeight);
				width = Math.Max((int)(this.renderArea1.Width / document.Zoom), elemsWidth);

				horScroll.Maximum = (int)(width - this.renderArea1.Width / document.Zoom) + horScroll.LargeChange - 1;

			}
			else
			{
				if (horScroll.Visible == true)
					redo = true;

				horScroll.Visible = false;
				horScroll.Value = 0;

				height = Math.Max((int)(this.renderArea1.Height / document.Zoom), elemsHeight);
				width = Math.Max((int)(this.renderArea1.Width / document.Zoom), elemsWidth);
			}

			document.WindowSize = new Size((int)(width * document.Zoom),(int)(height *document.Zoom));

			if (redo)
				CheckSize();

		}
		#endregion

		#endregion
		
		#region Events Raising
		
		// element handler
		public delegate void ElementEventHandler(object sender, ElementEventArgs e);

		#region Element Mouse Events
		
		// CLICK
		[Category("Element")]
		public event ElementEventHandler ElementClick;
		
		protected virtual void OnElementClick(ElementEventArgs e)
		{
			if (ElementClick != null)
			{
				ElementClick(this, e);
			}
		}

		// mouse handler
		public delegate void ElementMouseEventHandler(object sender, ElementMouseEventArgs e);

		// MOUSE DOWN
		[Category("Element")]
		public event ElementMouseEventHandler ElementMouseDown;
		
		protected virtual void OnElementMouseDown(ElementMouseEventArgs e)
		{
			if (ElementMouseDown != null)
			{
				ElementMouseDown(this, e);
			}
		}

		// MOUSE UP
		[Category("Element")]
		public event ElementMouseEventHandler ElementMouseUp;
		
		protected virtual void OnElementMouseUp(ElementMouseEventArgs e)
		{
			if (ElementMouseUp != null)
			{
				ElementMouseUp(this, e);
			}
		}

		#endregion
		 
		#region Element Move Events
		// Before Move
		[Category("Element")]
		public event ElementEventHandler ElementMoving;
		
		protected virtual void OnElementMoving(ElementEventArgs e)
		{
			if (ElementMoving != null)
			{
				ElementMoving(this, e);
			}
		}

		// After Move
		[Category("Element")]
		public event ElementEventHandler ElementMoved;
		
		protected virtual void OnElementMoved(ElementEventArgs e)
		{
			if (ElementMoved != null)
			{
				ElementMoved(this, e);
			}
		}
		#endregion

		#region Element Resize Events
		// Before Resize
		[Category("Element")]
		public event ElementEventHandler ElementResizing;
		
		protected virtual void OnElementResizing(ElementEventArgs e)
		{
			if (ElementResizing != null)
			{
				ElementResizing(this, e);
			}
		}

		// After Resize
		[Category("Element")]
		public event ElementEventHandler ElementResized;
		
		protected virtual void OnElementResized(ElementEventArgs e)
		{
			if (ElementResized != null)
			{
				ElementResized(this, e);
			}
		}
		#endregion

		#region Element Connect Events
		// connect handler
		public delegate void ElementConnectEventHandler(object sender, ElementConnectEventArgs e);

		// Before Connect
		[Category("Element")]
		public event ElementConnectEventHandler ElementConnecting;
		
		protected virtual void OnElementConnecting(ElementConnectEventArgs e)
		{
			if (ElementConnecting != null)
			{
				ElementConnecting(this, e);
			}
		}

		// After Connect
		[Category("Element")]
		public event ElementConnectEventHandler ElementConnected;
		
		protected virtual void OnElementConnected(ElementConnectEventArgs e)
		{
			if (ElementConnected != null)
			{
				ElementConnected(this, e);
			}
		}
		#endregion

		#region Element Selection Events
		// connect handler
		public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);

		// Selection
		[Category("Element")]
		public event ElementSelectionEventHandler ElementSelection;
		
		protected virtual void OnElementSelection(ElementSelectionEventArgs e)
		{
			if (ElementSelection != null)
			{
				ElementSelection(this, e);
			}
		}

		#endregion

		#endregion

		#region Events Handling
		private void document_PropertyChanged(object sender, EventArgs e)
		{
			if (!IsChanging())
			{
				CheckSize();
				base.Invalidate();
			}
		}

		private void document_AppearancePropertyChanged(object sender, EventArgs e)
		{
			if (!IsChanging())
			{
				AddUndo();
				base.Invalidate();
			}
		}

		private void document_ElementPropertyChanged(object sender, EventArgs e)
		{
			changed = true;

			if (!IsChanging())
			{
				AddUndo();
				base.Invalidate();
			}
		}

		private void document_ElementSelection(object sender, ElementSelectionEventArgs e)
		{
			OnElementSelection(e);
		}
		#endregion

		#region Properties

		public Document Document
		{
			get
			{
				return document;
			}
		}

		public bool CanUndo
		{
			get
			{
				return undo.CanUndo;
			}
		}

		public bool CanRedo
		{
			get
			{
				return undo.CanRedo;
			}
		}


		private bool IsChanging()
		{
			return (
					((moveAction != null) && (moveAction.IsMoving)) //isDragging
					|| isAddLink || isMultiSelection || 
					((resizeAction != null) && (resizeAction.IsResizing)) //isResizing
					);
		}
		#endregion
		
		#region Draw Methods
		
		/// <summary>
		/// Graphic surface coordinates to graphic object coordinates.
		/// </summary>
		/// <param name="p">Graphic surface point.</param>
		/// <returns></returns>
		

		public Rectangle Goc2Gsc(Rectangle gsr)
		{
			float zoom = document.Zoom;
			gsr.X = (int)((gsr.X - horScroll.Value) * zoom);
			gsr.Y = (int)((gsr.Y - verScroll.Value) * zoom);
			gsr.Width = (int)(gsr.Width * zoom);
			gsr.Height = (int)(gsr.Height * zoom);
			return gsr;
		}

		public Point Gsc2Goc(Point gsp)
		{
			float zoom = document.Zoom;
			gsp.X = (int)(gsp.X / zoom + horScroll.Value);
			gsp.Y = (int)(gsp.Y / zoom + verScroll.Value);
			return gsp;
		}

		public Rectangle Gsc2Goc(Rectangle gsr)
		{
			float zoom = document.Zoom;
			gsr.X = (int)((gsr.X + horScroll.Value) / zoom);
			gsr.Y = (int)((gsr.Y + verScroll.Value) / zoom);
			gsr.Width = (int)(gsr.Width / zoom);
			gsr.Height = (int)(gsr.Height / zoom);
			return gsr;
		}

		internal void DrawSelectionRectangle(Graphics g)
		{
			selectionArea.Draw(g);
		}
		#endregion

		#region Open/Save File
		public void Save(string fileName)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, document);
			stream.Close();
		}

		public void Open(string fileName)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			document.designer = null;
			document = (Document) formatter.Deserialize(stream);
			document.designer = this;
			stream.Close();
			RecreateEventsHandlers();
		}
		#endregion

		#region Copy/Paste
		public void Copy()
		{
			if (document.SelectedElements.Count == 0) return;

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			formatter.Serialize(stream, document.SelectedElements.GetArray());
			DataObject data = new DataObject(DataFormats.GetFormat("Diagram.NET Element Collection").Name,
				stream);
			Clipboard.SetDataObject(data);
		}

		public void Paste()
		{
			const int pasteStep = 20;

			undo.Enabled = false;
			IDataObject iData = Clipboard.GetDataObject();
			DataFormats.Format format = DataFormats.GetFormat("Diagram.NET Element Collection");
			if (iData.GetDataPresent(format.Name))
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = (MemoryStream) iData.GetData(format.Name);
				BaseElement[] elCol = (BaseElement[]) formatter.Deserialize(stream);
				stream.Close();

				foreach(BaseElement el in elCol)
				{
					el.Location = new Point(el.Location.X + pasteStep, el.Location.Y + pasteStep);
				}

				document.AddElements(elCol);
				document.ClearSelection();
				document.SelectElements(elCol);
			}
			undo.Enabled = true;
				
			AddUndo();
			EndGeneralAction();
		}

		public void Cut()
		{
			this.Copy();
			DeleteSelectedElements();
			EndGeneralAction();
		}
		#endregion

		#region Start/End Actions and General Functions
		
		#region General
		private void EndGeneralAction()
		{
			RestartInitValues();
			
			if (resizeAction != null) resizeAction.ShowResizeCorner(false);
		}
		
		private void RestartInitValues()
		{
			
			// Reinitialize status
			moveAction = null;

			isMultiSelection = false;
			isAddSelection = false;
			isAddLink = false;

			changed = false;

			connStart = null;
			
			selectionArea.FillColor1 = SystemColors.Control;
			selectionArea.BorderColor = SystemColors.Control;
			selectionArea.Visible = false;

			document.CalcWindow(true);
		}

		#endregion

		#region Selection
		private void StartSelectElements(BaseElement selectedElement, Point mousePoint)
		{
			// Vefiry if element is in selection
			if (!document.SelectedElements.Contains(selectedElement))
			{
				//Clear selection and add new element to selection
				document.ClearSelection();
				document.SelectElement(selectedElement);
			}

			changed = false;
			

			moveAction = new MoveAction();
			MoveAction.OnElementMovingDelegate onElementMovingDelegate = new Dalssoft.DiagramNet.MoveAction.OnElementMovingDelegate(OnElementMoving);
			moveAction.Start(mousePoint, document, onElementMovingDelegate);


			// Get Controllers
			controllers = new IController[document.SelectedElements.Count];
			for(int i = document.SelectedElements.Count - 1; i >= 0; i--)
			{
				if (document.SelectedElements[i] is IControllable)
				{
					// Get General Controller
					controllers[i] = ((IControllable) document.SelectedElements[i]).GetController();
				}
				else
				{
					controllers[i] = null;
				}
			}

			resizeAction = new ResizeAction();
			resizeAction.Select(document);
		}

		private void EndSelectElements(Rectangle selectionRectangle)
		{
			document.SelectElements(selectionRectangle);
		}
		#endregion		

		#region Resize
		private void StartResizeElement(Point mousePoint)
		{
			if ((resizeAction != null)
				&& ((document.Action == DesignerAction.Select)				
					|| ((document.Action == DesignerAction.Connect)
						&& (resizeAction.IsResizingLink))))
			{
				ResizeAction.OnElementResizingDelegate onElementResizingDelegate = new ResizeAction.OnElementResizingDelegate(OnElementResizing);
				resizeAction.Start(mousePoint, onElementResizingDelegate);
				if (!resizeAction.IsResizing)
					resizeAction = null;
			}
		}
		#endregion

		#region Link
		private void StartAddLink(ConnectorElement connStart, Point mousePoint)
		{
			if (document.Action == DesignerAction.Connect)
			{
				this.connStart = connStart;
				this.connEnd = new ConnectorElement(connStart.ParentElement);

				connEnd.Location = connStart.Location;
				IMoveController ctrl = (IMoveController) ((IControllable) connEnd).GetController();
				ctrl.Start(mousePoint);

				isAddLink = true;
				
				switch(document.LinkType)
				{
					case (LinkType.Straight):
						linkLine = new StraightLinkElement(connStart, connEnd);
						break;
					case (LinkType.RightAngle):
						linkLine = new RightAngleLinkElement(connStart, connEnd);
						break;
				}
				linkLine.Visible = true;
				linkLine.BorderColor = Color.FromArgb(150, Color.Black);
				linkLine.BorderWidth = 1;
				
				this.Invalidate(linkLine, true);
				
				OnElementConnecting(new ElementConnectEventArgs(connStart.ParentElement, null, linkLine));
			}
		}


		public event EventHandler<ElementsLinkedArgs> ElementsWillLink;
		public event EventHandler WillDelete;

		private void EndAddLink()
		{
			if (connEnd != linkLine.Connector2)
			{
				linkLine.Connector1.RemoveLink(linkLine);
				linkLine = document.AddLink(linkLine.Connector1, linkLine.Connector2);

				if (ElementsWillLink != null)
				{
					var args = new ElementsLinkedArgs { Connector1 = linkLine.Connector1, Connector2 = linkLine.Connector2, Element1 = linkLine.Connector1.ParentElement, Element2 = linkLine.Connector2.ParentElement, NewLink = linkLine };

					ElementsWillLink(this, args);
					
					if(args.Cancel)
					{
						document.DeleteLink(linkLine);
						connStart = null;
						connEnd = null;
						linkLine = null;
						return;
					
					}
				}
				
				OnElementConnected(new ElementConnectEventArgs(linkLine.Connector1.ParentElement, linkLine.Connector2.ParentElement, linkLine));
			}

			connStart = null;
			connEnd = null;
			linkLine = null;
		}
		#endregion

		#region Add Element
		private void StartAddElement(Point mousePoint)
		{
			document.ClearSelection();

			//Change Selection Area Color
			selectionArea.FillColor1 = Color.LightSteelBlue;
			selectionArea.BorderColor = Color.WhiteSmoke;

			isAddSelection = true;
			selectionArea.Visible = true;
			selectionArea.Location = mousePoint;
			selectionArea.Size = new Size(0, 0);		
		}

		private void EndAddElement(Rectangle selectionRectangle)
		{
			BaseElement el;
			switch (document.ElementType)
			{
				case ElementType.Rectangle:
					el = new RectangleElement(selectionRectangle);
					break;
				case ElementType.RectangleNode:
					el = new RectangleNode(selectionRectangle);
					break;
				case ElementType.Elipse:
					el = new ElipseElement(selectionRectangle);
					break;
				case ElementType.ElipseNode:
					el = new ElipseNode(selectionRectangle);
					break;
				case ElementType.CommentBox:
					el = new CommentBoxElement(selectionRectangle);
					break;
				default:
					el = new RectangleNode(selectionRectangle);
					break;
			}
			
			document.AddElement(el);
			
			document.Action = DesignerAction.Select;	
		}
		#endregion

		#region Delete
		private void DeleteElement(Point mousePoint)
		{
			document.DeleteElement(mousePoint);
			selectedElement = null;
			document.Action = DesignerAction.Select;		
		}

		private void DeleteSelectedElements()
		{
			foreach (var elem in document.SelectedElements)
			{

				if ((elem is BaseLinkElement || elem is RectangleGroup) && WillDelete != null)
					WillDelete(elem, EventArgs.Empty);
			
			}
			document.DeleteSelectedElements();
		}
		#endregion

		#endregion

		#region Undo/Redo
		public void Undo()
		{
			document.designer = null;
			document = (Document) undo.Undo();
			document.designer = this;
			RecreateEventsHandlers();
			if (resizeAction != null) resizeAction.UpdateResizeCorner();
			base.Invalidate();
		}

		public void Redo()
		{
			document.designer = null;
			document = (Document) undo.Redo();
			document.designer = this;
			RecreateEventsHandlers();
			if (resizeAction != null) resizeAction.UpdateResizeCorner();
			base.Invalidate();
		}

		private void AddUndo()
		{
			undo.AddUndo(document);
		}
		#endregion

		private void RecreateEventsHandlers()
		{
			document.PropertyChanged += new EventHandler(document_PropertyChanged);
			document.AppearancePropertyChanged+=new EventHandler(document_AppearancePropertyChanged);
			document.ElementPropertyChanged += new EventHandler(document_ElementPropertyChanged);
			document.ElementSelection += new Document.ElementSelectionEventHandler(document_ElementSelection);
		}

		private void verScroll_ValueChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void horScroll_ValueChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		public class ElementsLinkedArgs : EventArgs
		{

			public bool Cancel { get; set; }
			public BaseElement Element1 { get; set; }
			public BaseElement Element2 { get; set; }
			public BaseLinkElement NewLink { get; set; }
			public ConnectorElement Connector1 { get; set; }
			public ConnectorElement Connector2 { get; set; }
		
		}
	}
}
