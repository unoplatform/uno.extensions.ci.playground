using AppKit;

namespace Demo.macOS;

public static class MainClass
{
	public static void Main(string[] args)
	{
		NSApplication.Init();
		NSApplication.SharedApplication.Delegate = new App();
		NSApplication.Main(args);
	}
}
