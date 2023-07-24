using UserIdentification.common;

namespace UserIdentification.web
{
    /**
     * @author sty
     * @implnote encapsulate error code and messages in the com response body.
     *          Constructors for different error types are designed for internal server information.
     *          User can specify the type of response instead of directly modify http response type.
     */
    public class ComResponse<T>
    {
        public int code { get; set; }

        public String msg { get; set; }

        public T data { get; set; }

        public static ComResponse<T> success(T data)
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.SUCCESS.getCode())
                    .msg(ResponseCode.SUCCESS.getMessage())
                    .data(data).build();
        }

        public static ComResponse<string> success()
        {
            return ComResponse<string>.builder()
                    .code(ResponseCode.SUCCESS.getCode())
                    .msg(ResponseCode.SUCCESS.getMessage())
                    .data("").build();
        }

        public static ComResponse<T> error()
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.SYSTEM_ERROR.getCode())
                    .msg(ResponseCode.SYSTEM_ERROR.getMessage())
                    .build();
        }

        public static ComResponse<T> error(String code, String msg)
        {
            return ComResponse<T>.builder()
                    .code(int.Parse(code))
                    .msg(msg)
                    .build();
        }

        public static ComResponse<T> error(String msg)
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.SYSTEM_ERROR.getCode())
                    .msg(msg)
                    .build();
        }

        public static ComResponse<T> invalid(String message)
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.BAD_REQUEST.getCode())
                    .msg(message)
                    .build();
        }

        public static ComResponse<T> authFaild(String message)
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.NOT_LOGIN.getCode())
                    .msg(message)
                    .build();
        }

        public static ComResponse<T> authFaild()
        {
            return ComResponse<T>.builder()
                    .code(ResponseCode.NOT_LOGIN.getCode())
                    .msg(ResponseCode.NOT_LOGIN.getMessage())
                    .build();
        }

        public static ComResponse<T> invalid()
        {
            return invalid(ResponseCode.BAD_REQUEST.getMessage());
        }

        public ComResponse(int code, String msg, T data)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }

        public ComResponse() { }

        class ComResponseBuilder<T>
        {
            ComResponse<T> builder;
            public ComResponseBuilder()
            {
                builder = new ComResponse<T>();
            }

            public ComResponseBuilder<T> code(int responseCode)
            {
                builder.code = responseCode;
                return this;
            }

            public ComResponseBuilder<T> msg(String msg)
            {
                builder.msg = msg;
                return this;
            }

            public ComResponseBuilder<T> data(T data)
            {
                builder.data = data;
                return this;
            }

            public ComResponse<T> build()
            {
                return builder;
            }
        }

        private static ComResponseBuilder<T> builder()
        {
            return new ComResponseBuilder<T>();
        }
    }
}
