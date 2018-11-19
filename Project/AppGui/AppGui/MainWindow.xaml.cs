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

namespace AppGui{
    public partial class MainWindow : Window{
        private MmiCommunication mmiC;
        public MainWindow(){
            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();
        }

        private void MmiC_Message(object sender, MmiEventArgs e){
            //Console.WriteLine("Mensagemmmm       " + e);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            Shape shape = null;

            // First command
            //Console.WriteLine("Confiança    " + json.recognized[0].Result.confidence);
            String _s = (string)json.recognized[0].ToString();
            Console.WriteLine("JSON recognized" + json);

            switch (_s){
                case "M1":
                    ProcessingComandsFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "M2":
                    ProcessingComandsFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "M3":
                    ProcessingComandsFunctions.AcceptCommand((string)json.recognized[0].ToString(), null);
                    break;

                case "SEARCH":
                    Console.WriteLine("PESQUISAR  -> " + (string)json.recognized[2].ToString());
                    ProcessingComandsFunctions.AcceptCommand((string)json.recognized[1].ToString() , (string)json.recognized[2].ToString());
                    break;

                case "HELP":
                    Console.WriteLine("Activar sistema");
                    //Tentativa de meter o círculo a verde quando o sistema está a fornecer ajuda mas não funciona.
                    shape = circle;
                    shape.Fill = Brushes.Green;
                    break;

                case "CLOSE":
                    //Close the browser
                    ProcessingComandsFunctions.AcceptCommand( (string)json.recognized[0].ToString() , null);
                    break;
            }
        }
    }
}
