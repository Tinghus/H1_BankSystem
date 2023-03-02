using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H1_BankSystem.Classes;
using H1_BankSystem.Services;
using static H1_BankSystem.Services.UserService;

namespace H1_BankSystem.Classes
{
    public class MenuClass
    {
        public UserService User { get; set; }
        public UiClass Ui { get; set; }

        public List<MenuObject> MenuItems { get; set; } = new List<MenuObject>();
        public int ActiveMenu { get; set; }
        public int HoveredItem { get; set; } = 0;

        object ItemBeingViewed { get; set; }

        public ViewModel currentViewModel { get; set; }

        public void MenuHandler()
        {
            // Resetting the console
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
            Console.CursorLeft = 0;
            Console.CursorTop = 0;


            if (!User.UserIsLoggedIn)
            {
                currentViewModel = ViewModel.Login;
                LoadViewModel();
                return;
            }
            else
            {
                LoadMenu();
            }

            Ui.InsertHorizontalLine();
            LoadViewModel();
            MenuNavigation();
        }


        private void MenuNavigation()
        {
            ConsoleKeyInfo key;
            Console.CursorVisible = false;
            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    MenuNavigateVertical(-1);
                    break;

                case ConsoleKey.DownArrow:
                    MenuNavigateVertical(1);
                    break;

                case ConsoleKey.LeftArrow:
                    MenuNavigationHorizontal(-1);
                    break;

                case ConsoleKey.RightArrow:
                    MenuNavigationHorizontal(1);
                    break;

                case ConsoleKey.Enter:
                    MenuActivateItem();
                    break;

                case ConsoleKey.Escape:
                    MenuDeactivateItem();
                    break;

                case ConsoleKey.Backspace:
                    break;
            }
        }

        private void MenuActivateItem()
        {
            switch (currentViewModel)
            {
                case ViewModel.CustomerList_Teller:
                    currentViewModel = ViewModel.CustomerDetails_Teller;
                    ItemBeingViewed = DataService.UserList[HoveredItem];
                    break;
            }

        }

        private void MenuDeactivateItem()
        {
            switch (currentViewModel)
            {
                case ViewModel.CustomerDetails_Teller:
                    currentViewModel = ViewModel.CustomerList_Teller;
                    break;
            }
        }

        private void MenuNavigationHorizontal(int modifier)
        {
            int minHorizontalRange = 0;
            int maxHorizontalRange = MenuItems.Count - 1;

            if (modifier == -1 && ActiveMenu <= minHorizontalRange)
            {
                ActiveMenu = minHorizontalRange;
                return;
            }

            if (modifier == 1 && ActiveMenu >= maxHorizontalRange)
            {
                ActiveMenu = maxHorizontalRange;
                return;
            }

            ActiveMenu += modifier;
            HoveredItem = 0;
            currentViewModel = MenuItems[ActiveMenu].RelatedViewModel;
        }


        private void MenuNavigateVertical(int modifier)
        {
            int minVerticalRange = GetMinVerticalRange();
            int maxVerticalRange = GetMaxVerticalRange();

            if (modifier == -1 && HoveredItem <= minVerticalRange)
            {
                HoveredItem = minVerticalRange;
                return;
            }

            if (modifier == 1 && HoveredItem >= maxVerticalRange)
            {
                HoveredItem = maxVerticalRange;
                return;
            }

            HoveredItem += modifier;

        }

        private int GetMinVerticalRange()
        {
            // TODO add support for multi page data
            return 0;
        }

        private int GetMaxVerticalRange()
        {
            switch (currentViewModel)
            {
                case ViewModel.Login:
                    return 0;

                case ViewModel.CustomerList_Teller:
                    return DataService.UserList.Count - 1;

                case ViewModel.AccountList_Teller:
                    return DataService.AccountList.Count - 1;

                default:
                    return 0;
            }
        }


        private void LoadMenu()
        {
            Ui.ApplyEffect(User.CurrentUser.Username, UiClass.Effect.Username);

            int currentCursorPos = 15;

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.CursorLeft = currentCursorPos;

                if (i == ActiveMenu)
                {
                    Ui.ApplyEffect(MenuItems[i].DisplayName, UiClass.Effect.ActiveMenuItem);
                }
                else
                {
                    Console.Write(MenuItems[i].DisplayName);
                }

                currentCursorPos += 15;
            }
        }

        private void LoadViewModel()
        {
            switch (currentViewModel)
            {
                case ViewModel.Login:
                    Ui.ShowLoginForm();
                    break;

                case ViewModel.CustomerList_Teller:
                    Ui.ShowCustomerList(DataService.UserList, 2, 0);
                    break;

                case ViewModel.CustomerDetails_Teller:
                    Ui.ShowCustomerDetails((UserService.UserClass)ItemBeingViewed, 2);
                    break;

                case ViewModel.AccountList_Teller:
                    Ui.ShowAccountListDetailed(User.CurrentUser, 2, 0);
                    break;
            }

        }

        public void PrepareMenuItems(UserService.UserClass user)
        {
            MenuItems.Clear();

            switch (user.UserType)
            {
                case UserClass.UserTypes.Teller:
                    currentViewModel = MenuClass.ViewModel.CustomerList_Teller;
                    PrepareMenuItemsTeller();
                    break;

                case UserClass.UserTypes.AccountHolder:
                    currentViewModel = ViewModel.AccountList_AccountHolder;
                    PrepareMenuItemsAccountHolder();
                    break;
            }



            ActiveMenu = 0;
        }

        private void PrepareMenuItemsTeller()
        {
            MenuItems.Add(new MenuObject
            {
                Name = "CustomerList_Teller",
                DisplayName = "Customers",
                RelatedViewModel = ViewModel.CustomerList_Teller,
            });

            MenuItems.Add(new MenuObject
            {
                Name = "AccountList_Teller",
                DisplayName = "Accounts",
                RelatedViewModel = ViewModel.AccountList_Teller,
            });

            MenuItems.Add(new MenuObject
            {
                Name = "Deposit_Teller",
                DisplayName = "Deposit",
                RelatedViewModel = ViewModel.Deposit_Teller,
            });

            MenuItems.Add(new MenuObject
            {
                Name = "Withdraw_Teller",
                DisplayName = "Withdraw",
                RelatedViewModel = ViewModel.Withdraw_Teller,
            });

            MenuItems.Add(new MenuObject
            {
                Name = "Transfer_Teller",
                DisplayName = "Transfer",
                RelatedViewModel = ViewModel.Transfer_Teller,
            });

        }

        private void PrepareMenuItemsAccountHolder()
        {

        }

        public class MenuObject
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public ViewModel RelatedViewModel { get; set; }

        }

        public enum ViewModel
        {
            Login,
            CustomerList_Teller,
            CustomerDetails_Teller,
            Deposit_Teller,
            Withdraw_Teller,
            Transfer_Teller,
            NewCustomer_Teller,
            AccountList_Teller,
            AccountList_AccountHolder,
        }


    }
}
