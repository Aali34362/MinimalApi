using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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


