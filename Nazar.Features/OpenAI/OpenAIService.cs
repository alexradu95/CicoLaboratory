using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API.Models;

namespace Nazar.Features.AI
{
    internal class OpenAIService
    {




        string openAiKey = "";

        private OpenAIAPI openAIApi;

        private string aiText = @"Create a json block from prompt.
                                Example:
                                text:Create a blue cube at position one one one
                                json:{""id"": 0, ""position"": {""x"": 0, ""y"": 0, ""z"": -1}, ""scale"": {""x"": 1.0, ""y"": 1.0, ""z"": 1.0}, ""shape"": ""cube"", ""color"": {""r"": 0.0, ""g"": 0.0, ""b"": 1.0}}
                                text:remove or delete the blue cube
                                json:{""id"": 0, ""remove"": true}
                                Real start with id 0:
                                text:";

        public string AiText
        {
            get => aiText;
            set => aiText = value;
        }

        public async Task<CompletionResult> GenerateAIResponce(string aPrompt)
        {
            CompletionRequest request = new CompletionRequest(
                aPrompt,
                Model.CushmanCode,
                temperature: 0.1,
                max_tokens: 256,
                top_p: 1.0,
                frequencyPenalty: 0.0,
                presencePenalty: 0.0,
                stopSequences: new[] { "text:", "json:", "\n" }
            );
            CompletionResult result = await openAIApi.Completions.CreateCompletionAsync(request);
            return result;
        }
    }
}
