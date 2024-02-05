using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class WebServer {

    static Dictionary<int, string> comments;

    public static void Main(string[] args) {

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // 不讲HTML
        // 消息队列
        app.MapGet("/", () => {
            // return html
            // contetn-type: text/html
            // 服务端返回给客户端的也全是 字节流-byte[]

            // byte[] 文件
            // byte[] data = Encoding.UTF8.GetBytes("<h1>Hello World</h1>");
            byte[] data = File.ReadAllBytes("index.html");
            System.Console.WriteLine(data.Length);
            return Results.Bytes(data, "text/html");
        });

        app.MapPost("/submit", (HttpContext ctx) => {
            // 1. 从客户端收到的数据也是 字节流-byte[]
            HttpRequest req = ctx.Request;
            req.Form.TryGetValue("CommentSubmit", out var comment);
            foreach (var item in req.Form) {
                System.Console.WriteLine(item.Key + " : " + item.Value);
                if (item.Value == "Jack") {
                    return Results.StatusCode(404);
                } else if (item.Value == "Shut") {
                    app.StopAsync();
                }
            }

            return Results.Ok();
        });

        app.MapGet("/comments", () => {

        });
        // commnets

        // download
        app.MapGet("/dl", () => {
            byte[] data = File.ReadAllBytes("myfile.zip");
            return Results.File(data, "application/zip", "myfile.zip");
        });

        app.MapGet("/blog", () => {
            byte[] data = File.ReadAllBytes("blog.html");
            return Results.Bytes(data, "text/plain");
        });

        app.Run();

    }
}

