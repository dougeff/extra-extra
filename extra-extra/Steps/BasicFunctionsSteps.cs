using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        private static string _browserToLaunch;

        [BeforeFeature]
        public static void LaunchApp()
        {
            _browserToLaunch = "chrome";
            ChangeConfig();
            _applicationToTest = Application.Launch(@"..\..\..\extra-extra\bin\Debug\extra-extra.exe");
            _startingWindow = _applicationToTest.GetWindow("Extra Extra");
            _queryText = "Blah";
            CloseBrowser();
        }

        private static void ChangeConfig()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["browserToLaunch"].Value = _browserToLaunch;
            configuration.Save();
            ConfigurationManager.RefreshSection("appsettings");
        }

        private static void CloseBrowser()
        {
            foreach (var process in Process.GetProcessesByName(_browserToLaunch).Where(process => !process.HasExited))
            {
                process.Kill();
            }
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

        [Given(@"I have left the text field blank")]
        public void GivenIHaveLeftTheTextFieldBlank()
        {
            var textQuery = _startingWindow.Get<TextBox>("TextQuery");
            textQuery.Click();
        }

        [When(@"I click one of the results")]
        public void WhenIClickOneOfTheResults()
        {
            var queryResults = _startingWindow.Get<TreeNode>(_queryText);
            queryResults.Expand();
            var firstNode = queryResults.Nodes[0];
            firstNode.Click();
        }

        [Then(@"it should take me to a website")]
        public void ThenItShouldTakeMeToAWebsite()
        {
            Thread.Sleep(1000);
            var browserId = Process.GetProcessesByName(_browserToLaunch).Select(p => p.Id).FirstOrDefault();
            browserId.ShouldBeGreaterThan(0);
            CloseBrowser();
        }

        [Then(@"it should not fetch query results")]
        public void ThenItShouldNotFetchQueryResults()
        {
            var queryResults = _startingWindow.Get<Tree>("TreeItemsList");
            queryResults.Nodes.Count().ShouldEqual(0);
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

        [Then(@"it should not redisplay the same items")]
        public void ThenItShouldNotRedisplayTheSameItems()
        {
            var queryResults = _startingWindow.Get<TreeNode>(_queryText);
            queryResults.Expand();
            var firstNodeId = queryResults.Nodes[0].Id;
            var queryResultNodes =
               from qrNodes in queryResults.Nodes.Where(queryResultNodesId => queryResultNodesId.Id == firstNodeId)
               select qrNodes;
            queryResultNodes.Count().ShouldEqual(1);
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
            _browserToLaunch = "";
            ChangeConfig();
            _startingWindow.Close();
        }
    }
}