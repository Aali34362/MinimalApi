using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FunctionGenerator;

public class GivethsAggregation : ISyntaxReceiver
{
    public List<Capture> Captures { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {        
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Give" } } attr)
        {
            return;
        }
        var target = (attr.ArgumentList.Arguments.Single().Expression as LiteralExpressionSyntax).Token.ValueText;
        var method = attr.GetParent<MethodDeclarationSyntax>();
        var clazz = attr.GetParent<ClassDeclarationSyntax>();
        Captures.Add(new Capture(target, method, clazz));
    }
    public record Capture(string TargetImplementation, MethodDeclarationSyntax Method, ClassDeclarationSyntax Clazz);
}


