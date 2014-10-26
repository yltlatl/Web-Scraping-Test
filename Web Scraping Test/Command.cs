using System;
using System.Collections.Generic;

namespace Web_Scraping_Test
{
    internal class Command : CommandBase
    {
        #region Constructors

        //at a minimum a command must have a command number and a function to execute
        public Command(int commandNumber, Delegate executionDelegateInstance)
            //the base class has a constructor overload for this
            : base(commandNumber, executionDelegateInstance)
        {
        }

        //a command may optionally have a display message
        public Command(int commandNumber, string commandDisplayMessage, Delegate executionDelegateInstance)
            //the base class has a constructor overload for this
            : base(commandNumber, commandDisplayMessage, executionDelegateInstance)
        {
        }

        #endregion

        #region Properties

        //these properties act as aliases to the protected properties in the base class and are read only
        //they can be set only by the base class constructors, which are protected
        //the whole point of this structure is to differentiate commands from menu items
        public new int CommandNumber
        {
            get { return base.CommandNumber; }
        }

        public new string CommandDisplayMessage
        {
            get { return base.CommandDisplayMessage; }
        }

        public new List<Delegate> CommandAction
        {
            get { return base.CommandAction; }
        }

        #endregion
    }
}