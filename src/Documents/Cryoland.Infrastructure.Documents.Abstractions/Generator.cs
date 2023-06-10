namespace Cryoland.Infrastructure.Documents.Abstractions
{
    internal abstract class Generator<TInput> : IGenerator<TInput>, IDisposable
        where TInput : class
    {
        protected Stream _stream = new MemoryStream();
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _stream.Dispose();
            }

            _disposed = true;
        }

        public abstract Stream Generate(TInput input);
    }
}
