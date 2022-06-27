using System.Drawing;
using System.Drawing.Drawing2D;

internal class VitalityButton : ThemeControl154
{
	private Color G1;

	private Color G2;

	private Color BG;

	public VitalityButton()
	{
		base.Size = new Size(120, 26);
		SetColor("G1", Color.White);
		SetColor("G2", Color.LightGray);
		SetColor("BG", Color.FromArgb(240, 240, 240));
	}

	protected override void ColorHook()
	{
		G1 = GetColor("G1");
		G2 = GetColor("G2");
		BG = GetColor("BG");
	}

	protected override void PaintHook()
	{
		G.Clear(BG);
		if (State == MouseState.Over)
		{
			G.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), new Size(base.Width, base.Height)));
		}
		else if (State == MouseState.Down)
		{
			LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(base.Width, base.Height)), Color.FromArgb(240, 240, 240), Color.White, 90f);
			G.FillRectangle(brush, new Rectangle(new Point(0, 0), new Size(base.Width, base.Height)));
		}
		else if (State == MouseState.None)
		{
			LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(base.Width, base.Height)), Color.White, Color.FromArgb(240, 240, 240), 90f);
			G.FillRectangle(brush2, new Rectangle(new Point(0, 0), new Size(base.Width, base.Height)));
		}
		DrawBorders(Pens.LightGray);
		DrawCorners(Color.Transparent);
		StringFormat stringFormat = new StringFormat();
		stringFormat.Alignment = StringAlignment.Center;
		stringFormat.LineAlignment = StringAlignment.Center;
		G.DrawString(Text, new Font("Segoe UI", 9f), Brushes.Gray, new RectangleF(2f, 2f, base.Width - 5, base.Height - 4), stringFormat);
	}
}
