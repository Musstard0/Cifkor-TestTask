using System;
using Cysharp.Threading.Tasks;

public interface IRequestQueue
{
    UniTask<T> Enqueue<T>(IRequest<T> request);
    void Cancel(Func<IRequestBase, bool> match);
}