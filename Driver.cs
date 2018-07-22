using AventStack.ExtentReports;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumFramework
{
	public class Driver
	{
		private IWebDriver _driver;
		private WebDriverWait _wait;
		private ExtentTest _reporter;
		private string _path;
		private IJavaScriptExecutor _executor;
		private RandomHelper _random;

		public By FindElementByText(By by)
		{
			throw new NotImplementedException();
		}

		public Driver(IWebDriver driver, ExtentTest reporter, string path)
		{
			_driver = driver;
			_wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["TIMEOUT"])));
			_reporter = reporter;
			_path = path;
			_executor = _driver as IJavaScriptExecutor;
			_random = new RandomHelper();
		}

		public static object Navigate()
		{
			throw new NotImplementedException();
		}

		public void WaitForCondition(Action<IWebDriver> condition, string comment)

		{
			try
			{
				_wait.Until(d =>
				{
					try
					{
						condition.Invoke(d);
						TakeScreenshot(Status.Info, comment);
						return true;
					}
					catch
					{
						return false;
					}
				});
			}
			catch
			{
				TakeScreenshot(Status.Error, comment);
				condition.Invoke(_driver);
			}
		}
		public void WaitForCondition(Action<IWebDriver> condition, TimeSpan @override, string comment)
		{
			_wait.Timeout = @override;
			WaitForCondition(condition, comment);
			_wait.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["TIMEOUT"]));
		}
		public void rq(Func<int, int> getreuslts)
		{
		}
		public void WaitForCondition(Func<IWebDriver, bool> condition, string comment)
		{
			try
			{
				_wait.Until(d =>
				{
					try
					{
						return condition.Invoke(d);
					}
					catch
					{
						return false;
					}
				});

			}

			catch
			{
				TakeScreenshot(Status.Error, comment);
				condition.Invoke(_driver);
			}
			TakeScreenshot(Status.Info, comment);
		}

		public void WaitForCondition(Func<IWebDriver, bool> condition, TimeSpan @override, string comment)
		{
			_wait.Timeout = @override;
			WaitForCondition(condition, comment);
			_wait.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["TIMEOUT"]));
		}
		public void SendKeys(By locator, string text, string comment)
		{
			WaitForCondition(d =>
			{
				var element = d.FindElement(locator);
				element.Clear();
				element.SendKeys(text);
			}, comment);
		}
		public void SendKeysToAll(By locator, string text, string comment)
		{
			var elements = _driver.FindElements(locator);
			foreach (var element in elements)
			{
				element.SendKeys("1000");
				TakeScreenshot(Status.Info, comment);
			}
		}
		public void SelectFromDropdown(By locator, string text, string comment)
		{
			WaitForCondition(d =>
			{
				new SelectElement(d.FindElement(locator)).SelectByText(text);
			}, comment);
		}
		public void SelectFromDropdown(By locator, int index, string comment)
		{
			WaitForCondition(d =>
			{
				new SelectElement(d.FindElement(locator)).SelectByIndex(index);
			}, comment);
		}
		public void SelectFromVisibleDropdown(By locator, int index, string comment)
		{
			WaitForCondition(d =>
			{
				var element = d.FindElements(locator).First(e => e.Displayed);
				new SelectElement(element).SelectByIndex(index);
			}, comment);
		}
		public void Click(By locator, string comment)
		{
			WaitForCondition(d =>
			{
				d.FindElement(locator).Click();
			}, comment);
		}
		public void Sleep()
		{
			Thread.Sleep(1000);
		}
		public bool IsElementVisible(IWebElement element)
		{
			var location = element.Location;
			var windowSize = _driver.Manage().Window.Size;
			return location.Y < windowSize.Height - 100;
		}
		public void ClickAll(By locator, string comment)
		{
			var elements = _driver.FindElements(locator);
			foreach (var element in elements)
			{
				element.Click();
				TakeScreenshot(Status.Info, comment);
			}
		}
		public void ClickVisible(By locator, string comment)
		{
			_driver.FindElements(locator).First(e => e.Displayed).Click();
			TakeScreenshot(Status.Info, comment);
		}
		public void AssertDisplayed(By locator, Assert assert, string comment)
		{
			WaitForCondition(d =>
			{
				assert.IsTrue(d.FindElement(locator).Displayed, comment);
			}, comment);
		}
		public bool IsDisplayed(By locator, string comment)
		{
			try
			{
				var element = _driver.FindElement(locator);

				return element.Displayed;
			}
			catch
			{
				return false;
			}
		}
		public bool IsEnabled(By locator, string comment)
		{
			try
			{
				var elements = _driver.FindElements(locator);
				return elements.Any(e => e.Enabled);
			}
			catch
			{
				return false;
			}
		}
		public string GetAttributeValue(By locator, string attribute)
		{
			return _driver.FindElement(locator).GetAttribute(attribute);
		}
		public string GetElementText(By locator)
		{
			return _driver.FindElement(locator).Text;
		}
		public void ClickByJavascript(By locator, string comment)
		{
			WaitForCondition(d =>
			{
				var element = _driver.FindElement(locator);
				ClickByJavascript(element);
			}, comment);
		}
		public void ClickByJavascript(IWebElement element)
		{
			_executor.ExecuteScript("arguments[0].click()", element);
		}
		public int GetNumRowsOfGrid(string id)
		{
			return Convert.ToInt32(_executor.ExecuteScript("return jQuery('#" + id + "').data('kendoGrid').dataSource.data().length"));
		}
		public string GetGridValue(string id, int index, string value)
		{
			return Convert.ToString(_executor.ExecuteScript("return jQuery('#" + id + "').data('kendoGrid').dataSource.data()[" + index + "]." + value));
		}
		public int GetGridRowWithCellText(string id, string text)
		{
			int row;
			int col;
			var result = -1;
			Thread.Sleep(1000);
			var rowCount = Convert.ToInt32(_executor.ExecuteScript("return jQuery('#" + id + "').data('kendoGrid').items().length"));
			var columnCount = Convert.ToInt32(_executor.ExecuteScript("return jQuery('#" + id + "').data('kendoGrid').columns.length"));
			for (row = 1; row <= rowCount; row++)
			{
				for (col = 1; col <= columnCount; col++)
				{
					var cell = _executor.ExecuteScript("return jQuery('#" + id + " tr:nth-child(" + row + "):visible td:nth-child(" + col + "):visible')");
					var textActual = _executor.ExecuteScript("return jQuery('#" + id + " tr:nth-child(" + row + "):visible td:nth-child(" + col + "):visible').text()");
					if ((string)textActual == text)
					{
						result = row;
						return Convert.ToInt32(result);
					}
				}
			}
			return result;
		}
		public T GetGridRowById<T>(string gridId, string rowId, string id, int index = 1)
		{
			var numRows = GetNumRowsOfGrid(gridId);
			var numTimesFound = 0;
			for (int i = 1; i <= numRows; i++)
			{
				if (GetGridValue(gridId, i - 1, rowId) == id)
				{
					numTimesFound++;
					if (numTimesFound >= index)
					{
						var json = _executor.ExecuteScript("return JSON.stringify(jQuery('#" + gridId + "').data('kendoGrid').dataSource.data()[" + (i - 1) + "])") as string;
						return JsonConvert.DeserializeObject<T>(json);
					}
				}
			}
			return default(T);
		}
		public void SwitchWindow()
		{
			foreach (var handle in _driver.WindowHandles)
			{
				_driver.SwitchTo().Window(handle);
			}
		}
		public void CloseWindow()
		{
			_driver.Close();
			SwitchWindow();
		}
		public void GoToUrl(string url)
		{
			_driver.Navigate().GoToUrl(url);
		}
		public void Refresh()
		{
			_driver.Navigate().Refresh();
		}
		public void AssertTitle(string title, Assert assert)
		{
			WaitForCondition(d => assert.AreEqual(title, d.Title), "Assert title equals '" + title + "'.");
		}
		public void AssertText(By locator, string text, Assert assert)
		{
			assert.AreEqual(text, _driver.FindElement(locator).Text);
		}
		public void AssertTextContains(By locator, string text, Assert assert, string message)
		{
			assert.IsTrue(_driver.FindElement(locator).Text.Contains(text), message);
		}
		public string GetText(By locator)
		{
			return _driver.FindElement(locator).Text;
		}
		public void TakeScreenshot(Status status, string comment)
		{
			if (_reporter != null)
			{
				var screenshot = (_driver as ITakesScreenshot).GetScreenshot();
				var filename = _random.RandomString() + ".png";
				var path = Path.Combine(_path, filename);
				screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
				_reporter.Log(status, comment, MediaEntityBuilder.CreateScreenCaptureFromPath(filename).Build());
			}
		}
		public static By FindElementByText(string text)
		{
			return By.XPath("//*[text() = '" + text + "']");
		}
		public static By FindElementByTextContains(string text)
		{
			return By.XPath("//*[contains(text(), '" + text + "')]");
		}
		public static By FindElementByPropertyContains(string property, string text)
		{
			return FindElementByTagAndPropertyContains("*", property, text);
		}
		public static By FindElementByPropertyEquals(string property, string text)
		{
			return By.XPath("//*[@" + property + " = '" + text + "']");
		}
		public static By FindElementByTagAndPropertyContains(string tag, string property, string text)
		{
			return By.XPath("//" + tag + "[contains(@" + property + ", '" + text + "')]");
		}
		public static By FindElementByTagAndText(string tag, string text)
		{
			return By.XPath("//" + tag + "[text() = '" + text + "']");
		}
	}
}
