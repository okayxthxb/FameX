using System.Drawing;

internal struct Bloom
{
	public string _Name;

	private Color _Value;

	public string Name => _Name;

	public Color Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
		}
	}

	public string ValueHex
	{
		get
		{
			return "#" + _Value.R.ToString("X2", null) + _Value.G.ToString("X2", null) + _Value.B.ToString("X2", null);
		}
		set
		{
			try
			{
				_Value = ColorTranslator.FromHtml(value);
			}
			catch
			{
			}
		}
	}

	public Bloom(string name, Color value)
	{
		_Name = name;
		_Value = value;
	}
}
