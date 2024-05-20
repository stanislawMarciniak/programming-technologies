using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for ItemListView.xaml
    /// </summary>
    public partial class ItemListView : UserControl
    {
        public ItemListView()
        {
            InitializeComponent();

            ItemListViewModel _vm = (ItemListViewModel)DataContext;

            _vm.MessageBoxShowDelegate = text => MessageBox.Show(
                text, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
