magicmanam.Windows.ClipboardViewer
==============================

`magicmanam.Windows.ClipboardViewer` [nuget package](https://www.nuget.org/packages/magicmanam.Windows.ClipboardViewer) is a simple wrapper over Windows ClipboardViewer.

In order to add it to your solution, run `Install-Package magicmanam.Windows.ClipboardViewer` from your NuGet Package Manager console in Visual Studio. Sample of code you can find below:

```CSharp
using magicmanam.Windows;
using magicmanam.Windows.ClipboardViewer;

class Form {
  private ClipboardViewer _clipboardViewer;

  protected override void WndProc(ref Message m)
  {
    if (m.Msg == Messages.WM_CREATE)
    {// Create clipboard viewer wrapper
      this._clipboardViewer = new ClipboardViewer(this.Handle);

      // To make sure that the viewer still gets clipboard events (well-known Windows issue with customer viewers),
      // Start call the line below every n-seconds (create a timer or some other way)
      // this._clipboardViewer.RefreshViewer();
    }
    else if (this._clipboardViewer != null)
    {// Process well-known Windows messages
      this._clipboardViewer.HandleWindowsMessage(m.Msg, m.WParam, m.LParam);
    }

    if (m.Msg == Messages.WM_DRAWCLIPBOARD)
    {
      // After registering clipboard viewer you can get this event
      // and retrieve clipboard object as below
      var dataObject = System.Windows.Forms.Clipboard.GetDataObject();
    }

    base.WndProc(ref m);
  }
}
```

## License
MIT

## Thanks
I want to say thanks to the author of the Nuget icon:
https://www.iconsdb.com/royal-blue-icons/copy-icon.html 