using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyBlog.BLL.Models;
using System.Net.Http;
using System.Text;

namespace MyBlog.WebService.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            var request = context.Request;

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //get body string here...
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;  //rewinding the stream to 0

            var resultUser = await userManager.GetUserAsync(context.User);

            var paramString = "";
            foreach (var param in request.Query)
            {
                paramString += $"{param.Key} = {request.Query[param.Key]}; ";
            }

            var textLog = $"Запрос: {request.Path}\nТело запроса: {requestBody}\nПараметры запроса: {paramString}\nМетод: {request.Method}\n";

            if (resultUser != null)
            {
                textLog += $"От пользователя: {resultUser.Email}";
            }

            _logger.LogInformation(textLog);

            HttpResponse response = context.Response;
            var originalResponseBody = response.Body;
            using var newResponseBody = new MemoryStream();
            response.Body = newResponseBody;

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ОШИБКА {ex.Message}\n{ex.StackTrace}\n{textLog}");
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);

            _logger.LogInformation($"Статус ответа: {response.StatusCode}\nОтвет: {responseBodyText}");
        }
    }
}
