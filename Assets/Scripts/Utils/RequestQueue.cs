using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class RequestQueue : IRequestQueue, IDisposable
{
    private readonly Queue<IRequestWrapper> _queue = new();
    private IRequestWrapper _active;
    private bool _isDisposed; 
    private bool _isProcessing;

    public UniTask<T> Enqueue<T>(IRequest<T> request)
    {
        var wrapper = new RequestWrapper<T>(request);
        _queue.Enqueue(wrapper);

        if (_active == null)
            ProcessNext().Forget();

        return wrapper.Task;
    }


    private async UniTaskVoid ProcessNext()
    {
        if (_isProcessing) return;
        _isProcessing = true;

        try
        {
            while (_queue.Count > 0 && !_isDisposed)
            {
                _active = _queue.Dequeue();
                await _active.Run();
                _active = null;
            }
        }
        finally
        {
            _isProcessing = false;
        }
    }
    public void Cancel(Func<IRequestBase, bool> match)
    {
        if (_active != null && match(_active.Request))
            _active.Cancel();

        var remaining = new Queue<IRequestWrapper>();
        while (_queue.Count > 0)
        {
            var r = _queue.Dequeue();
            if (!match(r.Request)) remaining.Enqueue(r);
        }

        while (remaining.Count > 0)
            _queue.Enqueue(remaining.Dequeue());

        if (_active == null && _queue.Count > 0)
            ProcessNext().Forget();
    }


    public void Dispose()
    {
        _isDisposed = true;
        _active?.Cancel();
        _queue.Clear();
    }

    private interface IRequestWrapper
    {
        IRequestBase Request { get; }
        UniTask Run();
        void Cancel();
    }

    private class RequestWrapper<T> : IRequestWrapper
    {
        public IRequestBase Request => _request;
        private readonly IRequest<T> _request;
        private readonly UniTaskCompletionSource<T> _tcs = new();
        private CancellationTokenSource _cts;

        public UniTask<T> Task => _tcs.Task;

        public RequestWrapper(IRequest<T> request)
        {
            _request = request;
        }

        public async UniTask Run()
        {
            _cts = new CancellationTokenSource();

            try
            {
                var result = await _request.ExecuteAsync(_cts.Token);
                _tcs.TrySetResult(result);
            }
            catch (OperationCanceledException)
            {
                _tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                _tcs.TrySetException(ex);
            }
        }

        public void Cancel()
        {
            _cts?.Cancel();
            _request.Cancel();
        }
    }
}
