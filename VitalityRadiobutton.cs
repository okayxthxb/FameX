using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

[DefaultEvent("CheckedChanged")]
internal class VitalityRadiobutton : ThemeControl154
{
	public delegate void CheckedChangedEventHandler(object sender);

	private Color BG;

	private bool _Checked;

	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			_Checked = value;
			InvalidateControls();
			if (this.CheckedChanged != null)
			{
				this.CheckedChanged(this);
			}
			Invalidate();
		}
	}

	public event CheckedChangedEventHandler CheckedChanged;

	private void InvalidateControls()
	{
		if (!base.IsHandleCreated || !_Checked)
		{
			return;
		}
		foreach (Control control in base.Parent.Controls)
		{
			if (control != this && control is VitalityRadiobutton)
			{
				((VitalityRadiobutton)control).Checked = false;
			}
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (!_Checked)
		{
			Checked = true;
		}
		base.OnMouseDown(e);
	}

	public VitalityRadiobutton()
	{
		base.LockHeight = 22;
		base.Width = 140;
		SetColor("BG", Color.FromArgb(240, 240, 240));
	}

	protected override void ColorHook()
	{
		BG = GetColor("BG");
	}

	protected override void PaintHook()
	{
		G.Clear(BG);
		G.SmoothingMode = SmoothingMode.HighQuality;
		if (_Checked)
		{
			G.FillEllipse(Brushes.Gray, new Rectangle(new Point(7, 7), new Size(8, 8)));
		}
		if (State == MouseState.Over)
		{
			G.FillEllipse(Brushes.White, new Rectangle(new Point(4, 4), new Size(14, 14)));
			if (_Checked)
			{
				G.FillEllipse(Brushes.Gray, new Rectangle(new Point(7, 7), new Size(8, 8)));
			}
		}
		G.DrawEllipse(Pens.White, new Rectangle(new Point(3, 3), new Size(16, 16)));
		G.DrawEllipse(Pens.LightGray, new Rectangle(new Point(2, 2), new Size(18, 18)));
		G.DrawEllipse(Pens.LightGray, new Rectangle(new Point(4, 4), new Size(14, 14)));
		G.DrawString(Text, new Font("Segoe UI", 9f), Brushes.Gray, 23f, 3f);
	}
}
