using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FunctionGenerator;

public class DefinitionAggregate : ISyntaxReceiver
{
    public List<Capture> Captures { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Define" } } attr)
        {
            return;
        }
        var method = attr.GetParent<MethodDeclarationSyntax>();
        var key = method.Identifier.Text;
        Captures.Add(new Capture(key, method));
    }
    public record Capture(string Keys, MethodDeclarationSyntax Methods);
}


