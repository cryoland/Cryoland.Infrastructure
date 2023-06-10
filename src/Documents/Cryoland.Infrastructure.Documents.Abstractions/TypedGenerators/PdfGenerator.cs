namespace Cryoland.Infrastructure.Documents.Abstractions.TypedGenerators
{
    internal class PdfGenerator<TInput, TProcessor> : Generator<TInput>
        where TInput : class
        where TProcessor : class, ITemplateProcessor<TInput>, new()
    {
        private readonly IPdfConverter _pdfConverter;

        public PdfGenerator(IPdfConverter pdfConverter)
        {
            _pdfConverter = pdfConverter;
        }

        public override Stream Generate(TInput input)
        {
            TProcessor processor = new();
            processor.Process(input, _stream);
            return _pdfConverter.ProcessFileAsync(_stream, processor.Type).GetAwaiter().GetResult();
        }
    }
}
