using Microsoft.Extensions.AI.Evaluation.Quality;
using Microsoft.Extensions.AI.Evaluation.Tests.Shared.Extensions;
using AwesomeAssertions;

namespace Microsoft.Extensions.AI.Evaluation.Tests.Ollama;

public class GroundednessEvaluatorTests
{
    private ChatConfiguration? _chatConfigurationForEvaluation;
    private static string _userRequest = "What is the tracking for the order 123?";
    private readonly IList<ChatMessage> s_messages = [
        new ChatMessage(
            ChatRole.System,
            """
            You are an AI assistant that can answer questions related to ecommerce.
            Keep your responses concise staying under 100 words as much as possible.
            Use the imperial measurement system for all measurements in your response.
            """),
            new ChatMessage(ChatRole.User, _userRequest)
    ];


    public GroundednessEvaluatorTests()
    {
        IChatClient chatClient = new OllamaChatClient(new Uri("http://localhost:11434"), "llama2");
        _chatConfigurationForEvaluation = new ChatConfiguration(chatClient);
    }

    [Fact]
    public async Task GroundednessEvaluatorTest()
    {
        string groundTruth = "OrderId is 123, Tracking code is TKG_ABC.";
        GroundednessEvaluatorContext groundingContextForGroundednessEvaluator = new GroundednessEvaluatorContext(groundTruth);

        IEvaluator groundednessEvaluator = new GroundednessEvaluator();

        string fakeLlmResponse = "OrderId is 123, Tracking code is TKG_ABC.";
        ChatMessage chatMessageResponse = new ChatMessage(ChatRole.Assistant, fakeLlmResponse);
        ChatResponse? chatResponse = new ChatResponse(chatMessageResponse);

        EvaluationResult evaluationResult = await groundednessEvaluator.EvaluateAsync(s_messages, chatResponse, _chatConfigurationForEvaluation, [groundingContextForGroundednessEvaluator]);

        foreach (var result in evaluationResult.Metrics)
        {
            if (result.Value is EvaluationMetric evaluationMetric)
            {
                Assert.NotNull(evaluationMetric);
                if (evaluationMetric.Name == GroundednessEvaluator.GroundednessMetricName)
                {
                    Assert.False(evaluationMetric.Interpretation?.Failed, evaluationMetric.GenerateBecause(_userRequest, chatMessageResponse.Text));

                    EvaluationRating[] expectedRatings = [EvaluationRating.Good, EvaluationRating.Exceptional];
                    evaluationMetric.Interpretation?.Rating.Should().BeOneOf(expectedRatings, because: evaluationMetric.GenerateBecause(_userRequest, chatMessageResponse.Text));
                }
            }
        }
    }
}
