using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

internal class VitalityTabControl : TabControl
{
	public VitalityTabControl()
	{
		SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		DoubleBuffered = true;
	}

	protected override void CreateHandle()
	{
		base.CreateHandle();
		base.Alignment = TabAlignment.Bottom;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Bitmap bitmap = new Bitmap(base.Width, base.Height);
		Graphics graphics = Graphics.FromImage(bitmap);
		try
		{
			base.SelectedTab.BackColor = Color.FromArgb(21, 21, 21);
		}
		catch
		{
		}
		graphics.Clear(base.Parent.BackColor);
		for (int i = 0; i <= base.TabCount - 1; i++)
		{
			if (i != base.SelectedIndex)
			{
				Rectangle rectangle = new Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 3, GetTabRect(i).Width + 2, GetTabRect(i).Height);
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(rectangle.X, rectangle.Y), new Point(rectangle.X, rectangle.Y + rectangle.Height), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20));
				graphics.FillRectangle(linearGradientBrush, rectangle);
				linearGradientBrush.Dispose();
				graphics.DrawRectangle(Pens.Black, rectangle);
				graphics.DrawRectangle(Pens.Black, new Rectangle(rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 2, rectangle.Height));
				graphics.DrawString(base.TabPages[i].Text, Font, Brushes.White, rectangle, new StringFormat
				{
					LineAlignment = StringAlignment.Near,
					Alignment = StringAlignment.Center
				});
			}
		}
		graphics.FillRectangle(new SolidBrush(Color.FromArgb(25, 25, 25)), 0, base.ItemSize.Height, base.Width, base.Height);
		graphics.DrawRectangle(Pens.Black, 0, base.ItemSize.Height, base.Width - 1, base.Height - base.ItemSize.Height - 1);
		graphics.DrawRectangle(Pens.Black, 1, base.ItemSize.Height + 1, base.Width - 3, base.Height - base.ItemSize.Height - 3);
		if (base.SelectedIndex != -1)
		{
			Rectangle rectangle2 = new Rectangle(GetTabRect(base.SelectedIndex).X - 2, GetTabRect(base.SelectedIndex).Y, GetTabRect(base.SelectedIndex).Width + 3, GetTabRect(base.SelectedIndex).Height);
			LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(new Rectangle(rectangle2.X + 2, rectangle2.Y + 2, rectangle2.Width - 2, rectangle2.Height), Color.White, Color.FromArgb(30, 30, 30), 90f);
			graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), new Rectangle(rectangle2.X + 2, rectangle2.Y + 2, rectangle2.Width - 2, rectangle2.Height));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X, rectangle2.Y + rectangle2.Height - 2), new Point(rectangle2.X, rectangle2.Y));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X, rectangle2.Y), new Point(rectangle2.X + rectangle2.Width, rectangle2.Y));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X + rectangle2.Width, rectangle2.Y), new Point(rectangle2.X + rectangle2.Width, rectangle2.Y + rectangle2.Height - 2));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X + 1, rectangle2.Y + rectangle2.Height - 1), new Point(rectangle2.X + 1, rectangle2.Y + 1));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X + 1, rectangle2.Y + 1), new Point(rectangle2.X + rectangle2.Width - 1, rectangle2.Y + 1));
			graphics.DrawLine(Pens.Black, new Point(rectangle2.X + rectangle2.Width - 1, rectangle2.Y + 1), new Point(rectangle2.X + rectangle2.Width - 1, rectangle2.Y + rectangle2.Height - 1));
			graphics.DrawString(base.TabPages[base.SelectedIndex].Text, Font, Brushes.White, rectangle2, new StringFormat
			{
				LineAlignment = StringAlignment.Center,
				Alignment = StringAlignment.Center
			});
		}
		graphics.DrawLine(new Pen(Color.FromArgb(25, 25, 25)), new Point(0, 1), new Point(0, 2));
		e.Graphics.DrawImage((Bitmap)bitmap.Clone(), 0, 0);
		graphics.Dispose();
		bitmap.Dispose();
	}
}
