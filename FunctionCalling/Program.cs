using FunctionCalling.Plugins;
using FunctionCalling.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using System.ClientModel;
using System.Reflection;
using System.Text;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                       .AddUserSecrets(Assembly.GetExecutingAssembly())
                                       .Build();

var client = new OpenAIClient(new ApiKeyCredential(config["PAT"]),
                              new OpenAIClientOptions { Endpoint = new Uri(config["Uri"]) });

var kernelBuilder = Kernel.CreateBuilder()
                   .AddOpenAIChatCompletion(config["Model"], client);

kernelBuilder.Services.AddLogging();
kernelBuilder.Services.AddSingleton<IWeatherService, WeatherService>();
kernelBuilder.Plugins.AddFromType<WeatherPlugins>();
var kernel = kernelBuilder.Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply as Optimus Prime. Use emojis if possible.");

var settings = new PromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
};

while (true)
{
    Console.Write("Q: ");
    var userQ = Console.ReadLine();
    if (string.IsNullOrEmpty(userQ))
    {
        break;
    }
    history.AddUserMessage(userQ);

    var sb = new StringBuilder();
    Console.Write("AI: ");
    await foreach (var item in chat.GetStreamingChatMessageContentsAsync(history, settings, kernel))
    {
        sb.Append(item);
        Console.Write(item.Content);
    }
    Console.WriteLine();

    history.AddAssistantMessage(sb.ToString());
}