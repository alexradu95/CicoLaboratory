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
    SpeechToTextService speechToTextService;

    private Pose windowPose = new(0.04f, -0.32f, -0.34f, Quat.LookDir(-0.03f, 0.64f, 0.76f));

    public bool Enabled => true;

    public bool Initialize()
    {

        openAiService = SK.AddStepper<OpenAIService>();
        speechToTextService = SK.AddStepper<SpeechToTextService>();

        return true;
    }



    public void Step()
    {
        DrawAssistantUI();

        foreach (Object o in objects) o.Draw();
    }


    private void DrawAssistantUI()
    {
        UI.WindowBegin("Buttons", ref windowPose, new Vec2(30, 0) * U.cm);

        UI.Text(openAiService.OpenAIPromptSummary);

        UI.PushTint(speechToTextService.IsRecording ? new Color(1, 0.1f, 0.1f) : Color.White); //red when recording
        if (UI.Button("Mic(F1)")) speechToTextService.IsRecording = !speechToTextService.IsRecording;

        UI.PopTint();

        UI.SameLine();
        if (UI.Button("Clear(F2)") || (Input.Key(Key.F2) & BtnState.JustActive) > 0) speechToTextService.speechToTextInput = "";
        UI.SameLine();
        UI.PushTint(new Color(0.5f, 0.5f, 1));
        bool submit = UI.Button("Submit") || (Input.Key(Key.Return) & BtnState.JustActive) > 0;
        if (speechToTextService.speechToTextInput != "" && submit)
        {
            openAiService.AddNewEntryToPrompt(speechToTextService.speechToTextInput);
            openAiService.GenerateAIResponce(openAiService.OpenAIPromptFull).ContinueWith(response => HandleAIResponce(response.Result, objects, myIdCounter));

            speechToTextService.speechToTextInput = ""; //Clear input
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

    }




}