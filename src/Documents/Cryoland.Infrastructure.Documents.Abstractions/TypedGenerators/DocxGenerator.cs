namespace Cryoland.Infrastructure.Documents.Abstractions.TypedGenerators
{
    internal class DocxGenerator<TInput, TProcessor> : Generator<TInput>
        where TInput : class
        where TProcessor : class, ITemplateProcessor<TInput>, new()
    {
        public override Stream Generate(TInput input)
        {
            new TProcessor().Process(input, _stream);
            return _stream;
        }
    }
}
