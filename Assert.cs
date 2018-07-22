using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;

namespace SeleniumFramework
{

	public class Assert
		{
			private Driver _driver;
			public bool AssertionsFailed { get; private set; }
			public Assert(Driver driver)
			{
				_driver = driver;
				AssertionsFailed = false;
			}
			private void TryAndLog(Action assertion, string message)
			{
				try
				{
					assertion.Invoke();
					_driver.TakeScreenshot(Status.Pass, message);
				}
				catch
				{
					_driver.TakeScreenshot(Status.Error, message);
					AssertionsFailed = true;
				}
			}
			public void Fail(string message)
			{
				TryAndLog(() => NUnit.Framework.Assert.Fail(), message);
			}
			public void AreEqual(object expected, object actual, string message = "")
			{
				TryAndLog(() => NUnit.Framework.Assert.AreEqual(expected, actual), "Expected = '" + expected + "'; Actual = '" + actual + "; " + message);
			}
			public void IsTrue(bool condition, string message)
			{
				TryAndLog(() => NUnit.Framework.Assert.IsTrue(condition), message);
			}
			public void IsFalse(bool condition, string message)
			{
				TryAndLog(() => NUnit.Framework.Assert.IsFalse(condition), message);
			}
			public void Pass(string message)
			{
				IsTrue(true, message);
			}
			public void NotNull(object anObject, string message)
			{
				TryAndLog(() => NUnit.Framework.Assert.NotNull(anObject), message);
			}
			public void Greater(int arg1, int arg2, string message)
			{
				TryAndLog(() => NUnit.Framework.Assert.Greater(arg1, arg2), message);

			}
		}
	
}
