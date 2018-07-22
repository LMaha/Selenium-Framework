using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace SeleniumFramework
{
	public class HomePage : BasePage
	{
		public HomePage(Driver driver, Assert assert) : base(driver, assert) { }
		
		private By txtUsername = By.XPath("//*[@id= 'page_x002e_components_x002e_slingshot-login_x0023_default-username']");
		private By txtPassword = By.XPath("//*[@id='page_x002e_components_x002e_slingshot-login_x0023_default-password']");
		private By btnLogin = By.XPath("//*[@id='page_x002e_components_x002e_slingshot-login_x0023_default-submit-button']");
		private By heading = By.XPath("//*[@id='HEADER_TITLE']/span");


		public void Login()
		{
			Driver.SendKeys(txtUsername, "admin", "Enter User Name");
			Driver.SendKeys(txtPassword, "formtek", "Enter Password");
			Driver.Click(btnLogin, "Click login");		
			//_wait.Until(d => d.FindElement(heading));
			Driver.WaitForCondition(d =>
				{
					d.FindElement(heading);
				}, "Get alerts.");
			Driver.IsDisplayed(heading, "Verify main page has displayed");
		}
	}
}

