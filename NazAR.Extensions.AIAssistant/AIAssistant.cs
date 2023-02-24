using Nazar.Core.WorldGenerator;
using Nazar.Extension.OpenAI;
using Nazar.Extension.SpeechToText;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension.AIWorldGenerator;

public class AiAssistant : IStepper
{
    public bool Enabled => true;

    OpenAIService openAiService;
    SpeechToTextService speechToTextService;
    WorldGenerator generateWorldService;



    public bool Initialize()
    {
        //  Instantiate the components of AI Assistant
        openAiService = SK.AddStepper<OpenAIService>();
        speechToTextService = SK.AddStepper<SpeechToTextService>();
        generateWorldService = SK.AddStepper<WorldGenerator>();

        // Connect the features between them
        speechToTextService.TextWasSubmitted += async (s, entry) =>
        {
            var openAIResponse = await openAiService.GenerateAIResponce(entry);
            generateWorldService.HandleInput(openAIResponse);
        };

        return true;
    }

    public void Step()
    {
        
    }





    public void Shutdown() { 
    
        openAiService.Shutdown();
        speechToTextService.Shutdown();
        generateWorldService.Shutdown();

    }

}