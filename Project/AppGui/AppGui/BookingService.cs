using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class BookingService{
        private String URL = "http://www.booking.com";
        private IWebDriver driver;

        public BookingService()
        {
            //Create the reference for our browser
            IWebDriver driver = new ChromeDriver();

            //Navigate to google page
            driver.Navigate().GoToUrl("http:www.booking.com");

            //Find the Search text box UI Element
            IWebElement element = driver.FindElement(By.Name("q"));

            //Perform Ops
            element.SendKeys("executeautomation");

            //Close the browser
            driver.Close();
        }
    }
}
