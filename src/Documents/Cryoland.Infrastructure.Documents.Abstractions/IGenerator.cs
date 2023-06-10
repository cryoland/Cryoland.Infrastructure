namespace Cryoland.Infrastructure.Documents.Abstractions
{
    public interface IGenerator<in TInput>
        where TInput : class
    {
        Stream Generate(TInput input);
    }
}
