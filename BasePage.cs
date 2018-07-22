using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFramework
{
	
		public class BasePage
		{
			public Driver Driver { get; private set; }
			public Assert Assert { get; private set; }
			public RandomHelper Random { get; private set; }
			private WebDriverWait _wait;
			private IJavaScriptExecutor _executor;


			public BasePage(Driver driver, Assert assert)
			{
				Driver = driver;
				Assert = assert;
				Random = new RandomHelper();
				//	WaitForLoadIndicator();
			}
			//public void ClickHamburgerMenu()
			//{
			//	Driver.Click(btnHamburgerMenu, "Click hamburger menu.");
			//	Driver.AssertDisplayed(btnUserManagement, Assert, "Wait for user management button to be displayed.");
			//}
		}
	
}
