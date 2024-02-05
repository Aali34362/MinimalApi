using Microsoft.CodeAnalysis;

namespace FunctionGenerator;

[Generator]
public class FunctionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        //Task.Delay(10000).GetAwaiter().GetResult();
        //#pragma warning disable RS1035 // Do not use APIs banned for analyzers
        // File.WriteAllText(@"F:\\Programming\\Asp\\AspCore\\v8\\MinimalApi\\MinimalApi\\FunctionGenerator\\log.txt", "diag");
        //#pragma warning restore RS1035 // Do not use APIs banned for analyzers
        /*using (StreamWriter writer = new StreamWriter("F:\\Programming\\Asp\\AspCore\\v8\\MinimalApi\\MinimalApi\\FunctionGenerator\\log.txt"))
        {
            writer.Write("your content");
        }*/
        foreach (var compilationSource in context.Compilation.SyntaxTrees)
        {
            File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", compilationSource.GetText().ToString());
        }
        throw new Exception("Hello \n qw");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", "Hello");
        throw new Exception("Hello \n qw");
    }
}

/*public class SourceClassGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        throw new Exception("Hello \n Worlds");
    }
}*/


