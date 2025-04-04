using Microsoft.Extensions.Configuration;
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

var kernel = Kernel.CreateBuilder()
                   .AddOpenAIChatCompletion(config["Model"], client)
                   .Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply as Optimus Prime. Use emojis if possible.");

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
    await foreach (var item in chat.GetStreamingChatMessageContentsAsync(history))
    {
        sb.Append(item);
        Console.Write(item.Content);
    }
    Console.WriteLine();

    history.AddAssistantMessage(sb.ToString());
}