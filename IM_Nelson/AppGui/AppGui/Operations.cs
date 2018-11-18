using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGui
{
    internal class Operations
    {
        private static Boolean flag = false;
        private static IWebDriver driver;

        public static void Search(dynamic json) // Command 0: SEARCH
        {
            if (flag == false)
            {
                driver = new ChromeDriver();
                flag = true;
                //Console.WriteLine("flag: " + flag);
            }

            driver.Navigate().GoToUrl("https://booking.kayak.com");
            IWebElement destination = driver.FindElement(By.CssSelector("[id$=destination]"));
            IWebElement initDate = driver.FindElement(By.CssSelector("[id$=depart-input]"));
            IWebElement finDate = driver.FindElement(By.CssSelector("[id$=return-input]"));
            //IWebElement searchButton = driver.FindElement(By.ClassName("[id$=submit]"));

            String command1 = json.recognized[1].ToString();
            //Console.WriteLine("command 1: " + command1);

            switch (command1)
            {
                case "LISBOA":
                    //driver.Navigate().GoToUrl("https://booking.kayak.com");
                    destination.SendKeys("LLisbon (LIS)");
                    initDate.SendKeys("11/21/2018");
                    finDate.SendKeys("11/28/2018");
                    //searchButton.Click();
                    break;
                case "FARO":
                    //driver.Navigate().GoToUrl("https://booking.kayak.com");
                    //destination.SendKeys("Porto (OPO)");
                    //initDate.SendKeys("qua 21/11");
                    //finDate.SendKeys("qua 28/11");
                    //searchButton.Click();
                    break;
                case "MADRID":
                    //driver.Navigate().GoToUrl("https://booking.kayak.com");
                    //destination.SendKeys("Madrid (MAD)");
                    //initDate.SendKeys("qua 21/11");
                    //finDate.SendKeys("qua 28/11");
                    //searchButton.Click();
                    break;
                case "ROMA":
                    //driver.Navigate().GoToUrl("https://booking.kayak.com");
                    //destination.SendKeys("Roma (FCO)");
                    //initDate.SendKeys("qua 21/11");
                    //finDate.SendKeys("qua 28/11");
                    //searchButton.Click();
                    break;
            }
        }
    }
}
