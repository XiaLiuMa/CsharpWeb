using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Core
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = -1;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";

                    if (context.Request.Path.Equals("/") || context.Request.Path.Equals("/home/index"))
                    {
                        context.Response.Redirect("/home/login");
                    }

                }
                //else if (statusCode == )
                //{
                //    msg = "未找到服务";
                //}
                //else if (statusCode == )
                //{
                //    msg = "请求错误";
                //}
                //else if (statusCode != )
                //{
                //    msg = "未知错误";
                //}
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var result = JsonConvert.SerializeObject(new ApiResult() { IsSuccess = false, Msg = msg, Type = statusCode.ToString() });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}
