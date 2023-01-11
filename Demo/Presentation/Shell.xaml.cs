using Uno.Toolkit.UI;

namespace Demo.Presentation;

public sealed partial class Shell : UserControl, IContentControlProvider
{
	public Shell()
	{
		this.InitializeComponent();
	}

	public ContentControl ContentControl => Splash;
}
