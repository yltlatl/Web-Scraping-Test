using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Web_Scraping_Test
{
    internal class CommandBase
    {
        #region Constructors

        protected CommandBase(int commandNumber, Delegate executionDelegateInstance)
        {
            //all we need is an int, so arg checking should suffice for validation
            CommandNumber = commandNumber;

            //initialize the list member
            //all we need is a delegate, so arg checking should suffice for validation
            var executeCommand = new List<Delegate>(executionDelegateInstance as IEnumerable<Delegate>);
        }

        protected CommandBase(int commandNumber, string commandDisplayMessage, Delegate executionDelegateInstance)
            //call the constructor with a subset of these arguments
            : this(commandNumber, executionDelegateInstance)
        {
            //validate the commandDisplayText as valid display text
            try
            {
                ValidateCommandDisplayMessage(commandDisplayMessage);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }

            //assign the display text property
            CommandDisplayMessage = commandDisplayMessage;
        }

        #endregion

        #region Properties

        //common properties for commands and menu items
        protected int CommandNumber { get; private set; }
        protected string CommandDisplayMessage { get; private set; }
        protected List<Delegate> CommandAction { get; private set; }

        #endregion

        #region Methods

        private void ValidateCommandDisplayMessage(string menuItemDisplayText)
        {
            //we want menu items to start with a cap, not end with punctuation, and not be more than one line
            if (!Regex.IsMatch(menuItemDisplayText, @"^[A-Z]"))
            {
                throw new ArgumentException("Command display text must start with a capital letter.");
            }
            if (Regex.IsMatch(menuItemDisplayText, @"[.;:,]$"))
            {
                throw new ArgumentException("Command display text must not end with punctuation.");
            }
            if (menuItemDisplayText.Contains("\n"))
            {
                throw new ArgumentException("Command display text must be a single line.");
            }
        }

        #endregion
    }
}