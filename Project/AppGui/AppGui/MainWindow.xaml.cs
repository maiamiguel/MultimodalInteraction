using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;
        public MainWindow()
        {

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

            //mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            //mmiC.Message += MmiC_Message;
            //mmiC.Start();

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            Shape _s = null;
            switch ((string)json.recognized[0].ToString())
            {
                case "SQUARE": _s = rectangle;
                    break;
                case "CIRCLE": _s = circle;
                    break;
                case "TRIANGLE": _s = triangle;
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                switch ((string)json.recognized[1].ToString())
                {
                    case "GREEN":
                        _s.Fill = Brushes.Green;
                        break;
                    case "BLUE":
                        _s.Fill = Brushes.Blue;
                        break;
                    case "RED":
                        _s.Fill = Brushes.Red;
                        break;
                    case "ORANGE":
                        _s.Fill = Brushes.Orange;
                        break;
                }
            });
        }
    }
}
