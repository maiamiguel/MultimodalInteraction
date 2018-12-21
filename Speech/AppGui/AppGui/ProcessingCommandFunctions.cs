using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace AppGui
{
    class ProcessingCommandFunctions
    {
        private static Boolean firstTime = true;
        private static IWebDriver driver;
        private static Actions action;
        private static Boolean firstFlightSearch = true;
        private static Boolean firstHotelSearch = true;
        private static string travellers;

        public static void AcceptCommand(String type, String city){
            Console.WriteLine("Tipo -> " + type + "     city ->   " + city);

            if (firstTime)
            {
                driver = new ChromeDriver();
                //driver.Manage().Window.Maximize();
                //driver.Manage().Timeouts().ImplicitWait.Add(TimeSpan.FromSeconds(30));
                firstTime = false;
            }

            if (type == "CLOSE")
            {
                Console.WriteLine("FECHAR O BROWSER!");
                driver.Close();
                firstTime = true;
            }

            if (type == "FLIGHT")
            {
                // go to home page
                driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                // click country button
                driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                // select US country
                driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();

                switch (city)
                {
                    case "ROME":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("Rome, IT - Fiumicino (FCO)");
                        break;
                    case "PARIS":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("Paris, FR - Charles De Gaulle (CDG)");
                        break;

                    case "LONDON":
                        // send source (default)
                        driver.FindElement(By.Id("FromTag")).Clear();
                        driver.FindElement(By.Id("FromTag")).SendKeys("Lisbon, PT - Lisboa (LIS)");
                        // send destination
                        driver.FindElement(By.Id("ToTag")).Clear();
                        driver.FindElement(By.Id("ToTag")).SendKeys("London, GB - All airports (LON)");
                        break;
                }

                // click depart box
                driver.FindElement(By.XPath("//*[@id='DepartDate']")).Click();
                // click next month button
                if (firstFlightSearch)
                {
                    driver.FindElement(By.XPath("//*[@id='ui-datepicker-div']/div[2]/div/a")).Click();
                    firstFlightSearch = false;
                }
                // select depart day
                driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[29]")).Click();
                // click search button
                driver.FindElement(By.Id("SearchBtn")).Click();
            }
            else if(type == "HOTEL")
            {
                // go to home page
                driver.Navigate().GoToUrl("https://www.cleartrip.com/");
                // click country button
                driver.FindElement(By.XPath("//*[@id='userAccountNav']/nav/ul/li[2]/a")).Click();
                // select US country
                driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();
                // click hotels link
                driver.FindElement(By.XPath("//*[@id='Home']/div/aside[1]/nav/ul[1]/li[2]/a")).Click();
                // select € currency
                driver.FindElement(By.XPath("//*[@id='countryForm']/li[8]/a")).Click();

                switch (city)
                {
                    case "ROME":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("Rome");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;

                    case "PARIS":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("Paris");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;

                    case "LONDON":
                        // send destination
                        driver.FindElement(By.Name("from")).Click();
                        driver.FindElement(By.Name("from")).SendKeys("London");
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // instantiate action object to press keys
                        action = new Actions(driver);
                        // we need to first press Down and Up keys, otherwise it won't work
                        action.SendKeys(Keys.Down).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        action.SendKeys(Keys.Up).Build().Perform();
                        // sleep 3 seconds
                        Thread.Sleep(3000);
                        // press Enter key
                        action.SendKeys(Keys.Enter).Build().Perform();
                        break;
                }

                // click check-in box
                driver.FindElement(By.XPath("//*[@id='CheckInDate']")).Click();
                // click next month button
                if (firstHotelSearch)
                {
                    driver.FindElement(By.XPath("//*[@id='ui-datepicker-div']/div[2]/div/a")).Click();
                }
                // select check-in day
                driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[29]")).Click();
                // click check-out box
                driver.FindElement(By.XPath("//*[@id='CheckOutDate']")).Click();
                // select check-out day
                if (firstHotelSearch)
                {
                    driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[2]/table/tbody/tr/td/a)[1]")).Click();
                    firstHotelSearch = false;
                }
                else
                {
                    driver.FindElement(By.XPath("(//*[@id='ui-datepicker-div']/div[1]/table/tbody/tr/td/a)[1]")).Click();
                }
                // click search button
                driver.FindElement(By.Id("SearchHotelsButton")).Click();
            }

            if (type == "FILTER")
            {
                if (city == "AC")
                {
                    Console.WriteLine("Ar condicionado");
                    driver.FindElement(By.ClassName("filters__more"));
                    driver.FindElement(By.XPath("//*[@id='1_1amWIFI']/div")).Click();
                }
            }

            if (type == "O1")
            {
                travellers = "O1";
            }

            if (type == "O2")
            {
                travellers = "O2";
            }

            if (type == "O3")
            {
                travellers = "O3";
            }
        }
    }
}
