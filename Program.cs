using LLama.Common;
using LLama;

// Indicate where the GGUF model file in your local system
//string modelPath = @"C:\path\to\llama-2-7b-chat.Q5_K_M.gguf";
//string modelPath = @"C:\LLaMA_AI\llama-2-7b-chat.Q2_K.gguf";
string modelPath = @"C:\LLaMA_AI\llama-2-7b-chat.Q4_K_M.gguf";

// Load the model into memory
Console.ForegroundColor = ConsoleColor.DarkGray;
ModelParams modelParams = new(modelPath);
using LLamaWeights weights = LLamaWeights.LoadFromFile(modelParams);

// Setup a chat session
using LLamaContext context = weights.CreateContext(modelParams);
InteractiveExecutor ex = new(context);
ChatSession session = new(ex);
var hideWords = new LLamaTransforms.KeywordTextOutputStreamTransform(["User:", "Bot: "]);
session.WithOutputTransform(hideWords);
InferenceParams infParams = new()
{
    Temperature = 0.6f, // higher values give more "creative" answers
    AntiPrompts = ["User:"]
};

while (true)
{
    // Get a question from the user
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("\nQuestion: ");
    string userInput = Console.ReadLine() ?? string.Empty;
    ChatHistory.Message msg = new(AuthorRole.User, "Question: " + userInput);

    // Display answer text as it is being generated
    Console.ForegroundColor = ConsoleColor.Yellow;
    await foreach (string text in session.ChatAsync(msg, infParams))
    {
        Console.Write(text);
    }
}