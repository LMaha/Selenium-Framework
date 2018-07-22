using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.IO;
using System.Configuration;

namespace SeleniumFramework
{
	public class BaseTest : IDisposable
	{
		public HomePage HomePage { get; private set; }

		public RandomHelper Random { get; private set; }
		public static ExtentReports Reporter { get; private set; }
		public static ExtentTest Test { get; private set; }
		private IWebDriver _driver;
		private bool _testFinished;
		[SetUp]
		public void Setup()
		{
			var path = Path.Combine(Path.GetDirectoryName(new Uri(typeof(BaseTest).Assembly.CodeBase).LocalPath), TestContext.CurrentContext.Test.FullName);
			var htmlreporter = new ExtentHtmlReporter(Path.Combine(path, "extent.html"));
			Reporter = new ExtentReports();
			Reporter.AttachReporter(htmlreporter);
			Test = Reporter.CreateTest(TestContext.CurrentContext.Test.FullName);
			Setup(Test, path);
			_testFinished = false;
		}
		public BaseTest() { }
		public BaseTest(ExtentTest reporter, string path)
		{
			Setup(reporter, path);
		}
		public void Setup(ExtentTest reporter, string path)
		{
			Test = reporter;
			var runOnLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["RUN_ON_LOCAL"]);
			if (!runOnLocal)
			{
				ICapabilities capability = new ChromeOptions().ToCapabilities();
				_driver = new RemoteWebDriver(new Uri("http://localhost29:4444/wd/hub"), capability);
			}
			else
			{
				_driver = new ChromeDriver();
				_driver.Manage().Window.Maximize();
				_driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["URL"]);

			}
			//Initalize Driver class here
			var driver = new Driver(_driver, reporter, path);
			//For every page declared here new initalization
			HomePage = new HomePage(driver, new Assert(driver));
			Random = new RandomHelper();
			
		}
		[TearDown]
		public void Dispose()
		{
			if (!_testFinished)
			{
				try
				{
					if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
					{
						Test.Fail(TestContext.CurrentContext.Result.Message);
					}
					else if (HomePage.Assert.AssertionsFailed)
					{
						Test.Fail(TestContext.CurrentContext.Result.Message);
						if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
						{
							throw new Exception("Test failed. See report for details.");
						}
					}
					else
					{
						Test.Pass("Test finished successfully.");
					}
				}
				catch { }
			}
			_testFinished = true;
			_driver.Quit();
			_driver.Dispose();
		}
		[OneTimeTearDown]
		public void RunAfterAnyTests()
		{
			Reporter.Flush();
		}
	}
}