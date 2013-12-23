using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace extraextra.Steps
{
    [Binding]
    public class BasicFunctionsSteps
    {
        [Given(@"I have entered text into the query field")]
        public void GivenIHaveEnteredTextIntoTheQueryField()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I press search")]
        public void WhenIPressSearch()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"it should fetch query results")]
        public void ThenItShouldFetchQueryResults()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"they should be minimized with a button to expand")]
        public void ThenTheyShouldBeMinimizedWithAButtonToExpand()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
