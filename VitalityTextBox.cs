using System.Drawing;
using System.Windows.Forms;

internal class VitalityTextBox : TextBox
{
	protected override void WndProc(ref Message m)
	{
		int msg = m.Msg;
		int num = msg;
		if (num == 15)
		{
			Invalidate();
			base.WndProc(ref m);
			CustomPaint();
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	public VitalityTextBox()
	{
		Font = new Font("Microsoft Sans Serif", 8f);
		ForeColor = Color.Gray;
		BackColor = Color.FromArgb(235, 235, 235);
		base.BorderStyle = BorderStyle.FixedSingle;
	}

	private void CustomPaint()
	{
		Pen lightGray = Pens.LightGray;
		CreateGraphics().DrawLine(lightGray, 0, 0, base.Width, 0);
		CreateGraphics().DrawLine(lightGray, 0, base.Height - 1, base.Width, base.Height - 1);
		CreateGraphics().DrawLine(lightGray, 0, 0, 0, base.Height - 1);
		CreateGraphics().DrawLine(lightGray, base.Width - 1, 0, base.Width - 1, base.Height - 1);
	}
}
