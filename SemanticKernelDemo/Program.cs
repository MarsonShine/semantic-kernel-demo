// See https://aka.ms/new-console-template for more information
using Microsoft.SemanticKernel;

Console.WriteLine("Hello, World!");

var kernel = Kernel.Builder.Build();

// OpenAI API Key
kernel.Config.AddOpenAITextCompletionService(modelId: "text-davinci-003", apiKey: "");

#region Prompts Demo
var prompt = @"{{$input}}

One line TLDR with the fewest words.";

var summarize = kernel.CreateSemanticFunction(prompt);

string text1 = """
1st Law of Thermodynamics - Energy cannot be created or destroyed.
2nd Law of Thermodynamics - For a spontaneous process, the entropy of the universe increases.
3rd Law of Thermodynamics - A perfect crystal at zero Kelvin has zero entropy.
""";

string text2 = """
1. An object at rest remains at rest, and an object in motion remains in motion at constant speed and in a straight line unless acted on by an unbalanced force.
2. The acceleration of an object depends on the mass of the object and the amount of force applied.
3. Whenever one object exerts a force on another object, the second object exerts an equal and opposite on the first.";
""";

Console.WriteLine(await summarize.InvokeAsync(text1));
Console.WriteLine(await summarize.InvokeAsync(text2));
#endregion

#region Prompt chaining
// 将input翻译成数学符号，并生成一个摘要
string translationPrompt = @"{{input}}

Translate the text to math.";

string summarizePrompt = @"{{$input}}

Give me a TLDR with the fewest words.";

var translator = kernel.CreateSemanticFunction(translationPrompt);
var summarize2 = kernel.CreateSemanticFunction(summarizePrompt);

string inputText = """
1st Law of Thermodynamics - Energy cannot be created or destroyed.
2nd Law of Thermodynamics - For a spontaneous process, the entropy of the universe increases.
3rd Law of Thermodynamics - A perfect crystal at zero Kelvin has zero entropy.
""";

var output = await kernel.RunAsync(inputText, translator, summarize2);
Console.WriteLine(output);
#endregion
