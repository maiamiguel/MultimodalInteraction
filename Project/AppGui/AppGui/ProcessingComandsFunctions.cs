using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGui{
    class ProcessingComandsFunctions{

        private static Boolean firstTime = true;
        private static IWebDriver driver;
        private static IWebElement element;
        private static IWebElement element1;

        public static void AcceptCommand(String type, String country){
            
            if (firstTime){
                driver = new ChromeDriver();
                firstTime = false;                
            }

            // Selenium

            String countryName = null;

            if (type == "VOO")
            {
                driver.Navigate().GoToUrl("https://booking.kayak.com/");
                IWebElement source = driver.FindElement(By.ClassName("Common-Widgets-Text-TextInput size-l selectTextOnFocus input-flat"));
                source.SendKeys("Porto");
                IWebElement destination = driver.FindElement(By.ClassName("bpk-input-R6x4G fsc-large-above-tablet-3NddW fsc-docked-middle-above-tablet-1iZwT fsc-docked-last-on-tablet-vWch9 fsc-location-input-2Jtp_"));
                IWebElement search = driver.FindElement(By.ClassName("bpk-button-2YQI1 bpk-button--large-1Z1P5 SubmitButton-WxCV2"));
            }
            else if (type == "HOTEL")
            {
                driver.Navigate().GoToUrl("http://booking.com");                        // Navigate to booking page
                element = driver.FindElement(By.Name("ss"));                            //Find the Search text box UI Element
                element1 = driver.FindElement(By.ClassName("sb-searchbox__button"));    // Click the search button
            }
            else if (type == "FOOD")
            {
                driver.Navigate().GoToUrl("https://www.opentable.co.uk/?ref=13850");        // Navigate to booking page
                element = driver.FindElement(By.ClassName("dtp-picker-search tt-input"));   //Find the Search text box UI Element
                element1 = driver.FindElement(By.ClassName("button dtp-picker-button"));    // Click the search button
            }
            else if (type == "CARS")
            {
                driver.Navigate().GoToUrl("http://www.rentalcars.com/");                // Navigate to booking page
                element = driver.FindElement(By.ClassName("ui-autocomplete-input"));    //Find the Search text box UI Element
                element1 = driver.FindElement(By.Id("formsubmit"));                     // Click the search button
            }
            else if (type == "CLOSE")
            {
                Console.WriteLine("FECHAR O BROWSER!");
                driver.Close();
                firstTime = true;
            }

            switch (country){
                case "SPAIN":
                    countryName = "Espanha";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "PORTUGAL":
                    countryName = "Portugal";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "ITALY":
                    countryName = "Itália";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "SWITZERLAND":
                    countryName = "Suíça";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "HOLAND":
                    countryName = "Holanda";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "GERMAN":
                    countryName = "Alemanha";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "FRANCE":
                    countryName = "França";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "AUSTRIA":
                    countryName = "Aústria";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "CROATIA":
                    countryName = "Croácia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "SERVIA":
                    countryName = "Sérvia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "GREECE":
                    countryName = "Grécia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "BELGIUM":
                    countryName = "Bélgica";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "POLAND":
                    countryName = "Polónia";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;

                case "HUNGARY":
                    countryName = "Hungria";
                    element.SendKeys(countryName);
                    element1.Click();
                    break;
            }
        }
    }
}
