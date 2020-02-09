using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Navigation;

namespace Sample.TabVIew.ViewModels
{
    public class PersonViewModel : BindableBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            ObservableCollection<PersonViewModel> people = new ObservableCollection<PersonViewModel>();
            for (int i = 0; i < 5; i++)
            {
                people.Add(new PersonViewModel() { FirstName = $"FirstName {i}", LastName = $"LastName {i}" });
            }
            People = people;
        }

        public ObservableCollection<PersonViewModel> People { get; set; } = new ObservableCollection<PersonViewModel>();
    }
}
