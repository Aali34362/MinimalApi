using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FunctionGenerator;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public int Index  { get; set; }
    public DefinitionAggregate Definition { get; } = new();
    public GivethsAggregation Giveths  { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        Definition.OnVisitSyntaxNode(syntaxNode);
        Giveths.OnVisitSyntaxNode(syntaxNode);
        if (syntaxNode is ClassDeclarationSyntax)
        {
            //File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMddTHHmmsss")}.txt", syntaxNode.GetText().ToString());
            File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{Index++.ToString()}.txt", syntaxNode.GetText().ToString());
        }
    }
}


