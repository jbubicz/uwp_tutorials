using System.Composition;
using Windows.UI.Xaml.Controls;
using GlobalMenu.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GlobalMenu.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Export(typeof(Page))]
    public sealed partial class MainPage:Page
    {  
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded+=(sender, args) => this.DataContext = new GlobalMenuViewModel(this.Frame,this.NestedFrame);
        }
    }
}

