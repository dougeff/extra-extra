using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using Moq;
using NUnit.Framework;
using Should;
using TreeView = System.Windows.Controls.TreeView;

namespace extra_extra.UnitTests
{
    [TestFixture]
    public class MainTests
    {
        public string XmlUrl = @"..\..\UnitTests\Resources\dummy.xml";
        public string Query = "blah";
        public XmlNodeList ResultsFetched { get; set; }
        public XmlNodeList NoResultsFetched { get; set; }

        //        [Test]
        //        public void BadQueryShouldGetEmptyResult()
        //        {
        //            var webClientWrapper = new Mock<IXmlWrapper>();
        //            webClientWrapper
        //               .Setup(x => x.FetchXml(It.IsAny<string>()))
        //               .Returns(new EmptyXmlNodeList());
        //
        //            
        //
        //        }

        [SetUp]
        public void RestInit()
        {
            ResultsFetched = MainWindow.FetchXml(XmlUrl, Query);
            NoResultsFetched = MainWindow.FetchXml("blah.xml", Query);
        }

        [Test]
        public void QueryShouldFetchResults()
        {
            ResultsFetched.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void ExceptionShouldReturnEmptyList()
        {
            NoResultsFetched.Count.ShouldEqual(0);
        }

        [Test]
        public void UidShouldBeRetrieved()
        {
            var uid = MainWindow.GetUid(ResultsFetched[0]);
            uid.ShouldEqual("tag:news.blah.com");
        }

        [Test]
        public void LinkShouldBeRetrieved()
        {
            var link = MainWindow.GetLink(ResultsFetched[0]);
            link.NavigateUri.ToString().ShouldEqual("http://news.blah.com/");
        }

        [Test]
        public void TitleShouldBeRetrieved()
        {
            var title = MainWindow.GetTitle(ResultsFetched[0]);
            title.ShouldEqual("Dummy Item Title");
        }

//        [Test]
//        [STAThread]
//        public void ResultsShouldNotShowIfAlreadyDisplayed()
//        {
//            var treeview = new TreeView();
//            var treeviewItem = new TreeViewItem
//                {
//                    Header = Query,
//                    Name = Query
//                };
//            treeview.Items.Add(treeviewItem);
//            var foundTreeViewItem = MainWindow.GetTreeHeader(Query, treeview);
//            foundTreeViewItem.Name.ShouldEqual(Query);
//        }
    }
}
