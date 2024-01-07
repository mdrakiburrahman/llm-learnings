// Copyright (c) Microsoft. All rights reserved.

// Create a default kernel
//////////////////////////////////////////////////////////////////////////////////
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Plugins;

var LlmService = Env.Var("Global:LlmService")!;

if (LlmService == "AzureOpenAI")
{
    // Create a kernel with a logger and Azure OpenAI chat completion service
    //////////////////////////////////////////////////////////////////////////////////
    var AzureOpenAIDeploymentName = Env.Var("AzureOpenAI:ChatCompletionDeploymentName")!;
    var AzureOpenAIEndpoint = Env.Var("AzureOpenAI:Endpoint")!;
    var AzureOpenAIApiKey = Env.Var("AzureOpenAI:ApiKey")!;

    var builder = Kernel.CreateBuilder();
    ;
    builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
    builder.Services.AddAzureOpenAIChatCompletion(
        AzureOpenAIDeploymentName, // The name of your deployment (e.g., "gpt-35-turbo")
        AzureOpenAIEndpoint, // The endpoint of your Azure OpenAI service
        AzureOpenAIApiKey // The API key of your Azure OpenAI service
    );
    builder.Plugins.AddFromType<TimePlugin>();
    builder.Plugins.AddFromPromptDirectory("./plugins/WriterPlugin");

    var kernel = builder.Build();

    // Get the current time
    var currentTime = await kernel.InvokeAsync("TimePlugin", "GetCurrentUtcTime");
    Console.WriteLine(currentTime);

    // Write a poem with the WriterPlugin.ShortPoem function using the current time as input
    var poemResult = await kernel.InvokeAsync(
        "WriterPlugin",
        "ShortPoem",
        new() { { "input", currentTime } }
    );
    Console.WriteLine(poemResult);
}
else
{
    Console.WriteLine("Unknown LLM service");
}
