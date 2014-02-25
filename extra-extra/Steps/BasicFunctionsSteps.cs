using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Controls;
using Should;
using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using Button = TestStack.White.UIItems.Button;
using TextBox = TestStack.White.UIItems.TextBox;

namespace extra_extra.Steps
{
    [Binding]
    public class BasicFunctionsSteps : TechTalk.SpecFlow.Steps
    {
        private static Application _applicationToTest;
        private static Window _startingWindow;
        private static string _queryText;

        [BeforeFeature]
        public static void LaunchApp()
        {
            _applicationToTest = Application.Launch(@"..\..\..\extra-extra\bin\Debug\extra-extra.exe");
            _startingWindow = _applicationToTest.GetWindow("Extra Extra");
            _queryText = "Blah";
        }

        [Given(@"I have entered text into the query field")]
        public void GivenIHaveEnteredTextIntoTheQueryField()
        {
            var textQuery = _startingWindow.Get<TextBox>("TextQuery");
            textQuery.Enter(_queryText);
        }

        [Given(@"I have a query result")]
        public void GivenIHaveAQueryResult()
        {
            Given(@"I have entered text into the query field");
            And(@"I press search");
            And(@"it should fetch query results");


            //var queryResultHeader = new TreeViewItem
            //    {
            //        Header = string.Format("{0} - {1} results returned", _queryText, 1),
            //        Name = _queryText
            //    };
            //var queryResultList = new TreeViewItem
            //{
            //    Header = string.Format("{0}. {1}", 1, "Blah Headline 1"),
            //    Uid = "1"
            //};
            //queryResultHeader.Items.Add(queryResultList);
        }

        [Given(@"I have set the time interval")]
        public void GivenIHaveSetTheTimeInterval()
        {
            var intervalDropDown = _startingWindow.Get<IUIItem>("SelectInterval");
            intervalDropDown.SetValue("30 Seconds");
        }

        [When(@"I press search")]
        [Given(@"I press search")]
        public void WhenIPressSearch()
        {
            var queryButton = _startingWindow.Get<Button>("ButtonQuery");
            queryButton.Click();
        }

        [Then(@"it should fetch query results")]
        [Given(@"it should fetch query results")]
        public void ThenItShouldFetchQueryResults()
        {
            var queryResult = _startingWindow.Get<IUIItem>(_queryText);
            queryResult.ShouldNotBeNull();
        }

        [Then(@"it should not redisplay the results")]
        public void ThenItShouldNotRedisplayTheResults()
        {
            var queryResults = _startingWindow.Get<Tree>("TreeItemsList");
            var queryResultNodes =
                from qrNodes in queryResults.Nodes.Where(queryResultNodesId => queryResultNodesId.Id == _queryText)
                select qrNodes;
            queryResultNodes.Count().ShouldEqual(1);
        }

        [When(@"the interval ends")]
        public void WhenTheIntervalEnds()
        {

            ScenarioContext.Current.Pending();
        }

        [Then(@"the results number should be displayed")]
        public void ThenTheResultsNumberShouldBeDisplayed()
        {
            var queryResult = _startingWindow.Get<IUIItem>(_queryText);
            queryResult.Name.ShouldContain("results returned");
        }

        [AfterFeature]
        public static void CloseWindow()
        {
            _startingWindow.Close();
        }
    }
}