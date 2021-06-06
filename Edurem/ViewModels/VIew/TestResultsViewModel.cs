using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.ViewModels
{
    class TestResultsView
    {
        public int CountOfTests { get; set; }

        public int CountOfCompletedTests { get; set; }

        public List<(string TestName, string ResultHtml)> Results { get; set; }

        public (bool IsTested, DateTime Time) LastTestTime { get; set; }

        public User User { get; set; }

        public bool HasError { get; set; }

        public TestResultsView()
        {
            CountOfTests = 0;
            CountOfCompletedTests = 0;
            Results = new();
            HasError = false;
            User = new();
        }

        public TestResultsView(TestInfo testInfo, List<(string TestName, string ResultHtml)> results)
        {
            CountOfTests = testInfo.CountOfTests;
            CountOfCompletedTests = testInfo.CountOfCompletedTests;
            Results = results;
            LastTestTime = (true, testInfo.DateOfTesting);
            User = testInfo.User;

            HasError = testInfo.ResultText.Contains("[ERROR]");
        }

        public double GetProgress()
        {
            return CountOfTests != 0 ? (double)CountOfCompletedTests / CountOfTests : 0;
        }
    }
}
