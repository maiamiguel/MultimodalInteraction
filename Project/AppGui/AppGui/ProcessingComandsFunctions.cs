using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGui{
    class ProcessingComandsFunctions{

        public static void OpenProgram(String country){
            //Selenium
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("http://booking.com");        //Navigate to booking page

            IWebElement element = driver.FindElement(By.Name("ss"));    //Find the Search text box UI Element
            IWebElement element1 = driver.FindElement(By.ClassName("sb-searchbox__button"));    //Click the search button

            String countryName = null;

            switch (country){
                case "SPAIN":
                    //Console.WriteLine(_s);
                    //Console.WriteLine(_s1);
                    countryName = "Espanha";
                    //Perform Ops
                    element.SendKeys(countryName);
                    element1.Click();

                    //Close the browser
                    //driver.Close();
                    break;
                case "PORTUGAL":
                    countryName = "portugal";
                    //Perform Ops
                    element.SendKeys(countryName);
                    element1.Click();

                    //Close the browser
                    //driver.Close();
                    break;
                case "ITALY":
                    countryName = "italy";
                    //Perform Ops
                    element.SendKeys(countryName);
                    element1.Click();

                    //Close the browser
                    //driver.Close();
                    break;
            }
        }
    }
}
