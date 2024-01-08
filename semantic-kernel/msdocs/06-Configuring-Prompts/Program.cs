﻿// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

// Create kernel
var builder = Kernel.CreateBuilder();
builder.WithCompletionService();
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
builder.Plugins.AddFromType<ConversationSummaryPlugin>();

var kernel = builder.Build();

// Create a template for chat with settings
var chat = kernel.CreateFunctionFromPrompt(
    new PromptTemplateConfig()
    {
        Name = "Chat",
        Description = "Chat with the assistant.",
        Template = @"{{ConversationSummaryPlugin.SummarizeConversation $history}}
        User: {{$request}}
        Assistant: ",
        TemplateFormat = "semantic-kernel",
        InputVariables = [
            new() { Name = "history", Description = "The history of the conversation.", IsRequired = false, Default = "" },
            new() { Name = "request", Description = "The user's request.", IsRequired = true }
        ],
        ExecutionSettings = {
            { "default", new OpenAIPromptExecutionSettings() {
                MaxTokens = 1000,
                Temperature = 0
            } },
            { "gpt-3.5-turbo", new OpenAIPromptExecutionSettings() {
                ModelId = "gpt-3.5-turbo-0613",
                MaxTokens = 4000,
                Temperature = 0.2
            } },
            { "gpt-4", new OpenAIPromptExecutionSettings() {
                ModelId = "gpt-4-1106-preview",
                MaxTokens = 8000,
                Temperature = 0.3
            } }
        }
    }
);

// Create chat history and choices
ChatHistory history = [];

// Start the chat loop
while (true)
{
    // Get user input
    Console.Write("User > ");
    var request = Console.ReadLine();

    // Get chat response
    var chatResult = kernel.InvokeStreamingAsync<StreamingChatMessageContent>(
        chat,
        new() {
            { "request", request },
            { "history", string.Join("\n", history.Select(x => x.Role + ": " + x.Content)) }
        }
    );

    // Stream the response
    string message = "";
    await foreach (var chunk in chatResult)
    {
        if (chunk.Role.HasValue) Console.Write(chunk.Role + " > ");
        message += chunk;
        Console.Write(chunk);
    }
    Console.WriteLine();

    // Append to history
    history.AddUserMessage(request!);
    history.AddAssistantMessage(message);
}