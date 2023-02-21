using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OpenAI_API.Completions;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Features.AI;

public class AiAssistant : IStepper
{

    //AI generated game objects
    private readonly int myIdCounter = 0;
    private readonly List<Object> objects = new();

    OpenAIService openAiService;
    SpeechToTextService speechToTextService = new SpeechToTextService();

    private Pose buttonPose = new(0.04f, -0.32f, -0.34f, Quat.LookDir(-0.03f, 0.64f, 0.76f));
    private Action checkRecordMic;

    //Microphone and text
    private bool record;

    private string speechAIText = "";


    private readonly string startSequence = "\njson:";


    public bool Enabled => throw new NotImplementedException();

    public bool Initialize()
    {

        openAiService = SK.AddStepper<OpenAIService>();

        speechToTextService.speechRecognizer.Recognizing += (s, e) => { speechAIText = e.Result.Text; };

        speechToTextService.speechRecognizer.Recognized += (s, e) =>
        {
            openAiService.textInput += speechAIText;
            speechAIText = "";
        };

        checkRecordMic = async () =>
        {
            if (record)
                await speechToTextService.speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            else
                await speechToTextService.speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        };
        return true;
    }

    public void Step()
    {
        DrawButtonsUI();

        foreach (Object o in objects) o.Draw();
    }

    private void DrawButtonsUI()
    {
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
        if (UI.Button("Clear(F2)") || (Input.Key(Key.F2) & BtnState.JustActive) > 0) openAiService.textInput = "";
        UI.SameLine();
        UI.PushTint(new Color(0.5f, 0.5f, 1));
        bool submit = UI.Button("Submit") || (Input.Key(Key.Return) & BtnState.JustActive) > 0;
        if (openAiService.textInput != "" && submit)
        {
            openAiService.OutputText += openAiService.textInput + startSequence;
            openAiService.GenerateAIResponce(openAiService.OutputText).ContinueWith(response => HandleAIResponce(response.Result, objects, myIdCounter));

            openAiService.textInput = ""; //Clear input
        }

        UI.PopTint();
        UI.WindowEnd();
    }

    private static void HandleAIResponce(string aResponce, List<Object> someObjects, int someIdCounter)
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



    public void Shutdown()
    {
        speechToTextService.speechRecognizer.StopContinuousRecognitionAsync().Wait(); //Need to call, else slow shutdown
    }




}