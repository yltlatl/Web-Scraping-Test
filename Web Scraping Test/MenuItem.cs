using System;
using System.Collections.Generic;

namespace Web_Scraping_Test
{
    internal class MenuItem : CommandBase
    {
        #region Constructors

        //at a minimum a MenuItem must have an ID number, a display message, and a function to execute
        public MenuItem(int menuItemNumber, string menuItemDisplayText, Delegate executionDelegateInstance)
            //call the base constructor with the common constructor arguments
            : base(menuItemNumber, menuItemDisplayText, executionDelegateInstance)
        {
        }

        //a menuitem may optionally have a parent menu item
        public MenuItem(int menuItemNumber, int parentMenuItemNumber, string menuItemDisplayText,
            Delegate executionDelegateInstance)
            //call the constructor that has a subset of this overload's arguments
            : this(menuItemNumber, menuItemDisplayText, executionDelegateInstance)
        {
            //assign the ParentMenuItemNumber property
            //all we need is an int, so arg checking should suffice for validation
            ParentMenuItemNumber = parentMenuItemNumber;
        }

        //a menu item may optionally have a menu group number
        public MenuItem(int menuItemNumber, int parentMenuItemNumber, int menuItemGroupNumber,
            string menuItemDisplayText, Delegate itemActionDelegateInstance)
            //call the constructor that has a subset of this overload's arguments
            : this(menuItemNumber, parentMenuItemNumber, menuItemDisplayText, itemActionDelegateInstance)
        {
            //assign the MenuItemGroupNumber property
            //all we need is an int, so arg checking should suffice for validation
            MenuItemGroupNumber = menuItemGroupNumber;
        }

        #endregion

        #region Properties

        //these properties act as aliases for the protected properties of the base class and are read only
        //they can only be set via the base class constructors (which are protected)
        public int MenuItemNumber
        {
            get { return CommandNumber; }
        }

        public string MenuItemDisplayText
        {
            get { return base.CommandDisplayMessage; }
        }

        public List<Delegate> MenuItemAction
        {
            get { return base.CommandAction; }
        }

        //these are properties of this class proper
        public int ParentMenuItemNumber { get; private set; }
        public int MenuItemGroupNumber { get; private set; }

        #endregion
    }
}