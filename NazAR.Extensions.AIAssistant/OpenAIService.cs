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

        private Pose aiChatPose = new(0, 0, 0, Quat.LookDir(-0.03f, 0.64f, 0.76f));

        private readonly string startSequence = "\njson:";
        private readonly string restartSequence = "\ntext:\n";

        string openAiKey = "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";
        private OpenAIAPI openAIApi;


        /*
         The first thing we have to do is create a start prompt for the AI, which it is going to continue on. In the start prompt we set up the rules for the AI.
         Using OpenAI playground is a good place to test our prompts. The Codex Cushman model was used.
         */
        public string OpenAIPrompt = @"Create a json block from prompt.
                                Example:
                                text:Create a blue cube at position one one one
                                json:{""id"": 0, ""position"": {""x"": 0, ""y"": 0, ""z"": -1}, ""scale"": {""x"": 1.0, ""y"": 1.0, ""z"": 1.0}, ""shape"": ""cube"", ""color"": {""r"": 0.0, ""g"": 0.0, ""b"": 1.0}}
                                text:remove or delete the blue cube
                                json:{""id"": 0, ""remove"": true}
                                Real start with id 0:
                                text:";

        public bool Enabled => true;

        public bool Initialize()
        {
            openAIApi = new OpenAIAPI(openAiKey);
            return openAIApi != null;
        }

        public void Shutdown()
        {

        }

        public void Step()
        {
            DrawOpenAIConversationUI();
        }

        private void DrawOpenAIConversationUI()
        {
            UI.WindowBegin("Conversation", ref aiChatPose, new Vec2(70, 0) * U.cm);

            UI.Text(OpenAIPrompt);

            UI.WindowEnd();
        }

        /// <summary>
        /// Generates an AI response based on the input
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public async Task<string> GenerateAIResponce(string entry)
        {
            OpenAIPrompt += entry + startSequence;

            CompletionRequest request = new CompletionRequest(
                OpenAIPrompt,
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
            OpenAIPrompt += responce + restartSequence;
            return responce;
        }
    }
}
