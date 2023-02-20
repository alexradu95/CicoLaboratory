using StereoKit;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI_API.Completions;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using StereoKit.Framework;
using OpenAI_API;

namespace VRWorld
{
    public class AiAssistant : IStepper
    {
        public bool Enabled => throw new NotImplementedException();

        SpeechRecognizer speechRecognizer;
        Action checkRecordMic;

        Pose windowPose = new Pose(0.4f, 0.09f, -0.32f, Quat.LookDir(-0.7f, 0.09f, 0.71f));
        Pose buttonPose = new Pose(0.04f, -0.32f, -0.34f, Quat.LookDir(-0.03f, 0.64f, 0.76f));

        //Microphone and text
        bool record = true;
        string textInput = "";
        string speechAIText = "";

        string aiText = "Create a json block from prompt.\nExample:\ntext:Create a blue cube at position zero zero zero\njson:{\"id\": 0, \"position\": {\"x\": 0, \"y\": 0, \"z\": 0}, \"scale\": {\"x\": 1.0, \"y\": 1.0, \"z\": 1.0}, \"shape\": \"cube\", \"color\": {\"r\": 0.0, \"g\": 0.0, \"b\": 1.0}}\ntext:remove or delete the blue cube\njson:{\"id\": 0, \"remove\": true}\nReal start with id 0:\ntext:";
        string startSequence = "\njson:";
        string restartSequence = "\ntext:\n";

        Task<CompletionResult> generateTask = null;

        //GameObjects are stored in a list
        int myIdCounter = 0;
        List<VRWorld.Object> objects = new List<VRWorld.Object>();

        OpenAIAPI openAIApi;

        public bool Initialize()
        {
            string openAiKey = System.Configuration.ConfigurationManager.AppSettings["OPENAI_API_KEY"];

            speechRecognizer = BuildSpeechRecognizer();
            checkRecordMic();


            //Open AI
            openAIApi = new OpenAIAPI(openAiKey);



            return true;




        }

        private SpeechRecognizer BuildSpeechRecognizer()
        {
            //Azure speech to text AI
            string speechKey = System.Configuration.ConfigurationManager.AppSettings["SPEECH_KEY"];

            string speechRegion = System.Configuration.ConfigurationManager.AppSettings["SPEECH_REGION"];

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

            //SPEECH RECOGNIZER
            var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            speechRecognizer.Recognizing += (s, e) =>
            {
                speechAIText = e.Result.Text;
            };

            speechRecognizer.Recognized += (s, e) =>
            {
                textInput += speechAIText;
                speechAIText = "";
            };

            checkRecordMic = () =>
            {
                if (record)
                {
                    speechRecognizer.StartContinuousRecognitionAsync().Wait();
                }
                else
                {
                    speechRecognizer.StopContinuousRecognitionAsync().Wait();
                }
            };

            return speechRecognizer;
        }

        public void Step()
        {
            UI.WindowBegin("Open AI chat", ref windowPose, new Vec2(30, 0) * U.cm);

            //Get the 200 last characters of aiText
            int showLength = 1000;
            string showText = aiText.Length > showLength ? "..." + aiText.Substring(aiText.Length - showLength) : aiText;
            UI.Text(showText);

            if (speechAIText == "") //no AI speech == can edit text
            {
                UI.Input("Input", ref textInput);
            }
            else //AI speech can not edit text
            {
                string sum = textInput + speechAIText;
                UI.Input("Input", ref sum);
            }
            UI.WindowEnd();

            UI.WindowBegin("Buttons", ref buttonPose, new Vec2(30, 0) * U.cm);
            UI.PushTint(record ? new Color(1, 0.1f, 0.1f) : Color.White); //red when recording
            if (UI.Toggle("Mic(F1)", ref record))
            {
                checkRecordMic();
            }
            if ((Input.Key(Key.F1) & BtnState.JustActive) > 0) //keyboard 'M'
            {
                record = !record; //switch value
                checkRecordMic();
            }

            UI.PopTint();

            UI.SameLine();
            if (UI.Button("Clear(F2)") || (Input.Key(Key.F2) & BtnState.JustActive) > 0)
            {
                textInput = "";
            }
            UI.SameLine();
            UI.PushTint(new Color(0.5f, 0.5f, 1));
            bool submit = UI.Button("Submit") || (Input.Key(Key.Return) & BtnState.JustActive) > 0;
            if (textInput != "" && submit)
            {
                aiText += textInput + startSequence;
                generateTask = GenerateAIResponce(openAIApi, aiText);

                textInput = ""; //Clear input

            }
            UI.PopTint();
            UI.WindowEnd();

            if (generateTask != null && generateTask.IsCompleted)
            {
                string responce = generateTask.Result.ToString();
                HandleAIResponce(responce, objects, myIdCounter);
                aiText += responce + restartSequence;
                generateTask = null;
            }

            foreach (VRWorld.Object o in objects)
            {
                o.Draw();
            }
        }


        public void Shutdown()
        {
            speechRecognizer.StopContinuousRecognitionAsync().Wait(); //Need to call, else slow shutdown
        }

        static async Task<CompletionResult> GenerateAIResponce(OpenAI_API.OpenAIAPI anApi, string aPrompt)
        {
            var request = new CompletionRequest(
                    prompt: aPrompt,
                    model: OpenAI_API.Models.Model.CushmanCode,
                    temperature: 0.1,
                    max_tokens: 256,
                    top_p: 1.0,
                    frequencyPenalty: 0.0,
                    presencePenalty: 0.0,
                    stopSequences: new string[] { "text:", "json:", "\n" }
                    );
            var result = await anApi.Completions.CreateCompletionAsync(request);
            return result;
        }

        static void HandleAIResponce(string aResponce, List<VRWorld.Object> someObjects, int someIdCounter)
        {
            JObject JResponce = JObject.Parse(aResponce);
            int id = (int)JResponce.GetValue("id");

            //Remove
            JResponce.TryGetValue("remove", out JToken JRemove);
            JResponce.TryGetValue("delete", out JToken JDelete);
            bool remove = JRemove != null && (bool)JRemove;
            bool delete = JDelete != null && (bool)JDelete;
            if (remove || delete)
            {
                for (int i = 0; i < someObjects.Count; i++)
                {
                    if (someObjects[i].myId == id)
                    {
                        int lastIndex = someObjects.Count - 1;
                        someObjects[i] = someObjects[lastIndex];
                        someObjects.RemoveAt(lastIndex);
                        i--; //new object at current postion
                        break;
                    }
                }
            }
            else //Update or add new object
            {
                bool foundObject = false;
                for (int i = 0; i < someObjects.Count; i++)
                {
                    if (someObjects[i].myId == id)
                    {
                        someObjects[i].UpdateFromJSON(JResponce);
                        foundObject = true;
                        break;
                    }
                }

                if (!foundObject) //Create a new object
                {
                    someObjects.Add(new VRWorld.Object(id, JResponce));
                }
            }
        }



 


    }
}
