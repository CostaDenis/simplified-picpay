
namespace simplified_picpay.Views.ViewModels
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors ?? new();
        }

        public ResultViewModel(List<string> errors)
        {
            Errors = errors ?? new();
        }

        public ResultViewModel(string error)
        {
            Errors = new List<string> { error };
        }

        public T? Data { get; private set; }

        public List<string> Errors { get; private set; } = new();

        public bool Success => Errors == null || Errors.Count == 0;
    }
}