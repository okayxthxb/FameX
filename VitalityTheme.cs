using System.Drawing;
using System.Drawing.Drawing2D;

internal class VitalityTheme : ThemeContainer154
{
	private Color G1;

	private Color G2;

	private Color BG;

	public VitalityTheme()
	{
		base.TransparencyKey = Color.Fuchsia;
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
		LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(new Point(1, 1), new Size(base.Width - 2, 23)), G1, G2, 90f);
		G.FillRectangle(brush, new Rectangle(new Point(1, 1), new Size(base.Width - 2, 23)));
		G.DrawLine(Pens.LightGray, 1, 25, base.Width - 2, 25);
		G.DrawLine(Pens.White, 1, 26, base.Width - 2, 26);
		DrawCorners(base.TransparencyKey);
		DrawBorders(Pens.LightGray, 1);
		Rectangle targetRect = new Rectangle(3, 3, 20, 20);
		G.DrawIcon(base.ParentForm.Icon, targetRect);
		G.DrawString(base.ParentForm.Text, new Font("Segoe UI", 9f), Brushes.Gray, new Point(25, 5));
	}
}
