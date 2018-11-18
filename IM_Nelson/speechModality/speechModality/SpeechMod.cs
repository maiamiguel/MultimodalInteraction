using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mmisharp;
using Microsoft.Speech.Recognition;
using System.Timers;
using System.Media;

namespace speechModality
{
    public class SpeechMod
    {
        private SpeechRecognitionEngine sre;
        private Grammar gr;
        public event EventHandler<SpeechEventArg> Recognized;

        private Tts tts;

        private int i = 0; // Used to choose a random speak option
        private int searchNumber = 0;

        Timer timerActivation; // Timer for the Assistant activation
        Timer timerSpeaking; // Timer for the Assistant speaking

        private Boolean isAssistantActive = false; //Indicates whether the Assistant is active or not
        private Boolean isAssistantSpeaking = false; //Indicates whether the Assistant is speaking or not

        private SemanticValue pendingConf = null; //Used when the Assistant needs user confirmation

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
            AssistantSpeak("Olá, eu sou o teu assistente pessoal.", 4); // to review
        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false, AssistantActivation = isAssistantActive });
        }

        private void ActivationExpired(Object source, ElapsedEventArgs e)
        {
            // make sound when Assistant activation expires
            SoundPlayer sound = new SoundPlayer(Environment.CurrentDirectory + "\\activeoff.wav");
            sound.Play();

            Console.WriteLine("Assistant activation expired.");
            isAssistantActive = false;

            onRecognized(new SpeechEventArg());
        }

        private void SpeakingStopped(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Assistant speaking stopped.");
            isAssistantSpeaking = false;
        }

        private void AssistantSpeak(String message, int seconds)
        {
            string str = "<speak version=\"1.0\"";
            str += " xmlns:ssml=\"http://www.w3.org/2001/10/synthesis\"";
            str += " xml:lang=\"pt-PT\">";
            str += message;
            str += "</speak>";

            tts.Speak(str, 0);

            Console.WriteLine("Assistant speaking started.");
            isAssistantSpeaking = true;

            timerSpeaking = new Timer(seconds * 1000);
            timerSpeaking.Elapsed += SpeakingStopped;
            timerSpeaking.AutoReset = false;
            timerSpeaking.Enabled = true;
        }

        private void RandomSpeak(String[] options, int seconds)
        {
            AssistantSpeak(options[i++ % options.Length], seconds);
        }

        private void ActivateAssistant()
        {
            if (timerActivation != null)
            {
                timerActivation.Stop();
            }

            if (!isAssistantActive)
            {
                // make sound when Assistant activation initiates
                SoundPlayer sound = new SoundPlayer(Environment.CurrentDirectory + "\\activeon.wav");
                sound.Play();
            }

            Console.WriteLine("Assistant activation initiated.");
            isAssistantActive = true;

            onRecognized(new SpeechEventArg() { AssistantActivation = isAssistantActive });

            // activate assistant (for 10 seconds)
            timerActivation = new Timer(10 * 1000);
            timerActivation.Elapsed += ActivationExpired;
            timerActivation.AutoReset = false;
            timerActivation.Enabled = true;
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Command: " + e.Result.Semantics["command"].Value.ToString() + "; Confidence: " + e.Result.Confidence);

            // ignore while the assistant is speaking
            if (isAssistantSpeaking)
            {
                return;
            }

            // User speeches with poor confidence levels are discarded
            if (e.Result.Confidence <= 0.4)
            {
                return;
            }

            // Activation command
            if (e.Result.Semantics["command"].Value.ToString() == "ACTIVATION")
            {
                ActivateAssistant();
                pendingConf = null;
                return;
            }

            //Do nothing while the Assistant not active
            if (!isAssistantActive)
            {
                pendingConf = null;
                return;
            }

            // Maintain the Assistant active
            ActivateAssistant();

            onRecognized(new SpeechEventArg(){Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true, AssistantActivation = isAssistantActive });

            // User speeches with confidence levels between 40% and 60%
            if (e.Result.Confidence <= 0.6)
            {
                AssistantSpeak("Não consegui perceber o que disse. Importa-se de repetir?", 4);
                pendingConf = null;
                return;
            }

            // Help command
            if (e.Result.Semantics["command"].Value.ToString() == "HELP")
            {
                RandomSpeak(new string[] {
                    "Pergunte por exemplo: Quero viajar para Lisboa."
                }, 4);
                pendingConf = null;
                return;
            }

            // User speeches with confidence levels between 60% and 80%
            if (e.Result.Confidence <= 0.8)
            {
                pendingConf = e.Result.Semantics;
                String command = e.Result.Semantics["command"].Value.ToString();

                switch (command)
                {
                    case "SEARCH":
                        AssistantSpeak("Quer que procure voos?", 2);
                        return;
                }

                // by default, do not confirm actions that would be a mess if envolved voice feedback, like asking to go to the next slide of a powerpoint
                pendingConf = null;
            }

            // hold semantics from a previous command that is being confirmed or from the current command
            SemanticValue semanticVal = null;

            if (pendingConf != null)
            {
                // handle confirmation command
                switch (e.Result.Semantics["command"].Value.ToString())
                {
                    case "YES":
                        RandomSpeak(new string[] {
                            "Ok, estou a ver isso.",
                            "Ok, certo.",
                            "Está bem."
                        }, 4);
                        break;
                    case "NO":
                        pendingConf = null;
                        RandomSpeak(new string[] {
                            "Ah ok, eu não percebi bem.",
                            "Desculpa, eu não ouvi bem."
                        }, 4);
                        return;
                }

                semanticVal = pendingConf;
                pendingConf = null;
            }
            else
            {
                // handle other commands
                switch (e.Result.Semantics["command"].Value.ToString())
                {
                    case "SEARCH":
                        if(e.Result.Semantics["command"].Value.ToString() == "SEARCH")
                        {
                            if (e.Result.Semantics.ContainsKey("destination") && e.Result.Semantics["destination"].Value.ToString() == "LISBOA")
                            {
                                AssistantSpeak("Boa escolha! Lisboa é uma cidade lindíssima.", 4);
                                return;
                            }
                            if (searchNumber == 1)
                            {
                                AssistantSpeak("Muito bem, já vi que gosta de viajar. Estou a tratar disso.", 4);
                            }
                            else
                            {
                                AssistantSpeak("Com certeza.", 4);
                            }
                        }
                        return;
                }

                semanticVal = e.Result.Semantics;
            }

            if (semanticVal["command"].Value.ToString() == "SEARCH")
            {
                searchNumber++;
            }

            // if a command was recognized and the confirmation of a previous command was ignored by the user, disable it
            pendingConf = null;

            // stop here if it was a inner command
            if (semanticVal["command"].Value.ToString() == "YES" || semanticVal["command"].Value.ToString() == "NO")
            {
                return;
            }

            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}
            string json = "{ \"recognized\": [";
            foreach (var resultSemantic in semanticVal) //
            {
                json+= "\"" + resultSemantic.Value.Value +"\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";
            Console.WriteLine(json); //

            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime+"", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration)+"",e.Result.Confidence, json);
            mmic.Send(exNot);
        }
    }
}
