namespace Cryoland.Infrastructure.Documents.Abstractions
{
    /// <summary>
    /// Produces stream containing required document and ready to pass further
    /// </summary>
    public interface IDocument<in TInput>
        where TInput : class
    {
        /// <summary>
        /// Creates output <see cref="Stream"/> containing required document
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns></returns>
        Stream Create(TInput input);

        /// <summary>
        /// Document MIME
        /// </summary>
        string Mime { get; }

        /// <summary>
        /// Document file extension
        /// </summary>
        string Extension { get; }
    }
}
