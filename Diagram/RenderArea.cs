using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Diagram.NET
{
	public partial class RenderArea : UserControl
	{
		public event EventHandler<PaintEventArgs> DoPaint;
		public event EventHandler<PaintEventArgs> DoPaintBackground;

		public RenderArea()
		{
			InitializeComponent();

			// This change control to not flick
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);

		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (DoPaintBackground != null)
				DoPaintBackground(this, e);
			else
				base.OnPaintBackground(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (DoPaint != null)
				DoPaint(this, e);
			else
				base.OnPaint(e);

		}
	}
}
