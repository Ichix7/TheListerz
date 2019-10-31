using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;

namespace Listerz2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor for Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow1_Loaded;
        }
        /// <summary>
        /// Collection of items that a user can add to a list. 
        /// This collection should not vary during use of program.
        /// </summary>
        List<Item> items = new List<Item>();
        /// <summary>
        /// Collection of items added into lists. 
        /// Objects have all elements of Item class, as well as an additional element called "Username"
        /// that indicates which user list it belongs to. "UserName" is actually the name of the list
        /// </summary>
        List<User> users = new List<User>();
        /// <summary>
        /// A string that aids with preventing lists from being outputted twice to the UserLists listbox
        /// See use in MainWindow1_Loaded
        /// </summary>
        string Nametwice = "";
        /// <summary>
        /// The logic for the program loading.
        /// Loads items from ListItems XML document;
        /// loads lists from UserListerz XML document. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string Usernames;
        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("Listerz"))
            { Directory.CreateDirectory("Listerz"); }
            if (!File.Exists(("Listerz\\ListItems.xml")))
            {
                XmlWriter xW = XmlWriter.Create("Listerz\\ListItems.xml");
                xW.WriteStartElement("Items");
                xW.WriteEndElement();
                xW.Close();
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\ListItems.xml");
            foreach (XmlNode xnode in xdoc.SelectNodes("Items/Item"))
            {
                Item p = new Item();
                p.Store = xnode.SelectSingleNode("Store").InnerText;
                p.Itm = xnode.SelectSingleNode("Item").InnerText;
                p.Price = xnode.SelectSingleNode("Price").InnerText;
                items.Add(p); // puts items in list
                ListItems.Items.Add(p.Itm); // puts items in combobox
            }
            if (!File.Exists(("Listerz\\UserListerz.xml")))
            {
                XmlWriter xW = XmlWriter.Create("Listerz\\UserListerz.xml");
                xW.WriteStartElement("UserLists");
                xW.WriteEndElement();
                xW.Close();
            }
            xdoc.Load("Listerz\\UserListerz.xml");
            foreach (XmlNode xnode in xdoc.SelectNodes("UserLists/User"))
            {
                User p = new User();
                p.UStore = xnode.SelectSingleNode("Store").InnerText;
                p.UItm = xnode.SelectSingleNode("Item").InnerText;
                p.UPrice = xnode.SelectSingleNode("Price").InnerText;
                p.UserName = xnode.SelectSingleNode("UserName").InnerText;
                if (Nametwice == p.UserName) { continue; }
                else { UserLists.Items.Add(p.UserName); Nametwice = p.UserName; }
            }

        }

        /// <summary>
        /// Definition of the items a user can add to a list,
        /// including which Store the item is from, its price, and its name (as "Itm")
        /// </summary>
        class Item
        {
            public string Store
            {
                get;
                set;
            }
            public string Itm
            {
                get;
                set;
            }
            public string Price
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Definition of the items in a user's list, including the name of the list in "UserName"
        /// </summary>
        class User
        {
            public string UStore
            {
                get;
                set;
            }
            public string UItm
            {
                get;
                set;
            }
            public string UPrice
            {
                get;
                set;
            }
            public string UserName
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Logic for populating ListOfItems with the items in the selected user list after user selects said list
        /// from the ListItems listView. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\ListItems.xml");
            foreach (XmlNode xnode in xdoc.SelectNodes("Items/Item"))
            {
                Item p = new Item();
                User q = new User();
                p.Itm = xnode.SelectSingleNode("Item").InnerText;
                if (p.Itm == ListItems.SelectedItem.ToString())
                {
                    q.UItm = xnode["Item"].InnerText;
                    q.UPrice = xnode["Price"].InnerText;
                    q.UStore = xnode["Store"].InnerText;
                    q.UserName = Usernames;
                    users.Add(q);
                    ListOfItems.Items.Add(xnode["Item"].InnerText + "     $" + xnode["Price"].InnerText + "  @" + xnode["Store"].InnerText);
                }
            }
        }

        /// <summary>
        /// Logic for button, when clicked, that shows UserLists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenUser_Click(object sender, RoutedEventArgs e)
        {
            Clear_List.Visibility = Visibility.Hidden;
            OpenUser.Visibility = Visibility.Hidden;
            UserLists.Visibility = Visibility.Visible;
            CloseUser.Visibility = Visibility.Visible;
            ListItems.Visibility = Visibility.Visible;
            
        }

        /// <summary>
        /// Logic for button, when clicked, that hides UserLists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseUser_Click(object sender, RoutedEventArgs e)
        {
            OpenUser.Visibility = Visibility.Visible;
            UserLists.Visibility = Visibility.Hidden;
            CloseUser.Visibility = Visibility.Hidden;
            CreateList.Visibility = Visibility.Visible;
            ListItems.Visibility = Visibility.Hidden;
            textBox.Visibility = Visibility.Visible;
            CreateList.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Logic for button, when clicked, that creates a new user list. Shows the 'list editing' veiw.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            ListItems.Visibility = Visibility.Visible;
            ListOfItems.Visibility = Visibility.Visible;
            CreateList.Visibility = Visibility.Hidden;
            textBox.Visibility = Visibility.Hidden;
            label.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            Usernames = textBox.Text;
            label.Content =Usernames;
            ListOfItems.Items.Clear();
            Done.Visibility = Visibility.Visible;
            Clear_List.Visibility = Visibility.Visible;
            Remove_List.Visibility = Visibility.Hidden;
            Remove_Item.Visibility = Visibility.Hidden;
            Share.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Function that adds all the items in the current user list to that user list
        /// </summary>>
        private void AddToDoc()
        {

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\UserListerz.xml");
            XmlNode xNode = xdoc.SelectSingleNode("UserLists");
            //xNode.RemoveAll();
            foreach (User p in users)
            {

                XmlNode xTop = xdoc.CreateElement("User");
                XmlNode xStore = xdoc.CreateElement("Store");
                XmlNode xItem = xdoc.CreateElement("Item");
                XmlNode xPrice = xdoc.CreateElement("Price");
                XmlNode xUserName = xdoc.CreateElement("UserName");
                xStore.InnerText = p.UStore;
                xItem.InnerText = p.UItm;
                xPrice.InnerText = p.UPrice;
                xUserName.InnerText = p.UserName;
                xTop.AppendChild(xStore);
                xTop.AppendChild(xItem);
                xTop.AppendChild(xPrice);
                xTop.AppendChild(xUserName);
                xdoc.DocumentElement.AppendChild(xTop);

            }
            users.Clear();
            items.Clear();
            xdoc.Save("Listerz\\UserListerz.xml");
        }

        /// <summary>
        /// Logic for button, when clicked, that clears all items in the users list in ListOfItems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            ListOfItems.Items.Clear();

            users.Clear();
            //label.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Changes what items appear in ListOfItems when the user selects another user list to view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListOfItems.Visibility = Visibility.Visible;
            UserLists.Visibility = Visibility.Hidden;
            OpenUser.Visibility = Visibility.Visible;
            CloseUser.Visibility = Visibility.Hidden;
            label.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            Done.Visibility = Visibility.Visible;
            ListOfItems.Items.Clear();
            Remove_List.Visibility = Visibility.Visible;
            Remove_Item.Visibility = Visibility.Visible;
            Share.Visibility = Visibility.Visible;
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\UserListerz.xml");
            foreach (XmlNode xnode in xdoc.SelectNodes("UserLists/User"))
            {
                User p = new User();
                p.UserName = xnode.SelectSingleNode("UserName").InnerText;
                if (UserLists.SelectedItem == null) { continue; }
                else
                {
                    if (p.UserName == UserLists.SelectedItem.ToString())
                    {
                        Usernames= xnode["UserName"].InnerText;
                        label.Content = xnode["UserName"].InnerText;
                        ListOfItems.Items.Add(xnode["Item"].InnerText + "     $" + xnode["Price"].InnerText + "  @" + xnode["Store"].InnerText);
                    }
                }
            }
        }
        /// <summary>
        /// Clears textBox when the user selects it.
        /// </summary>
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";

        }

        /// <summary>
        /// Logic for button, when clicked, calls AddToDoc() and resets the window back to the 'pre-list' veiw.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            UserLists.Items.Remove(label.Content);

            AddToDoc();
            ListOfItems.Items.Clear();            
            UserLists.Items.Add(label.Content);
            CreateList.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Visible;
            textBox.Text = "";
            ListItems.Visibility = Visibility.Hidden;
            ListOfItems.Visibility = Visibility.Hidden;
            label.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            Done.Visibility = Visibility.Hidden;
            Remove_Item.Visibility = Visibility.Hidden;
            Remove_List.Visibility = Visibility.Hidden;
            Clear_List.Visibility = Visibility.Hidden;
            Share.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// Logic for button, when clicked, calls RemoveList()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveList_Click(object sender, RoutedEventArgs e)
        {
            RemoveLists();
        }
        /// <summary>
        /// Logic for button, when clicked, removes the selected item from its list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\UserListerz.xml");

            foreach (XmlNode xnode in xdoc.SelectNodes("UserLists/User"))
            {

                User p = new User();
                p.UserName = xnode.SelectSingleNode("UserName").InnerText;
                p.UItm = xnode.SelectSingleNode("Item").InnerText;
                if (ListOfItems.SelectedItem == null) { textBox1.Visibility = Visibility.Visible; continue; }
                else
                {
                    if (p.UserName == UserLists.SelectedItem.ToString())
                    {
                        if (ListOfItems.SelectedItem.ToString() == ((xnode["Item"].InnerText + "     $" + xnode["Price"].InnerText + "  @" + xnode["Store"].InnerText).ToString())) 
                            {
                            XmlNode node = xdoc.SelectSingleNode(String.Format("/UserLists/User[Item='{0}' and UserName='{1}']", p.UItm, p.UserName));
                            node.ParentNode.RemoveChild(node);
                            xdoc.Save("Listerz\\UserListerz.xml");
                            //users.Remove(p);
                            ListOfItems.Items.Remove(ListOfItems.SelectedItem);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Removes the selected list from the xml document.
        /// </summary>
        private void RemoveLists()
        {
            string DeleteUser = "";
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\UserListerz.xml");
            User p = new User();
            foreach (XmlNode xnode in xdoc.SelectNodes("UserLists/User"))
            {

                p.UserName = xnode.SelectSingleNode("UserName").InnerText;
                DeleteUser = p.UserName;
                if (UserLists.SelectedItem == null) { continue; }
                else
                {
                    if (p.UserName == UserLists.SelectedItem.ToString())
                    {
                        XmlNode node = xdoc.SelectSingleNode(String.Format("/UserLists/User[UserName='{0}']", p.UserName));
                        node.ParentNode.RemoveChild(node);
                        xdoc.Save("Listerz\\UserListerz.xml");
                        users.Remove(p);

                        ListOfItems.Items.Clear();
                        label.Visibility = Visibility.Hidden;
                        Done.Visibility = Visibility.Hidden;
                        Share.Visibility = Visibility.Hidden;
                    }
                }
            }
            UserLists.Items.Remove(UserLists.SelectedItem);
            Remove_List.Visibility = Visibility.Hidden;
            Remove_Item.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Logic for button, when clicked, calls adds the selected lists items to a txt file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareList(object sender, RoutedEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("Listerz\\UserListerz.xml");
            User p = new User();
            List<string> testitems = new List<string>();
            foreach (XmlNode xnode in xdoc.SelectNodes("UserLists/User"))
            {
                
                p.UserName = xnode.SelectSingleNode("UserName").InnerText;
                if (UserLists.SelectedItem == null) { continue; }
                else
                {
                    if (p.UserName == UserLists.SelectedItem.ToString())
                    {
                        
                           testitems.Add(xnode["Item"].InnerText + "     $" + xnode["Price"].InnerText + "  @" + xnode["Store"].InnerText);
                        
                    }

                }
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"Listerz\\" + UserLists.SelectedItem.ToString() + ".txt"))
                foreach (string s in testitems)
            {
                file.WriteLine(s);
            }
            Done_Click(sender, e);
        }
        
    }
}
