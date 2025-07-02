# Microsoft.Extensions.AI.Evaluation.Tests.Ollama

Based on [dotnet/ai-sample - microsoft-extensions-ai-evaluation](https://github.com/dotnet/ai-samples/tree/main/src/microsoft-extensions-ai-evaluation)

- Run the ollama through docker: `docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama`
- Pull the llama2 model: `docker exec -it ollama ollama pull llama2`
