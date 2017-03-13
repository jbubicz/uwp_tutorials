using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Prism.Commands;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Controls
{
    public sealed partial class TileMenu : UserControl
    {
        public static readonly DependencyProperty ItemTemplateDependencyProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(TileMenu), new PropertyMetadata(null));
        public static readonly DependencyProperty ItemsSourceDependencyProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(TileMenu), new PropertyMetadata(null));
        public static readonly DependencyProperty CanReorderItemsDependencyProperty = DependencyProperty.Register("CanReorderItems", typeof(bool), typeof(TileMenu), new PropertyMetadata(null));
        public static readonly DependencyProperty CanDragItemsDependencyProperty = DependencyProperty.Register("CanDragItems", typeof(bool), typeof(TileMenu), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectionModeDependencyProperty = DependencyProperty.Register("SelectionMode", typeof(ListViewSelectionMode), typeof(TileMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemClickDependencyProperty = DependencyProperty.Register("ItemClick", typeof(DelegateCommand<object>), typeof(TileMenu), new PropertyMetadata(null));

        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)this.GetValue(SelectionModeDependencyProperty); }
            set { this.SetValue(SelectionModeDependencyProperty, value); }
        }
        public bool CanDragItems
        {
            get { return (bool)this.GetValue(CanDragItemsDependencyProperty); }
            set { this.SetValue(CanDragItemsDependencyProperty, value); }
        }
        public bool CanReorderItems
        {
            get { return (bool)this.GetValue(CanReorderItemsDependencyProperty); }
            set { this.SetValue(CanReorderItemsDependencyProperty, value); }
        }
        public object ItemsSource
        {
            get { return (object)this.GetValue(ItemsSourceDependencyProperty); }
            set { this.SetValue(ItemsSourceDependencyProperty, value); }
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)this.GetValue(ItemTemplateDependencyProperty); }
            set { this.SetValue(ItemTemplateDependencyProperty, value); }
        }
        public DelegateCommand<object> ItemClick
        {
            get { return (DelegateCommand<object>)this.GetValue(ItemClickDependencyProperty); }
            set { this.SetValue(ItemClickDependencyProperty, this.ItemClick); }
        }

        public TileMenu()
        {
            this.InitializeComponent();

            this.MyGridView.ItemClick += this.MyGridView_ItemClick;

            //this.root.DataContext = this;
        }

        private void MyGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ItemClick.Execute(e.ClickedItem);
        }
    }
}
