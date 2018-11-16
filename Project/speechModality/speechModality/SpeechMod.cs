using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmisharp;
using Microsoft.Speech.Recognition;
using System.Timers;

namespace speechModality{
    public class SpeechMod{

        private Tts tts;
        private SpeechRecognitionEngine sre;
        private Grammar gr;
        public event EventHandler<SpeechEventArg> Recognized;

        Timer activeTimer;
        private Boolean assistantActive = false;

        Timer speakingTimer;
        private Boolean ttS_Speaking = false;

        private int choice = 0;

        protected virtual void onRecognized(SpeechEventArg msg){
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null){
                handler(this, msg);
            }
        }

        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        public SpeechMod(){
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

            // load speech sintesizer
            tts = new Tts();

            // introduce assistant
            Speak("Olá, eu sou o teu assistente de viagens." +
                " Tenho todo o gosto em ajudar-te a planear as tuas férias de sonho." +
                " Podes saber mais sobre mim dizendo: preciso de ajuda.");
        }

        //TTS
        private void Speak(String text){
            ttS_Speaking = true;
            string str = "<speak version=\"1.0\"";
            str += " xmlns:ssml=\"http://www.w3.org/2001/10/synthesis\"";
            str += " xml:lang=\"pt-PT\">";
            str += text;
            str += "</speak>";

            tts.Speak(str, 0);

            // enable talking flag
            
            Console.WriteLine("Assistant speaking.");

            ttS_Speaking = false;
        }

        private void OnSpeakingEnded(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Assistant stopped speaking.");
            ttS_Speaking = false;
        }

        private void RandomSpeak(String[] choices)
        {
            Speak(choices[choice++ % choices.Length]);
        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e){
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false });
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e){
            // ignore while the assistant is speaking
            if (ttS_Speaking){
                return;
            }

            // ignore low confidance levels
            if (e.Result.Confidence < 0.5){
                return;
            }

            onRecognized(new SpeechEventArg(){Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true});

            // if confidence is between 40% and 60%
            if (e.Result.Confidence <= 0.4){
                Speak("Desculpa, não consegui entender.");
                return;
            }
            
            //Console.WriteLine("DEBUG   "  + e.Result.Semantics["help"].Value.ToString());
            // help
            /*if (e.Result.Semantics["action"].Value.ToString() == "HELP")
            {
                Console.WriteLine("FUCK YEAH!!!!!");
                RandomSpeak(new string[]{
                    "Experimenta dizer: Pesquisar voos em Espanha",
                    "Experimenta dizer: Pesquisar alojamento em Itália",
                    "Experimenta dizer: Pesquisar carros em França",
                    "Experimenta dizer: Pesquisar voos para Suíça",
                });
                return;
            }
            */

            // TO DOOOOOOOOOOOOOOOO
            // if confidence is between 60% and 80%, confirm explicitly
            if (e.Result.Confidence <= 0.80){
                Console.WriteLine("Confirmar resposta...");
            }

            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}
            string json = "{ \"recognized\": [";
            foreach (var resultSemantic in e.Result.Semantics){
                json+= "\"" + resultSemantic.Value.Value +"\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";

            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime+"", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration)+"",e.Result.Confidence, json);
            mmic.Send(exNot);
        }
    }
}
