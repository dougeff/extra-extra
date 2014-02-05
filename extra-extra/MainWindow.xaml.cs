using System;
using System.Collections.Generic;
using System.IO;
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

namespace extra_extra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonQuery_Click(object sender, RoutedEventArgs e)
        {
            var queryToGet = TextQuery.Text;
            var feedUrl = String.Format("http://news.google.com/news?pz=1&cf-all&ned=us&hl=en&q={0}&cf=all&output=rss",
                            queryToGet);

            var xml = new XmlDocument();
            string str;
            try
            {
                xml.Load(feedUrl);
            }
            catch (Exception ex)
            {
                str = ex.ToString();
                TextQuery.Text = str;
            }

//            var sw = new StringWriter();
//            var tx = new XmlTextWriter(sw);
//            xml.WriteTo(tx);
//
//            str = sw.ToString();
            
            var xmlNodes = xml.SelectNodes("//item");
            if (xmlNodes == null)
            {
                return;
            }
            
            var queryHeader = new TreeViewItem
                {
                    Header = string.Format("{0} - {1} results returned", queryToGet, xmlNodes.Count),
                    Name = queryToGet
                };

            TreeItemsList.Items.Add(queryHeader);
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
                var treeViewItem = new TreeViewItem
                    {
                        Header = string.Format("{0}. {1}", ++itemCount, articleTitle.InnerText),
                        Uid = articleId.InnerText
                    };

                queryHeader.Items.Add(treeViewItem);
            }


            //todo: working on add results to some kind of list

            


            







        }
    }
}
