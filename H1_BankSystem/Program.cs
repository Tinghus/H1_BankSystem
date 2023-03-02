using H1_BankSystem.Classes;
using H1_BankSystem.Services;

namespace H1_BankSystem
{
    internal class Program
    {
        public static MenuClass Menu = new MenuClass();
        public static UserService User = new UserService();
        public static UiClass Ui = new UiClass();
        public static AccountService Account = new AccountService();


        static void Main(string[] args)
        {
            BuildReferences();

            while (true)
            {
                Menu.MenuHandler();
            }

        }

        static void BuildReferences()
        {
            Menu.User = User;
            Menu.Ui = Ui;

            Ui.User = User;
            Ui.Menu = Menu;

            User.Menu = Menu;
        }

    }
}