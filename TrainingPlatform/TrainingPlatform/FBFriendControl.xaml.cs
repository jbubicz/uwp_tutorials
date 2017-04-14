using Windows.UI.Xaml.Controls;

namespace TrainingPlatform
{
    public sealed partial class FBFriendControl : UserControl
    {
        public FBUserRootobject FBUserRootobject { get { return this.DataContext as FBUserRootobject; } }
        public FBFriendControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
