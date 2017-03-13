using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Navigator.Navigation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Navigator.FrameControl
{
    public sealed class NavigatedFrame : Frame
    {
        public NavigatedFrame()
        {
            this.DefaultStyleKey = typeof(NavigatedFrame);
          //  this.Loaded += (sender, args) => FramesRegister.AddFrameRegister(new NavigationFrame(this, this.SourcePageType, this.HistoryKey));
        }

        public static readonly DependencyProperty HistoryKeyDP =
            DependencyProperty.Register("HistoryKey", typeof(string), typeof(NavigationFrame), new PropertyMetadata(null));

        public string HistoryKey
        {
            get { return (string)this.GetValue(HistoryKeyDP); }
            set { this.SetValue(HistoryKeyDP, value); }
        }
    }
}
