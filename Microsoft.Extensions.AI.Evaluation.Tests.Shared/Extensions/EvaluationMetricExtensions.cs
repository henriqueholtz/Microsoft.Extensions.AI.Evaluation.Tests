namespace Microsoft.Extensions.AI.Evaluation.Tests.Shared.Extensions;

public static class EvaluationMetricExtensions
{
    public static string GenerateBecause(this EvaluationMetric evaluationMetric, string userRequest, string chatResponseText) =>
        $"\n------------------------------------- \nFailed: {evaluationMetric.Interpretation?.Failed} \n" +
        $"Reason: {evaluationMetric.Reason} \n" +
        $"Interpretation Reason: {evaluationMetric.Interpretation?.Reason} \n" +
        $"Interpretation Rating: {evaluationMetric.Interpretation?.Rating} \n" +
        $"Diagnostics Count: {evaluationMetric.Diagnostics?.Count ?? 0}: {string.Join(";", evaluationMetric.Diagnostics?.Select(d => d.Message) ?? [])} \n" +
        $"\n------------------------------------- \nQuery: {userRequest} \n" +
        $"\n------------------------------------- \nChatResponse: {chatResponseText} \n";
}
