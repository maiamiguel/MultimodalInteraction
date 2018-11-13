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
            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();
        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            Shape shape = null;
            String _s = (string)json.recognized[0].ToString();
            String _s1 = (string)json.recognized[1].ToString();
            Console.WriteLine("_S  " + _s);
            Console.WriteLine("_S1  " + _s1);

            IWebDriver driver = new ChromeDriver();

            //Navigate to google page
            driver.Navigate().GoToUrl("http://booking.com");

            //Find the Search text box UI Element
            IWebElement element = driver.FindElement(By.Name("ss"));
            IWebElement element1 = driver.FindElement(By.ClassName("sb-searchbox__button"));

            Console.WriteLine(_s);
            switch (_s)
            {
                case "SEARCH":
                    Console.WriteLine("pesquisaaaaaaaaaa");
                    break;
                case "ACTIVATE":
                    Console.WriteLine("Activar sistema");
                    shape.Fill = Brushes.Green;
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                switch (_s1)
                {
                    case "SPAIN":
                        //Console.WriteLine(_s);
                        //Console.WriteLine(_s1);
                        _s = "Espanha";
                        //Perform Ops
                        element.SendKeys(_s);
                        element1.Click();

                        //Close the browser
                        //driver.Close();
                        break;
                    case "PORTUGAL":
                        _s = "portugal";
                        //Perform Ops
                        element.SendKeys(_s);
                        element1.Click();

                        //Close the browser
                        //driver.Close();
                        break;
                    case "ITALY":
                        _s = "italy";
                        //Perform Ops
                        element.SendKeys(_s);
                        element1.Click();

                        //Close the browser
                        //driver.Close();
                        break;
                }
            });
        }
    }
}
