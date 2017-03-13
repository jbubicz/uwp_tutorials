using Windows.UI.Xaml.Controls;
using PropertyChanged;

namespace Navigator.Navigation
{
    [ImplementPropertyChanged]
    public class NavigationFrame
    {

        public NavigationFrame(): this(null) { }

        public NavigationFrame(Frame currentFrame, Frame nestedFrame = null)
        {
            this.CurrentFrame = currentFrame;
            this.NestedFrame = nestedFrame;
        }

        public Frame CurrentFrame { get; set; }
        public Frame NestedFrame { get; set; }
    }
}
