namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public class Document<TInput, TProcessor> : IDocument<TInput>
        where TInput : class
        where TProcessor : class, ITemplateProcessor<TInput>, new()
    {
        private readonly IPdfConverter _pdfConverter;
        private readonly SupportedTypes _type = new TProcessor().Type;

        public Document(IPdfConverter pdfConverter)
        {
            _pdfConverter = pdfConverter;
        }

        public string Mime => _type.ToMime();

        public string Extension => _type.ToFileExtension();

        protected virtual IGenerator<TInput> CreateGenerator()
            => _type switch
            {
                SupportedTypes.PDF => new PdfGenerator<TInput, TProcessor>(_pdfConverter),
                SupportedTypes.DOCX => new DocxGenerator<TInput, TProcessor>(),
                SupportedTypes.XLSX => new XlsxGenerator<TInput, TProcessor>(),
                SupportedTypes.JPEG => throw new NotImplementedException("No generator implemented for type JPEG"),
                _ => throw new NotImplementedException($"No generator implemented for type {_type}")
            };

        public virtual Stream Create(TInput input)
        {
            Stream stream = CreateGenerator().Generate(input);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
