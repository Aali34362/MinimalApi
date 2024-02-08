using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
        //foreach (var compilationSource in context.Compilation.SyntaxTrees)
        //{
        //    compilationSource.GetRoot().ChildNodes()
        //        .OfType<ClassDeclarationSyntax>();
        //    File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", compilationSource.GetText().ToString());
        //}

        //foreach (var compilationSyntaxTree in context.Compilation.SyntaxTrees)
        //{
        //    foreach (var classDeclarationSyntax in compilationSyntaxTree.GetRoot().ChildNodes().OfType<ClassDeclarationSyntax>())
        //    {
        //        File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", classDeclarationSyntax.GetText().ToString());
        //    }
        //}

        var receiver = (MainSyntaxReceiver) context.SyntaxReceiver;

        //Part 1
        /*var output = @"
                public class Test
                {
                    public static void P() => Console.WriteLine(""Hello New World"");
                }";
        context.AddSource("hello.g.cs",output);*/
        //Part II
        /*var sb = new StringBuilder();
        sb.Append("public class Test1");
        sb.Append("{");
        sb.Append("public static void P() => Console.WriteLine(\"Hello New World\");");
        sb.Append("}");
        context.AddSource("hellos.g.cs", sb.ToString());*/

        //Part III
        /*var source = CompilationUnit()
.WithMembers(
    SingletonList<MemberDeclarationSyntax>(
        ClassDeclaration("Test")
        .WithModifiers(
            TokenList(
                Token(SyntaxKind.PublicKeyword)))
        .WithMembers(
            SingletonList<MemberDeclarationSyntax>(
                MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier("P"))
                .WithModifiers(
                    TokenList(
                        new[]{
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.StaticKeyword)}))
                .WithExpressionBody(
                    ArrowExpressionClause(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("Console"),
                                IdentifierName("WriteLine")))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal(""))),
                                        MissingToken(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("Hello")),
                                        MissingToken(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("New")),
                                        MissingToken(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName("World")),
                                        MissingToken(SyntaxKind.CommaToken),
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal("")))})))))
                .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken))))))
.NormalizeWhitespace();
        context.AddSource("HelloWorld.g.cs", source.GetText(Encoding.UTF8));*/
        throw new Exception("Hello world \n qw");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        //File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", "Hello");
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
        //throw new Exception("Hello \n qw");
    }
}

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
public class Capture
{
    public string Keys { get; set; }
    public MethodDeclarationSyntax Methods { get; set; }

    public Capture(string Keys, MethodDeclarationSyntax Methods)
    {
        this.Keys = Keys;
        this.Methods = Methods;
    }
}
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
}

public class GivethsAggregation : ISyntaxReceiver
{
    public List<Capture> Captures { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {        
        if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Give" } } attr)
        {
            return;
        }
        var target = (attr.ArgumentList.Arguments.Single() as LiteralExpressionSyntax).Token.ValueText;
        //var key = method.Identifier.Text;
       // Captures.Add(new Capture(key, method));
    }
}

public static class SyntaxNodeExtensions
{
    public static T GetParent<T>(this SyntaxNode node)
    {
        var parent = node.Parent;
        while(true)
        {
            if(parent == null)
            {
                throw new Exception();
            }
            if(parent is T t)
            {
                return t;
            }
            parent = parent.Parent;
        }
    }
}



[Generator] 
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //foreach (var compilationSource in context.CompilationProvider.)
        //{
        //    compilationSource.GetRoot().ChildNodes()
        //        .OfType<ClassDeclarationSyntax>();

        //    File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", compilationSource.GetText().ToString());
        //}
        //File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyMMdd")}.txt", "Hellos");

        var provider = context
           .SyntaxProvider
           .CreateSyntaxProvider(
               (node, _) => node is ClassDeclarationSyntax,//predicate
               (syntaxContext, _) => (ClassDeclarationSyntax)syntaxContext.Node//transform
           ).Where(x => x is not null);

        var compilation = context
            .CompilationProvider
            .Combine(provider.Collect());
        context.RegisterSourceOutput(compilation, Execute);

        throw new Exception("Hellos \n Worlds");
    }

    private void Execute(SourceProductionContext context, (Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes) tuple)
    {
        var (compilation, classes) = tuple;
        var code = @"
                    //<auto-generated/>
                    public class Test
                    {
                        public static void P() => Console.WriteLine(""Hello New World"");
                    }";
        context.AddSource("helloworld.g.cs", code);
    }
}


