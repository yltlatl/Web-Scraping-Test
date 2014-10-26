using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Web_Scraping_Test
{
    //delegate signature for display menu, etc.
    //we use a Dictionary<int, string> (as opposed to a string array) so that we can choose specific key values
    public delegate void Del_Void_DictionaryIntString(Dictionary<int, string> choices);

    //delegate signature for site scraper
    public delegate SiteScraper Del_SiteScraper_();


    internal class ConsoleUI
    {
        #region Constructors

        public ConsoleUI( /*string configFilePath*/)
        {
            //validate the configFilePath
            //if (string.IsNullOrEmpty(configFilePath)) throw new ArgumentNullException("Null config file path.");
            //if (!File.Exists(configFilePath)) throw new ArgumentException("File note found.");

            //populate the actions list from the methods in this class
            //if we decide to add execute menu actions in the future this allows us to just do it once in this class
            IEnumerable<MethodInfo> methodList = GetObjectMethods(this);


            //instantiate a delegate object for each method in the list
            //subsequent code will assign delegate objects to MenuItem objects
            var actions = new Dictionary<string, Delegate>();

            //get the available delegate types
            List<Type> delegateTypes = GetNamespaceDelegateTypes();

            //match methods with delegates
            MatchMethodsWithDelegates(delegateTypes);

            //get all of the methods that return void
            IEnumerable<MethodInfo> voidMethods = from m in methodList
                where m.ReturnType.Name.Equals("Void")
                select m;

            //get all of the void methods that take a Dictionary<int, string>
            foreach (MethodInfo m in voidMethods)
            {
                ParameterInfo[] parameters = m.GetParameters();
                //we want to avoid an index out of range exception
                if (parameters.Length == 1)
                {
                    //we are looking for a method that takes a Dictionary<int, string> parameter
                    if (Regex.IsMatch(parameters[0].ParameterType.FullName,
                        @".*Dictionary`2\[\[System\.Int32.*\[System\.String.*"))
                    {
                        //create a delegate of the appropriate type and add it to the actions list
                        Delegate d = Delegate.CreateDelegate(typeof (Del_Void_DictionaryIntString), this, m);
                        actions.Add(m.Name, d);
                    }
                }
            }

            //Delegate d = Delegate.CreateDelegate(m.DeclaringType, this, m);
            //actions[m.Name] = d;


            //assign the dictionary to the Actions property
            Actions = new Dictionary<string, Delegate>(actions);

            //initialize the menu items member
            MenuItems = new List<MenuItem>();
        }

        #endregion

        #region Fields        

        #endregion

        #region Properties

        protected Dictionary<string, Delegate> Actions { get; private set; }
        public List<MenuItem> MenuItems { get; private set; }

        #endregion

        #region Methods

        //we only allow the item number to be set via this function to guarantee unique values
        public void AddMenuItem(string itemText, string itemAction)
        {
            //item number is keyed to MenuItems index
            //which is a bit weird - maybe create a unique key of item number and parent number (todo)
            int itemNumber = MenuItems.Count + 1;
            //get the delegate object corresponding to the itemAction string after validating argument
            if (!Actions.Keys.Contains(itemAction))
            {
                throw new ArgumentException("Invalid itemAction.");
            }

            Delegate action = Actions[itemAction];

            //instantiate a MenuItem and add it to the list of MenuItems
            var menuItem = new MenuItem(itemNumber, itemText, action);
            MenuItems.Add(menuItem);
        }

        //function to create a new Site Scraper instance
        public SiteScraper CreateSiteScraper()
        {
            var scraper = new SiteScraper();
            return scraper;
        }

        //alias for DisplayList when displaying a link list
        public void DisplayLinkList(Dictionary<int, string> list)
        {
            DisplayList(list);
        }

        //function that does that actual work for the display list aliases
        private void DisplayList(Dictionary<int, string> list)
        {
            foreach (var kvp in list)
            {
                Console.WriteLine("{0}  {1}", kvp.Key, kvp.Value);
            }
        }

        //alias for DisplayList when displaying a menu
        public void DisplayMenu(Dictionary<int, string> list)
        {
            DisplayList(list);
        }

        //collect user input from menu
        //permitMulti allows the user to select multiple menu options at once
        //permitChooseAll permits the user to enter a zero and select all menu options
        public int[] GetMenuChoice(bool permitMulti = false, bool permitChooseAll = false)
        {
            //store the user's input
            string input;

            //create a local variable that can be dynamically sized
            //we want it dynamically sized since we don't know if we will assign one element or some unknown number of elements
            var inputElements = new List<string>();

            //display an appropriate prompt, validate the input, and process valid input depending on whether we can choose multiple menu items or choose all
            if (permitMulti && permitChooseAll)
            {
                Console.WriteLine("Enter one or menu choice numbers separated by commas, or enter 0 to choose all.");

                input = Console.ReadLine();

                if (!(Regex.IsMatch(input, @"^(\d*,*\d*)+$")) && !(Regex.IsMatch(input, @"^0$")))
                {
                    Console.WriteLine("Invalid entry.");
                    GetMenuChoice(true, true);
                }

                string[] inputList = input.Split(new[] {','});

                inputElements = inputList.ToList();
            }
            else if (permitMulti)
            {
                Console.WriteLine("Enter one or menu choice numbers separated by commas.");

                input = Console.ReadLine();

                if (!(Regex.IsMatch(input, @"^(\d*,*\d*)+$")))
                {
                    Console.WriteLine("Invalid entry.");
                    GetMenuChoice(true);
                }
            }
            else
            {
                Console.WriteLine("Enter a menu choice number.");

                input = Console.ReadLine();

                if (!Regex.IsMatch(input, @"^d+$"))
                {
                    Console.WriteLine("Invalid entry.");
                    GetMenuChoice();
                }
            }

            //convert to return ints; reasonably certain we have parseable elements because of assignment code (regex/"0")
            List<int> intList = inputElements.ConvertAll(Int32.Parse);
            int[] selectedItems = intList.ToArray();
            return selectedItems;
        }

        //get the delgate types available in this namespace
        private List<Type> GetNamespaceDelegateTypes()
        {
            const string @namespace = "Web_Scraping_Test";

            IEnumerable<Type> types = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.BaseType != null && (t.BaseType.Name.Contains("Delegate") && t.Namespace.Equals(@namespace))
                select t;

            return types.ToList();
        }

        //get the methods from any object and return a list of method info objects
        private IEnumerable<MethodInfo> GetObjectMethods(Object currentObject)
        {
            //argument validation
            if (currentObject == null)
            {
                throw new ArgumentNullException("currentObject", "Attempted GetOjbectMethods on null object.");
            }

            //use reflection to get the methods
            Type type = currentObject.GetType();
            MethodInfo[] methods = type.GetMethods();
            List<MethodInfo> methodList = methods.ToList();

            //we don't want any methods inherited directly from System.Object
            //or any methods such as accessors, etc.
            methodList.RemoveAll(delegate(MethodInfo m)
            {
                if (m.DeclaringType.Name.Equals("Object") || m.IsSpecialName) return true;
                return false;
            });

            return methodList;
        }

        //match delegates to methods based on signatures
        private void MatchMethodsWithDelegates(IEnumerable<Type> delegates)
        {
            foreach (Type t in delegates)
            {
            }
        }

        #endregion
    }
}