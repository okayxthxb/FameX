using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

[DefaultEvent("CheckedChanged")]
internal class VitalityCheckbox : ThemeControl154
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
			Invalidate();
			if (this.CheckedChanged != null)
			{
				this.CheckedChanged(this);
			}
		}
	}

	public event CheckedChangedEventHandler CheckedChanged;

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		if (_Checked)
		{
			_Checked = false;
		}
		else
		{
			_Checked = true;
		}
	}

	public VitalityCheckbox()
	{
		base.LockHeight = 22;
		SetColor("G1", Color.White);
		SetColor("G2", Color.LightGray);
		SetColor("BG", Color.FromArgb(240, 240, 240));
	}

	protected override void ColorHook()
	{
		BG = GetColor("BG");
	}

	protected override void PaintHook()
	{
		G.Clear(BG);
		if (_Checked)
		{
			G.DrawString("a", new Font("Marlett", 14f), Brushes.Gray, new Point(0, 1));
		}
		if (State == MouseState.Over)
		{
			G.FillRectangle(Brushes.White, new Rectangle(new Point(3, 3), new Size(15, 15)));
			if (_Checked)
			{
				G.DrawString("a", new Font("Marlett", 14f), Brushes.Gray, new Point(0, 1));
			}
		}
		G.DrawRectangle(Pens.White, 2, 2, 17, 17);
		G.DrawRectangle(Pens.LightGray, 3, 3, 15, 15);
		G.DrawRectangle(Pens.LightGray, 1, 1, 19, 19);
		G.DrawString(Text, new Font("Segoe UI", 9f), Brushes.Gray, 22f, 3f);
	}
}
