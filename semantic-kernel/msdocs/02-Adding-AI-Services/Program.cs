// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;

var LlmService = Env.Var("Global:LlmService")!;

if (LlmService == "AzureOpenAI")
{
    var AzureOpenAIDeploymentType = Env.Var("AzureOpenAI:DeploymentType")!;

    if (AzureOpenAIDeploymentType == "chat-completion")
    {
        // Create a kernel with an Azure OpenAI chat completion service
        //////////////////////////////////////////////////////////////////////////////////
        var AzureOpenAIDeploymentName = Env.Var("AzureOpenAI:ChatCompletionDeploymentName")!;
        var AzureOpenAIEndpoint = Env.Var("AzureOpenAI:Endpoint")!;
        var AzureOpenAIApiKey = Env.Var("AzureOpenAI:ApiKey")!;
        var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins", "WriterPlugin");

        var builder = Kernel.CreateBuilder();
        builder.Services.AddAzureOpenAIChatCompletion(
            deploymentName: AzureOpenAIDeploymentName,  // The name of your deployment (e.g., "gpt-35-turbo")
            endpoint: AzureOpenAIEndpoint,              // The endpoint of your Azure OpenAI service
            apiKey: AzureOpenAIApiKey                   // The API key of your Azure OpenAI service
        );
        builder.Plugins.AddFromPromptDirectory(pluginDirectory);
        var kernel = builder.Build();

        var result = await kernel.InvokeAsync("WriterPlugin", "ShortPoem", new() {
            { "input", "Hello world" }
        });
        Console.WriteLine(result);
    }
    else
    {
        throw new Exception($"The Azure OpenAI deployment type '{AzureOpenAIDeploymentType}' is not supported.");
    }
}
else
{
    throw new Exception($"The LLM service '{LlmService}' is not supported.");
}