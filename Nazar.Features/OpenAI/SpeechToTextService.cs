using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Nazar.Features.AI
{
    internal class SpeechToTextService
    {
        public SpeechRecognizer speechRecognizer;

        public SpeechToTextService()
        {
                //Azure speech to text AI
                string speechKey = "9abb06bd923b40fc9b99692bc077c9e9";
                string speechRegion = "westeurope";

                SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                speechConfig.SpeechRecognitionLanguage = "en-US";
                AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();

                //SPEECH RECOGNIZER
                speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        }


    }
}
