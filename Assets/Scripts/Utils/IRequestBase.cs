public interface IRequestBase
{
    bool IsRunning { get; }
    void Cancel();
}