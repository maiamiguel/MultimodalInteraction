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

        private static Boolean isAssistantSpeaking = false;

        private int choice = 0;

        Timer speakingTimer;
        private Boolean assistantSpeaking = false;

        protected virtual void onRecognized(SpeechEventArg msg){
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null){
                handler(this, msg);
            }
        }

        private LifeCycleEvents lce;
        private MmiCommunication mmic;
        private SemanticValue pendingSemantic;

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

            // load speech synthetizer
            tts = new Tts();

            // introduce assistant
            Speak( "Olá, eu sou o teu assistente de viagens. Tenho todo o gosto em ajudar-te a planear as tuas férias de sonho. Podes saber mais sobre mim dizendo: preciso de ajuda." , 12);
        }

        //TTS
        private void Speak(String text, int seconds){
            string str = "<speak version=\"1.0\"";
            str += " xmlns:ssml=\"http://www.w3.org/2001/10/synthesis\"";
            str += " xml:lang=\"pt-PT\">";
            str += text;
            str += "</speak>";

            tts.Speak(str, 0);

            // enable talking flag
            assistantSpeaking = true;
            Console.WriteLine("Assistant speaking.");

            speakingTimer = new Timer(seconds * 1000);
            speakingTimer.Elapsed += OnSpeakingEnded;
            speakingTimer.AutoReset = false;
            speakingTimer.Enabled = true;
        }

        private void OnSpeakingEnded(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Assistant stopped speaking.");
            assistantSpeaking = false;
        }

        private void RandomSpeak(String[] choices, int seconds)
        {
            Speak(choices[choice++ % choices.Length], seconds);
        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false });
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e){

            // ignore while the assistant is speaking
            if (assistantSpeaking){
                return;
            }

            // ignore low confidance levels
            if (e.Result.Confidence < 0.4)
            {
                return;
            }

            // if confidence is between 40% and 60%
            if (e.Result.Confidence <= 0.6)
            {
                Speak("Desculpa, não consegui entender. Repete por favor..", 4);
                return;
            }

            // TO DOOOOOOOOOOOOOOOO
            // if confidence is between 60% and 80%, confirm explicitly
            /*if (e.Result.Confidence <= 0.80)
            {
                Speak("Confirmar resposta..." , 2);
            }*/

            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true });

            // SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}
            string json = "{ \"recognized\": [";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                json += "\"" + resultSemantic.Value.Value + "\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";

            if (json.Contains("HELP")){
                RandomSpeak(new string[]{
                    "Experimenta dizer: Pesquisar voos em Espanha",
                    "Experimenta dizer: Pesquisar alojamento em Itália",
                    "Experimenta dizer: Pesquisar carros em França",
                    "Experimenta dizer: Pesquisar voos para Suíça",
                }, 4);
                return;
            }

            if (json.Contains("CLOSE")){
                pendingSemantic = e.Result.Semantics;
                Speak("Tens a certeza que pretendes sair da aplicação ?", 4);
                return;
            }

            // hold sematics from a previous command that is being confirmed or from the current command
            SemanticValue semanticValue = null;

            if (pendingSemantic != null)
            {
                //voice feedback for confirmation
                switch (e.Result.Semantics["action"].Value.ToString())
                {
                    case "YES":
                        RandomSpeak(new string[]{
                        "Adeus, até uma próxima! ",
                    } , 4);
                        semanticValue = pendingSemantic;
                        pendingSemantic = null;

                        json = "{ \"recognized\": [";
                        foreach (var resultSemantic in semanticValue)
                        {
                            json += "\"" + resultSemantic.Value.Value + "\", ";
                        }
                        json = json.Substring(0, json.Length - 2);
                        json += "] }";
                        break;
                    case "NO":
                        pendingSemantic = null;
                        RandomSpeak(new string[]{
                        "Percebi mal, peço desculpa.."} , 4);
                        return;
                }                    
            }                

            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime + "", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration) + "", e.Result.Confidence, json);
            mmic.Send(exNot);

        }
    }
}
