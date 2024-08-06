using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

// Create a kernel with OpenAI chat completion
#pragma warning disable SKEXP0010
Kernel kernel = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: "phi3",
                        endpoint: new Uri("http://localhost:11434"),
                        apiKey: "")
                    .Build();

var aiChatService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory();

while (true)
{
    // Get user prompt and add to chat history
    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessageContent(AuthorRole.User, userPrompt));

    // Stream the AI response and add to chat history
    Console.WriteLine("AI Response:");
    var response = "";
    await foreach (var item in
        aiChatService.GetStreamingChatMessageContentsAsync(chatHistory))
    {
        Console.Write(item.Content);
        response += item.Content;
    }
    chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, response));
    Console.WriteLine();
}

// Based on https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/quickstart-local-ai