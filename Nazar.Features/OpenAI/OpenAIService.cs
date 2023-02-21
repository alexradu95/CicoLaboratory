using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API.Models;
using StereoKit.Framework;
using StereoKit;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Nazar.Features.AI
{
    internal class OpenAIService : IStepper
    {

        private Pose windowPose = new(0.4f, 0.09f, -0.32f, Quat.LookDir(-0.7f, 0.09f, 0.71f));

        private readonly string restartSequence = "\ntext:\n";

        string openAiKey = "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";

        private OpenAIAPI openAIApi;

        public string textInput = "";

        private string outputText = @"Create a json block from prompt.
                                Example:
                                text:Create a blue cube at position one one one
                                json:{""id"": 0, ""position"": {""x"": 0, ""y"": 0, ""z"": -1}, ""scale"": {""x"": 1.0, ""y"": 1.0, ""z"": 1.0}, ""shape"": ""cube"", ""color"": {""r"": 0.0, ""g"": 0.0, ""b"": 1.0}}
                                text:remove or delete the blue cube
                                json:{""id"": 0, ""remove"": true}
                                Real start with id 0:
                                text:";


        public OpenAIService() { }

        public bool Enabled => throw new NotImplementedException();

        public string OutputText { get => outputText; set => outputText = value; }

        public bool Initialize()
        {
            openAIApi = new OpenAI_API.OpenAIAPI(openAiKey);
            return true;
        }

        public void Shutdown()
        {

        }

        public void Step()
        {
            DrawAIChatUI();

        }


        public async Task<string> GenerateAIResponce(string aPrompt)
        {
            CompletionRequest request = new CompletionRequest(
                aPrompt,
                OpenAI_API.Models.Model.CushmanCode,
                temperature: 0.1,
                max_tokens: 256,
                top_p: 1.0,
                frequencyPenalty: 0.0,
                presencePenalty: 0.0,
                stopSequences: new[] { "text:", "json:", "\n" }
            );

            CompletionResult result = await openAIApi.Completions.CreateCompletionAsync(request);

            string responce = result.ToString();
            outputText += responce + restartSequence;
            return responce;
        }

        private void DrawAIChatUI()
        {
            UI.WindowBegin("Open AI chat", ref windowPose, new Vec2(30, 0) * U.cm);

            //Get the 200 last characters of aiText
            int showLength = 1000;
            string showText = outputText.Length > showLength ? "..." +  outputText.Substring(outputText.Length - showLength) : outputText;
            UI.Text(showText);

            if (OutputText == "") //no AI speech == can edit text
            {
                UI.Input("Input", ref textInput);
            }
            else //AI speech can not edit text
            {
                string sum = textInput + OutputText;
                UI.Input("Input", ref sum);
            }

            UI.WindowEnd();
        }




    }
}
