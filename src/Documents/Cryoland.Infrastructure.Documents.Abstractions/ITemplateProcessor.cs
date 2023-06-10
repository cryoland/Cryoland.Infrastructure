namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public interface ITemplateProcessor<in TInput>
        where TInput : class
    {
        void Process(TInput input, Stream targetStream);

        SupportedTypes Type { get; }
    }
}
