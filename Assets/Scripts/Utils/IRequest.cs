using System.Threading;
using Cysharp.Threading.Tasks;

public interface IRequest<T> : IRequestBase
{
    UniTask<T> ExecuteAsync(CancellationToken token);
}