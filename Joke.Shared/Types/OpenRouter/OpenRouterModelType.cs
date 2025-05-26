using System.ComponentModel;

namespace Joke.Shared.Types.OpenRouter;

public enum OpenRouterModelType
{
    [Description("default")]
    Default,
    [Description("google/gemini-2.5-flash-preview")]
    Google,
    [Description("meta-llama/llama-4-maverick")]
    Meta,
    [Description("openai/gpt-4o-mini")]
    OpenAi,
    [Description("qwen/qwen3-235b-a22b")]
    Qwen
}