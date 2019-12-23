using MyVet.Common.Models;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyVet.Prism.ViewModels
{
    public class VeterinaryMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public VeterinaryMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_pets_menu",
                    PageName = "PetsPage",
                    Title = "Mis mascotas"
                },

                new Menu
                {
                    Icon = "ic_list_alt",
                    PageName = "AgendaPage",
                    Title = "Mi agenda"
                },

                new Menu
                {
                    Icon = "ic_map",
                    PageName = "MapPage",
                    Title = "Mapa"
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ProfilePage",
                    Title = "Modificar perfil"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Cerrar sesión"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}