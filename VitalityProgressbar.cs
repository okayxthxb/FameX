using System;
using System.Drawing;
using System.Drawing.Drawing2D;

internal class VitalityProgressbar : ThemeControl154
{
	private Color BG;

	private int HBPos;

	private int _Minimum;

	private int _Maximum = 100;

	private int _Value;

	public int Minimum
	{
		get
		{
			return _Minimum;
		}
		set
		{
			if (value < 0)
			{
				throw new Exception("Property value is not valid.");
			}
			_Minimum = value;
			if (value > _Value)
			{
				_Value = value;
			}
			if (value > _Maximum)
			{
				_Maximum = value;
			}
			Invalidate();
		}
	}

	public int Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			if (value < 0)
			{
				throw new Exception("Property value is not valid.");
			}
			_Maximum = value;
			if (value < _Value)
			{
				_Value = value;
			}
			if (value < _Minimum)
			{
				_Minimum = value;
			}
			Invalidate();
		}
	}

	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (value > _Maximum || value < _Minimum)
			{
				throw new Exception("Property value is not valid.");
			}
			_Value = value;
			Invalidate();
		}
	}

	public bool Animated
	{
		get
		{
			return base.IsAnimated;
		}
		set
		{
			base.IsAnimated = value;
			Invalidate();
		}
	}

	private void Increment(int amount)
	{
		Value += amount;
	}

	protected override void OnAnimation()
	{
		if (HBPos == 0)
		{
			HBPos = 7;
		}
		else
		{
			HBPos++;
		}
	}

	public VitalityProgressbar()
	{
		Animated = true;
		SetColor("BG", Color.FromArgb(240, 240, 240));
	}

	protected override void ColorHook()
	{
		BG = GetColor("BG");
	}

	protected override void PaintHook()
	{
		G.Clear(BG);
		DrawBorders(Pens.LightGray, 1);
		DrawCorners(Color.Transparent);
		LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(new Point(2, 2), new Size(base.Width - 2, base.Height - 5)), Color.White, Color.FromArgb(240, 240, 240), 90f);
		G.FillRectangle(brush, new Rectangle(new Point(2, 2), new Size(base.Width / Maximum * Value - 5, base.Height - 5)));
		G.RenderingOrigin = new Point(HBPos, 0);
		HatchBrush brush2 = new HatchBrush(HatchStyle.BackwardDiagonal, Color.LightGray, Color.Transparent);
		G.FillRectangle(brush2, new Rectangle(new Point(1, 2), new Size(base.Width / Maximum * Value - 3, base.Height - 3)));
		G.DrawLine(Pens.LightGray, new Point(base.Width / Maximum * Value - 2, 1), new Point(base.Width / Maximum * Value - 2, base.Height - 3));
	}
}
