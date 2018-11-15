using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmisharp;
using Microsoft.Speech.Recognition;

namespace speechModality
{
    public class SpeechMod
    {
        private SpeechRecognitionEngine sre;
        private Grammar gr;
        public event EventHandler<SpeechEventArg> Recognized;

        private Tts tts;

        private Boolean active = false; //Indicates whether the Assistant is active or not

        protected virtual void onRecognized(SpeechEventArg msg)
        {
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null)
            {
                handler(this, msg);
            }
        }

        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        public SpeechMod()
        {
            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "FUSION","speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());

            //load pt recognizer
            sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-PT"));
            gr = new Grammar(Environment.CurrentDirectory + "\\ptG.grxml", "rootRule");
            sre.LoadGrammar(gr);

            sre.SetInputToDefaultAudioDevice();
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += Sre_SpeechRecognized;
            sre.SpeechHypothesized += Sre_SpeechHypothesized;

            //load speech synthesizer (Text to Speech)
            tts = new Tts();

            //assistant welcome message
            Assistant("Olá, eu sou o teu assistente pessoal. Em que posso ajudar?"); // To be completed...
        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false, Active = active });
        }

        private void Assistant(String message)
        {
            string str = "<speak version=\"1.0\"";
            str += " xmlns:ssml=\"http://www.w3.org/2001/10/synthesis\"";
            str += " xml:lang=\"pt-PT\">";
            str += message;
            str += "</speak>";

            tts.Speak(str, 0);
        }

        private void Help(String text)
        {
            Assistant(text);
        }

        private void Activate()
        {
            active = true;
            onRecognized(new SpeechEventArg() { Active = active });
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Console.WriteLine("Command: " + e.Result.Semantics["command"].Value.ToString() + "; Confidence: " + e.Result.Confidence);

            // User speeches with poor confidence levels are discarded
            if (e.Result.Confidence < 0.4)
            {
                return;
            }

            // Activation command
            if (e.Result.Semantics["command"].Value.ToString() == "ACTIVATION")
            {
                Activate();
                //pendingSemantic = null;
                return;
            }

            //Do nothing while the Assistant not active
            if (!active)
            {
                //pendingSemantic = null;
                return;
            }

            onRecognized(new SpeechEventArg(){Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true, Active = active });

            if (e.Result.Confidence < 0.6)
            {
                Assistant("Não consegui perceber o que disse. Importa-se de repetir?");
                //pendingSemantic = null;
                return;
            }

            // Help command
            if (e.Result.Semantics["command"].Value.ToString() == "HELP")
            {
                Help("Ok, experimente perguntar por exemplo: Quero viajar para Lisboa.");
                //pendingSemantic = null;
                return;
            }

            // User speeches with confidence levels between 60% and 80%
            if (e.Result.Confidence < 0.8)
            {
                //pendingSemantic = e.Result.Semantics;
                String command = e.Result.Semantics["command"].Value.ToString();

                switch (command)
                {
                    case "SEARCH":
                        Assistant("Quer que procure voos?");
                        return;
                }

                // by default, do not confirm actions that would be a mess if envolved voice feedback, like asking to go to the next slide of a powerpoint
                //pendingSemantic = null;
            }

            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}
            string json = "{ \"recognized\": [";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                json+= "\"" + resultSemantic.Value.Value +"\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";

            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime+"", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration)+"",e.Result.Confidence, json);
            mmic.Send(exNot);
        }
    }
}
