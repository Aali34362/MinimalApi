using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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


        var receiver = (MainSyntaxReceiver)context.SyntaxReceiver;
        if (receiver != null)
        {
            foreach (var giveth in receiver.Giveths.Captures)
            {
                var def = receiver.Definition.Captures.FirstOrDefault(x => x.Keys == giveth.TargetImplementation);
                var output = giveth.Clazz.WithMembers(new(CreateGiveMethod(giveth.Method, def.Methods))).NormalizeWhitespace();
                //throw new Exception(output.ToFullString().ReplaceLineEndings(""));
                //throw new Exception(giveth.Clazz.ToFullString().ReplaceLineEndings(""));
                context.AddSource($"{giveth.Clazz.Identifier.Text}.g.cs", output.GetText(Encoding.UTF8));

            }
        }
        throw new Exception(receiver?.Definition.Captures.First().Methods.ToFullString().ReplaceLineEndings(""));
    }

    private MethodDeclarationSyntax CreateGiveMethod(MethodDeclarationSyntax givethMethod, MethodDeclarationSyntax def)
    {
        return MethodDeclaration(givethMethod.ReturnType, givethMethod.Identifier)
                .WithModifiers(givethMethod.Modifiers)
                .WithBody(def.Body);               
                //.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        //File.WriteAllText($@"F:\Programming\Asp\AspCore\v8\MinimalApi\MinimalApi\Log\log_{DateTime.Now.ToString("yyyyyMMdd")}.txt", "Hello");
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
        //throw new Exception("Hello \n qw");
    }
}


