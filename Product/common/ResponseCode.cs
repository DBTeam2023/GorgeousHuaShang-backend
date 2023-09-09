<<<<<<<< HEAD:Order/common/ResponseCode.cs
﻿namespace Order.common
========
﻿namespace Product.common
>>>>>>>> Product:Product/common/ResponseCode.cs
{
    /**
     * @implnote enumeration for com response codes
     */
    public class ResponseCode
    {
        public static readonly ResponseCode SUCCESS = new ResponseCode(200,"success");
        public static readonly ResponseCode NOT_LOGIN = new ResponseCode(300,"not login");
        public static readonly ResponseCode NO_PERMISSION = new ResponseCode(310,"permission denied");
        public static readonly ResponseCode BAD_REQUEST = new ResponseCode(400,"bad request");
        public static readonly ResponseCode SYSTEM_ERROR = new ResponseCode(500,"system error");

        public int code;
        public string message;
        ResponseCode(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public int getCode()
        {
            return code;
        }

        public void setCode(int code)
        {
            this.code = code;
        }

        public String getMessage()
        {
            return message;
        }

        public void setMessage(String message)
        {
            this.message = message;
        }
    }

    
}
