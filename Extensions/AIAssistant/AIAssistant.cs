using Nazar.Core.WorldGenerator;
using Nazar.Extension.OpenAI;
using Nazar.Extension.SpeechToText;
using Nazar.Framework;
using StereoKit;
using StereoKit.Framework;
using System.Collections.Generic;

namespace Nazar.Extension.AIWorldGenerator;

public class AiAssistant : IStepper
{
    public bool Enabled => true;

    OpenAIService openAiService;
    SpeechToTextService speechToTextService;
    GeneratedWorld generatedWorld;

    public bool Initialize()
    {
        //  Instantiate the components of AI Assistant
        openAiService = SK.AddStepper<OpenAIService>();
        speechToTextService = SK.AddStepper<SpeechToTextService>();
        generatedWorld = SK.AddStepper<GeneratedWorld>();

        // Connect the features between them
        speechToTextService.TextWasSubmitted += async (s, entry) =>
        {
            var openAIResponse = await openAiService.GenerateAIResponce(entry);
            generatedWorld.HandleInput(openAIResponse);
        };

        return true;
    }

    public void Step() { }

    public void Shutdown() { 
        openAiService.Shutdown();
        speechToTextService.Shutdown();
        generatedWorld.Shutdown();
    }
}