namespace HttpConsumer.Domain.DTOs
{
    public class HttpResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T ResponseBody { get; set; }
    }
}
