using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Presentation.Command;
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase selectedViewModel;

        private ICommand switchViewCommand;

        public MainViewModel()
        {
            selectedViewModel = new ClientListViewModel();

            switchViewCommand = new RelayCommand(view => { SwitchView(view.ToString()); });
        }

        public ViewModelBase SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand SwitchViewCommand
        {
            get => switchViewCommand;
        }

        public void SwitchView(string view)
        {
            switch (view)
            {
                case "ClientListView":
                    ShowClientListView();
                    break;

                case "ItemListView":
                    ShowItemsView();
                    break;

                case "EventListView":
                    ShowEventsView();
                    break;
            }
        }

        private void ShowClientListView()
        {
            SelectedViewModel = new ClientListViewModel();
        }

        private void ShowItemsView()
        {
            SelectedViewModel = new ItemListViewModel();
        }

        private void ShowEventsView()
        {
            SelectedViewModel = new EventListViewModel();
        }
    }
}
