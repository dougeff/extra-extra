using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Should;
using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using Button = TestStack.White.UIItems.Button;
using TextBox = TestStack.White.UIItems.TextBox;

namespace extra_extra.Steps
{
    [Binding]
    public class BasicFunctionsSteps
    {
        private static Application _applicationToTest;
        private static Window _startingWindow;
        private static string _queryText;

        [BeforeFeature]
        public static void LaunchApp()
        {
            _applicationToTest = Application.Launch(@"..\..\..\extra-extra\bin\Debug\extra-extra.exe");
            _startingWindow = _applicationToTest.GetWindow("Extra Extra");
            _queryText = "blah";
        }


        [Given(@"I have entered text into the query field")]
        public void GivenIHaveEnteredTextIntoTheQueryField()
        {
            var textQuery = _startingWindow.Get<TextBox>("TextQuery");
            textQuery.Enter(_queryText);
        }

        [When(@"I press search")]
        public void WhenIPressSearch()
        {
            var queryButton = _startingWindow.Get<Button>("ButtonQuery");
            queryButton.Click();
        }

        [Then(@"it should fetch query results")]
        public void ThenItShouldFetchQueryResults()
        {
            var queryResult = _startingWindow.Get<IUIItem>(_queryText);
            queryResult.ShouldNotBeNull();
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