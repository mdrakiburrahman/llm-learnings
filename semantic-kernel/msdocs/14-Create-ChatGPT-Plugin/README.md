# Creating and using a OpenAI plugin

The `14-Create-ChatGPT-Plugin` console application shows the final solution to the [OpenAI plugin](https://learn.microsoft.com/en-us/semantic-kernel/agents/chatgpt-plugins) doc article.

## Prerequisites

- [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools) version 4.x.
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) is required to run this sample.
- Install the recommended extensions
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Semantic Kernel Tools](https://marketplace.visualstudio.com/items?itemName=ms-semantic-kernel.semantic-kernel) (optional)

You must also have the Azure Function located [here](../MathPlugin/) running locally, otherwise the sample will fail.

## Configuring the sample

The sample can be configured by using the command line with .NET [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to avoid the risk of leaking secrets into the repository, branches and pull requests.

This sample has been tested with the following models:

| Service      | Model type      | Model            | Model version | Supported |
| ------------ | --------------- | ---------------- | ------------: | --------- |
| OpenAI       | Text Completion | text-davinci-003 |             1 | ❌        |
| OpenAI       | Chat Completion | gpt-3.5-turbo    |             1 | ❌        |
| OpenAI       | Chat Completion | gpt-3.5-turbo    |          0301 | ❌        |
| Azure OpenAI | Chat Completion | gpt-3.5-turbo    |          0613 | ✅        |
| Azure OpenAI | Chat Completion | gpt-3.5-turbo    |          1106 | ✅        |
| OpenAI       | Chat Completion | gpt-4            |             1 | ❌        |
| OpenAI       | Chat Completion | gpt-4            |          0314 | ❌        |
| Azure OpenAI | Chat Completion | gpt-4            |          0613 | ✅        |
| Azure OpenAI | Chat Completion | gpt-4            |          1106 | ✅        |

This sample uses function calling, so it only works on models newer than 0613.

### Using .NET [Secret Manager](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

Configure an OpenAI endpoint

```powershell
cd 14-Create-ChatGPT-Plugin/Solution

dotnet user-secrets set "Global:LlmService" "OpenAI"

dotnet user-secrets set "OpenAI:ModelType" "chat-completion"
dotnet user-secrets set "OpenAI:ChatCompletionModelId" "gpt-4"
dotnet user-secrets set "OpenAI:ApiKey" "... your OpenAI key ..."
dotnet user-secrets set "OpenAI:OrgId" "... your ord ID ..."
```

Configure an Azure OpenAI endpoint

```powershell
cd 14-Create-ChatGPT-Plugin/Solution

dotnet user-secrets set "Global:LlmService" "AzureOpenAI"

dotnet user-secrets set "AzureOpenAI:DeploymentType" "chat-completion"
dotnet user-secrets set "AzureOpenAI:ChatCompletionDeploymentName" "gpt-35-turbo"
dotnet user-secrets set "AzureOpenAI:ChatCompletionModelId" "gpt-3.5-turbo-0613"
dotnet user-secrets set "AzureOpenAI:Endpoint" "... your Azure OpenAI endpoint ..."
dotnet user-secrets set "AzureOpenAI:ApiKey" "... your Azure OpenAI key ..."
```

## Running the sample

First, refer to the [README](./MathPlugin/README.md) in the MathPlugin\ folder
to start the Azure Function.

After starting the Azure Function and configuring the sample,
to build and run the console application, navigate to the [Solution](./Solution/) folder and hit `F5`.

To build and run the console application from the terminal use the following commands:

```powershell
cd Solution
dotnet build
dotnet run
```
