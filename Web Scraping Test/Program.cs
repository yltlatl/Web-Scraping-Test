using System;
using System.Collections.Generic;

namespace Web_Scraping_Test
{
    //for testing
    internal class MainMenu
    {
        public MainMenu()
        {
            TestingMenu = new Dictionary<string, string>
            {
                {"Display main menu", "DisplayMenu"},
                {"Display sub-menu", "DisplayMenu"},
                {"Enter url to scrape", "ScrapeUrl"},
                {"Exit", "Exit"}
            };
        }

        public Dictionary<string, string> TestingMenu { get; private set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            //instantiate our testing struct
            var mMenu = new MainMenu();

            //create a menu object and display the starting menu
            var ConsoleUI = new ConsoleUI();

            //covariance test
            Del_Void_DictionaryIntString displayList = ConsoleUI.DisplayMenu;
            var delegateList = new List<Delegate> {displayList};

            Console.ReadLine();

            //add our testing menu items
            foreach (var kvp in mMenu.TestingMenu)
            {
                ConsoleUI.AddMenuItem(kvp.Key, kvp.Value);
            }

            //display some menus
            MenuItem menuItem = ConsoleUI.MenuItems.Find(m => m.MenuItemNumber == 1);
            do
            {
                var list = new Dictionary<int, string> {{menuItem.MenuItemNumber, menuItem.MenuItemDisplayText}};
                //menuItem.executeMenuChoice.Invoke(list);
                int[] choice = ConsoleUI.GetMenuChoice();
            } while (!menuItem.MenuItemDisplayText.Contains("Exit"));
        }
    }
}