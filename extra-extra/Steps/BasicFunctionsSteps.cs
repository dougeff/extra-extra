using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace extra_extra.Steps
{
    [Binding]
    public class BasicFunctionsSteps
    {
        private static Application _applicationToTest;
        private static Window _startingWindow;

        [BeforeFeature]
        public static void LaunchApp()
        {
            _applicationToTest = Application.Launch(@"..\..\..\extra-extra\bin\Debug\extra-extra.exe");
            _startingWindow = _applicationToTest.GetWindow("Extra Extra");
        }


        [Given(@"I have entered text into the query field")]
        public void GivenIHaveEnteredTextIntoTheQueryField()
        {
            var textQuery = _startingWindow.Get<TextBox>("TextQuery");
            textQuery.Enter("blah");
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
            //assert at least one result is fetched
        }

        [Then(@"they should be minimized with a button to expand")]
        public void ThenTheyShouldBeMinimizedWithAButtonToExpand()
        {
            ScenarioContext.Current.Pending();
        }

        [AfterFeature]
        public static void CloseWindow()
        {
            _startingWindow.Close();
        }
    }
}