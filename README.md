# Technical Task

This is a backend technical challenge with a specific focus on WebSockets, APIs & Concurrency.

## Overview

The context prompt is as follows:

_"At **Company**, we build real-time systems that process, enrich, and exchange language data across APIs and WebSockets. This challenge tests your ability to build a simple event-driven backend pipeline, handle bidirectional WebSocket communication, and work with external APIs. Your task is to build two components that talk to each other via WebSockets."_ 

## Tech Stack

#### Languages & Frameworks 
- C# 13
- .NET 9 Blazor (Web Server with SignalR)

#### Libraries 
- _**Serilog**_: Detailed (and customisable) server + client console logging
- _**Radzen Blazor**_: Pre-build components for server UI as this was a bonus feature

**External Services:**  
- OpenRouter (see more below)

## OpenRouter

Both server and client make use of OpenRouter.ai models. This offers an abstraction layer for many model types including OpenAi, Google, Meta, Anthropic, Qwen etc. (and more). Furthermore, we can use the same request/response schema across all models including the response content, metadata (ie. usage) and more.

For now, the following models can be used via the configuration:
- Google Gemini 2.5 Flash Preview
- Meta Llama 4 Maverick
- OpenAI GPT 4o mini
- Qwen 235B A22B

Note: _Both Client and Server are free to use different models._

You can find more information and/or documentation for OpenRouter here: https://openrouter.ai

## Installation

_Below is a step-by-step guide on how to run the Server + Client._

1. Clone the repository and navigate in the root folder
   ```sh
   git clone https://github.com/louis-henry/JokeTechnicalTask.git
   cd JokeTechnicalTask
   ```
2. If running with OpenRouter, update the Server & Client appsettings.json file(s) in **~/Joke.Server/appsettings.json** and **~/Joke.Client/appsettings.json** (respectivley). You will need to enter an API key and toggle 'IsTestMode' to false.

3. To start the Server, navigate to the Server project and run. By default, this should run on http://localhost:5044
   ```sh
   cd Joke.Server
   dotnet run
   ```
4. Open a new terminal window (at the root repository) so you are able to run the Client separatley

5. To start the Client, navigate to the client project and run
   ```sh
   cd Joke.Client
   dotnet run
   ```

_Note: If you do not update the appsettings.json file from the Client or Server, both will automatically use Mocked test data with default processing delays._

The following values can be updated in the appsettings.json file: 
  ```json
   {
      "Serilog": {
        "MinimumLevel": {
          "Default": "Debug"
        }
      },
      "OpenRouter": {
        "ApiKey": "API_KEY",
        "IsTestMode": true,
        "TestDelay": 1500,
        "QueryModel": "Google"
      }
   }
  ```
- **OpenRouter.ApiKey** - OpenRouter API Key.

- **OpenRouter.IsTestMode** - When true, the sever or client will use mock data saved within the project. Set to false for live queries via OpenRouter.

- **OpenRouter.TestDelay** - Adds a delay (in ms) to simulate a realistic request/response time during live query conditions.

- **OpenRouter.QueryModel** - Selects which model to use. Currently, **"Google", "Meta", "Qwen" and "OpenAi"** are supported.

- **Serilig.MinimumLevel.Default** - Select the log level (ie. Debug, Warning, Information etc.).

## Notes

Some notes:

- Testing with OpenRouter can, at times, yield incosistent results. It's possible the response will not meet the expected criteria (ie. JSON schema).
- The project(s) default settings are setup to use a test mode. This will use mocked data.
- Test mode should be run for both Client and Server, or neither (as the Client will be unable to match mocked data via the supplied Guids).
- There is some basic unit test coverage. However, this is mostly as a demonstration as I did not want to spend too much time trying to cover absolutley everything.
