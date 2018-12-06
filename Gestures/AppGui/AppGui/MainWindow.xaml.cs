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

            // Print json recognized
            Console.WriteLine("json: " + json);

            // Get first command
            String _s = (string)json.recognized[0].ToString();

            switch (_s)
            {
                case "O1":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "O2":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "O3":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "M1":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "M2":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "M3":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "SEARCH":
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[1].ToString(), (string)json.recognized[2].ToString());
                    break;

                case "CLOSE":
                    //Close the browser
                    ProcessingCommandFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "FLIGHT":
                    break;

                case "HOTEL":
                    break;
            }
        }
    }
}
