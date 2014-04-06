using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;
using System.Xml;
using mshtml;


namespace extra_extra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private static String _googleUrl, _defaultText;

        public MainWindow()
        {
            InitializeComponent();
            _dispatcherTimer = new DispatcherTimer();
            PopulateTimeIntervals();
            _googleUrl = "http://news.google.com/news?pz=1&cf-all&ned=us&hl=en&q={0}&cf=all&output=rss";
            _defaultText = "Enter your query here";
        }

        private void ButtonQuery_Click(object sender, RoutedEventArgs e)
        {
            FetchResults();
        }

        private void FetchResults()
        {
            var queryToGet = TextQuery.Text;
            if (queryToGet.Length > 0 && queryToGet != _defaultText)
            {
                var feedUrl = String.Format(_googleUrl, queryToGet);
                
                var xml = new XmlDocument();
                try
                {
                    xml.Load(feedUrl);
                }
                catch (Exception ex)
                {
                    var str = ex.ToString();
                    TextQuery.Text = str;
                }

                var xmlNodes = xml.SelectNodes("//item");
                if (xmlNodes == null)
                {
                    return;
                }

                TreeViewItem queryHeader, foundQueryHeader = null;
                var queryHeaderItemUidFound = false;
                var queryHeaderName = char.ToUpper(queryToGet[0]) + queryToGet.Replace(" ", "").Substring(1);

                foreach (var treeListItemName in TreeItemsList.Items.Cast<FrameworkElement>())
                {
                    if (treeListItemName.Name != queryHeaderName)
                    {
                        continue;
                    }
                    foundQueryHeader = (TreeViewItem)treeListItemName;
                }

                if (foundQueryHeader != null)
                {
                    queryHeader = foundQueryHeader;
                }
                else
                {
                    queryHeader = new TreeViewItem
                    {
                        Header = string.Format("{0} - {1} results returned", queryToGet, xmlNodes.Count),
                        Name = queryHeaderName
                    };
                }

                var itemCount = 0;
                foreach (XmlNode xmlNode in xmlNodes)
                {
                    var titleNode = xmlNode.SelectNodes("title");
                    if (titleNode == null)
                    {
                        continue;
                    }
                    var guidNode = xmlNode.SelectNodes("guid");
                    if (guidNode == null)
                    {
                        continue;
                    }
                    var articleTitle = titleNode.Item(0);
                    if (articleTitle == null)
                    {
                        continue;
                    }
                    var articleId = guidNode.Item(0);
                    if (articleId == null)
                    {
                        continue;
                    }
                    var uid = articleId.InnerText;
                    var linkNode = xmlNode.SelectNodes("link");
                    if (linkNode == null)
                    {
                        continue;
                    }
                    var webLink = linkNode.Item(0);
                    if (webLink == null)
                    {
                        continue;
                    }

                    foreach (TreeViewItem queryHeaderItem in queryHeader.Items)
                    {
                        if (queryHeaderItem.Uid != uid)
                        {
                            continue;
                        }
                        queryHeaderItemUidFound = true;
                    }
                    ++itemCount;
                    if (queryHeaderItemUidFound)
                    {
                        continue;
                    }
                    var queryList = new TreeViewItem
                        {
                            Header = string.Format("{0}. {1}", itemCount, articleTitle.InnerText),
                            Uid = uid,
                            Tag = webLink.InnerText
                        };
                    queryList.Selected += ListItemClick;
                    queryHeader.Items.Add(queryList);
                }
                if (itemCount > 0)
                {
                    if (foundQueryHeader == null)
                    {
                        TreeItemsList.Items.Add(queryHeader);
                    }
                }
                else
                {
                    var nothingReturned = new TreeViewItem
                    {
                        Header = string.Format("Nothing found for {0}", queryToGet),
                        Name = queryToGet
                    };
                    TreeItemsList.Items.Add(nothingReturned);
                }
            }
        }

        private void TextQuery_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextQuery.Text == _defaultText)
            {
                TextQuery.Text = "";
            }
        }

        private void TextQuery_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextQuery.Text == "")
            {
                TextQuery.Text = _defaultText;
            }
        }

        private void SelectInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTimeInterval = e.AddedItems[0] as ComboBoxItem;
            if (selectedTimeInterval == null)
            {
                throw new InvalidOperationException("Time Interval selected is null");
            }

            if ((string) selectedTimeInterval.Content == "Off")
            {
                _dispatcherTimer.Stop();
            }
            else
            {
                _dispatcherTimer.Tick += dispatcherTimer_Tick;
                _dispatcherTimer.Interval = (TimeSpan) selectedTimeInterval.Tag;
                _dispatcherTimer.Start();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            FetchResults();
        }

        private void PopulateTimeIntervals()
        {
            SelectInterval.DataContext = TimeInterval.GetIntervals();
        }

        private static void ListItemClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var clickedItem = (FrameworkElement) sender;
            var website = clickedItem.Tag;
            //System.Diagnostics.Debugger.Launch();
            var browserToLaunch = ConfigurationManager.AppSettings["browserToLaunch"];
            if (String.IsNullOrEmpty(browserToLaunch))
            {
                System.Diagnostics.Process.Start(website.ToString());
            }
            else
            {
                System.Diagnostics.Process.Start(browserToLaunch, website.ToString());
            }
        }
    }
}
