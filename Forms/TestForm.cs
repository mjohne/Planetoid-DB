using Krypton.Toolkit;

using System.Diagnostics;

namespace Planetoid_DB.Forms;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial class TestForm : KryptonForm
{
	public TestForm()
	{
		InitializeComponent();
	}

	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
