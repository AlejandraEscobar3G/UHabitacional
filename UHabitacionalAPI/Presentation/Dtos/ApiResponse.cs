namespace UHabitacionalAPI.Presentation.Dtos
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T> { Code = StatusCodes.Status200OK, Success = true, Data = data };
        }

        public static ApiResponse<T> Created(T data)
        {
            return new ApiResponse<T> { Code = StatusCodes.Status201Created, Success = true, Data = data };
        }

        public static ApiResponse<T> Fail(int code, T data, List<string> errors)
        {
            return new ApiResponse<T> { Code = code, Success = false, Data = data, Errors = errors };
        }
    }
}
