using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using StereoKit;
using StereoKit.Framework;
using System;

namespace Nazar.Features.AI
{
    internal class SpeechToTextService : IStepper
    {

        public bool Enabled => true;
        public event EventHandler<string> TextWasSubmitted;

        private SpeechRecognizer speechRecognizer;

        private string intermediateSpeechInput = string.Empty;
        private string speechToTextInput = string.Empty;

        //Azure speech to text AI
        private string speechKey = "9abb06bd923b40fc9b99692bc077c9e9";
        private string speechRegion = "westeurope";

        private Pose buttonsPose = new(0.04f, -0.32f, -0.34f, Quat.LookDir(-0.03f, 0.64f, 0.76f));

        //Microphone and text
        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;

            set
            {
                isRecording = value;

                if (isRecording)
                {
                    speechRecognizer.StartContinuousRecognitionAsync().Wait();
                }
                else
                {
                    speechRecognizer.StopContinuousRecognitionAsync().Wait();
                }
            }
        }

        public SpeechToTextService()
        {
            SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";
            AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            // When it is still building and recognizing a word
            speechRecognizer.Recognizing += (s, e) => { intermediateSpeechInput = e.Result.Text; };

            // When it recognized a whole word
            speechRecognizer.Recognized += (s, e) =>
            {
                speechToTextInput += intermediateSpeechInput;
                intermediateSpeechInput = "";
            };
        }

        public bool Initialize() => true;

        public void Step() {
            DrawSpeechRecorderUI();
        }

        public void Shutdown()
        {
            speechRecognizer.StopContinuousRecognitionAsync().Wait(); // Need to call, else slow shutdown
        }

        private void DrawSpeechRecorderUI()
        {

            UI.WindowBegin("Input/Output", ref buttonsPose, new Vec2(70, 0) * U.cm);
            UI.Text($"Current input: {speechToTextInput}");
            UI.PushTint(IsRecording ? new Color(1, 0.1f, 0.1f) : Color.White); // Red when recording
            if (UI.Button("Mic(F1)")) IsRecording = !IsRecording;
            UI.PopTint();

            UI.SameLine();
            if (UI.Button("Clear(F2)") || (Input.Key(Key.F2) & BtnState.JustActive) > 0) ClearSpeechInput();
            UI.SameLine();
            UI.PushTint(new Color(0.5f, 0.5f, 1));
            bool submit = UI.Button("Submit") || (Input.Key(Key.Return) & BtnState.JustActive) > 0;
            if (speechToTextInput != "" && submit)
            {
                TextWasSubmitted.Invoke(this, speechToTextInput);
                ClearSpeechInput();
            }

            UI.PopTint();
            UI.WindowEnd();
        }

        public void ClearSpeechInput()
        {
            speechToTextInput = string.Empty;
        }



    }
}
