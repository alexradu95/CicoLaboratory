using Nazar.Extensions.OpenAI;
using Nazar.Extensions.SpeechToText;
using Nazar.Framework;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extensions.AIWorldGenerator;

public class AiAssistant : Node
{
    public bool Enabled => true;

    OpenAIService openAiService;
    SpeechToTextService speechToTextService;
    GeneratedWorld generatedWorld;

    public override bool Initialize()
    {
        //  Instantiate the components of AI Assistant
        openAiService = SK.AddStepper<OpenAIService>();
        speechToTextService = SK.AddStepper<SpeechToTextService>();
        generatedWorld = SK.AddStepper<GeneratedWorld>();

        // Connect the features between them
        speechToTextService.TextWasSubmitted += async (s, entry) =>
        {
            string openAIResponse = await openAiService.GenerateAIResponce(entry);
            generatedWorld.HandleInput(openAIResponse);
        };

        return true;
    }

    public override void Step() { }

    public override void Shutdown() { 
        openAiService.Shutdown();
        speechToTextService.Shutdown();
        generatedWorld.Shutdown();
    }
}