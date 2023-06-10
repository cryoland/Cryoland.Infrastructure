namespace Cryoland.Infrastructure.Documents.Abstractions.TypedGenerators
{
    internal class XlsxGenerator<TInput, TProcessor> : DocxGenerator<TInput, TProcessor>
        where TInput : class
        where TProcessor : class, ITemplateProcessor<TInput>, new()
    {
    }
}
