using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using StereoKit.Framework;

namespace Nazar.Features.AI
{
    internal class SpeechToTextService : IStepper
    {
        //Microphone and text
        private bool isRecording;

        private string intermediateSpeechInput = "";
        public string speechToTextInput;
        private SpeechRecognizer speechRecognizer;

        //Azure speech to text AI
        string speechKey = "9abb06bd923b40fc9b99692bc077c9e9";
        string speechRegion = "westeurope";



        public SpeechToTextService()
        {
            SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";
            AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            //SPEECH RECOGNIZER
            speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);


        }

        public bool Enabled => throw new System.NotImplementedException();

        public bool IsRecording { get => isRecording; 
            
            set {
                isRecording = value;

                if(isRecording)
                {
                    speechRecognizer.StartContinuousRecognitionAsync().Wait();
                } else
                {
                    speechRecognizer.StopContinuousRecognitionAsync().Wait();
                }
            }
        }

        public bool Initialize()
        {
            return true;
        }

        public void Shutdown()
        {
            speechRecognizer.StopContinuousRecognitionAsync().Wait(); //Need to call, else slow shutdown
        }

        public void Step()
        {

        }

        private void ConfigureSpeechRecognizer()
        {
            // When it is still building and recognizing a word
            speechRecognizer.Recognizing += (s, e) => { intermediateSpeechInput = e.Result.Text; };

            // When it recognized a whole word
            speechRecognizer.Recognized += (s, e) =>
            {
                speechToTextInput += intermediateSpeechInput;
                intermediateSpeechInput = "";
            };
        }
    }
}
