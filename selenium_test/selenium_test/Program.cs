using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selenium_test
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            //Console.WriteLine("Hello World!");
            //Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
            IWebDriver driver = new ChromeDriver();

            //Navigate to google page
            //driver.Navigate().GoToUrl("http:www.google.com");
            driver.Navigate().GoToUrl("http://booking.com");

            //Find the Search text box UI Element
            //IWebElement element = driver.FindElement(By.Name("q"));
            IWebElement element = driver.FindElement(By.Name("ss"));
            IWebElement element1 = driver.FindElement(By.ClassName("sb-searchbox__button"));

            //Perform Ops
            //element.SendKeys("executeautomation");
            element.SendKeys("Porto");
            element1.Click();

            //Close the browser
            //driver.Close();
        }
    }
}
