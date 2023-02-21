using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json.Linq;
using OpenAI_API;
using OpenAI_API.Completions;
using StereoKit;
using StereoKit.Framework;
using Model = OpenAI_API.Models.Model;

namespace Nazar.Features.AI;

public class AiAssistant : IStepper
{

    OpenAIService openAiService = new OpenAIService();


    private Pose buttonPose = new(0.04f, -0.32f, -0.34f, Quat.LookDir(-0.03f, 0.64f, 0.76f));
    private Action checkRecordMic;

    private Task<CompletionResult> generateTask;

    //GameObjects are stored in a list
    private readonly int myIdCounter = 0;
    private readonly List<Object> objects = new();



    //Microphone and text
    private bool record;
    private readonly string restartSequence = "\ntext:\n";
    private string speechAIText = "";

    private SpeechRecognizer speechRecognizer;
    private readonly string startSequence = "\njson:";
    private string textInput = "";

    private Pose windowPose = new(0.4f, 0.09f, -0.32f, Quat.LookDir(-0.7f, 0.09f, 0.71f));
    public bool Enabled => throw new NotImplementedException();

    public bool Initialize()
    {
        speechRecognizer = BuildSpeechRecognizer();

        return true;
    }

    public void Step()
    {
        UI.WindowBegin("Open AI chat", ref windowPose, new Vec2(30, 0) * U.cm);

        //Get the 200 last characters of aiText
        int showLength = 1000;
        string showText = openAiService.AiText.Length > showLength ? "..." + openAiService.AiText.Substring(openAiService.AiText.Length - showLength) : openAiService.AiText;
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
        if (UI.Toggle("Mic(F1)", ref record)) checkRecordMic();
        if ((Input.Key(Key.F1) & BtnState.JustActive) > 0) //keyboard 'M'
        {
            record = !record; //switch value
            checkRecordMic();
        }

        UI.PopTint();

        UI.SameLine();
        if (UI.Button("Clear(F2)") || (Input.Key(Key.F2) & BtnState.JustActive) > 0) textInput = "";
        UI.SameLine();
        UI.PushTint(new Color(0.5f, 0.5f, 1));
        bool submit = UI.Button("Submit") || (Input.Key(Key.Return) & BtnState.JustActive) > 0;
        if (textInput != "" && submit)
        {
            openAiService.AiText += textInput + startSequence;
            generateTask = openAiService.GenerateAIResponce(openAiService.AiText);

            textInput = ""; //Clear input
        }

        UI.PopTint();
        UI.WindowEnd();

        if (generateTask != null && generateTask.IsCompleted)
        {
            string responce = generateTask.Result.ToString();
            HandleAIResponce(responce, objects, myIdCounter);
            openAiService.AiText += responce + restartSequence;
            generateTask = null;
        }

        foreach (Object o in objects) o.Draw();
    }


    public void Shutdown()
    {
        speechRecognizer.StopContinuousRecognitionAsync().Wait(); //Need to call, else slow shutdown
    }

    private SpeechRecognizer BuildSpeechRecognizer()
    {
        //Azure speech to text AI
        string speechKey = "9abb06bd923b40fc9b99692bc077c9e9";
        string speechRegion = "westeurope";

        SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        speechConfig.SpeechRecognitionLanguage = "en-US";
        AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();

        //SPEECH RECOGNIZER
        SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        speechRecognizer.Recognizing += (s, e) => { speechAIText = e.Result.Text; };

        speechRecognizer.Recognized += (s, e) =>
        {
            textInput += speechAIText;
            speechAIText = "";
        };

        checkRecordMic = async () =>
        {
            if (record)
                await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            else
                await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        };

        return speechRecognizer;
    }

    private static void HandleAIResponce(string aResponce, List<Object> someObjects, int someIdCounter)
    {
        JObject JResponce = JObject.Parse(aResponce);
        int id = (int) JResponce.GetValue("id");

        //Remove
        JResponce.TryGetValue("remove", out JToken JRemove);
        JResponce.TryGetValue("delete", out JToken JDelete);
        bool remove = JRemove != null && (bool) JRemove;
        bool delete = JDelete != null && (bool) JDelete;
        if (remove || delete)
        {
            for (int i = 0; i < someObjects.Count; i++)
                if (someObjects[i].myId == id)
                {
                    int lastIndex = someObjects.Count - 1;
                    someObjects[i] = someObjects[lastIndex];
                    someObjects.RemoveAt(lastIndex);
                    i--; //new object at current postion
                    break;
                }
        }
        else //Update or add new object
        {
            bool foundObject = false;
            for (int i = 0; i < someObjects.Count; i++)
                if (someObjects[i].myId == id)
                {
                    someObjects[i].UpdateFromJSON(JResponce);
                    foundObject = true;
                    break;
                }

            if (!foundObject) //Create a new object
                someObjects.Add(new Object(id, JResponce));
        }
    }
}