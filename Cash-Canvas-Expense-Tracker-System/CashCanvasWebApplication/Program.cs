using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans.Configuration;
using CashCanvas.Web.Extensions;
using CashCanvas.Web.Hubs;
using CashCanvas.Web.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
RouteSettings? routeSettings = builder.Configuration.GetSection(Constants.DEFAULT_ROUTE_CONFIG).Get<RouteSettings>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddConfigurationBindings(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSignalR();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/ErrorHandler/PageNotFound");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<TransactionHub>("/transactionHub");

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<JWTMiddleware>();

app.MapControllerRoute( 
                    name: routeSettings?.DefaultRouteName??string.Empty, 
                    pattern: routeSettings?.DefaultRoutePattern ?? string.Empty);

app.MapFallback(context =>
{
    string path = context.Request.Path.Value ?? string.Empty;

    // Avoid redirect loop if already on error page
    if (path != null && path.StartsWith("/ErrorHandler", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = 404;
        return Task.CompletedTask;
    }

    context.Response.Redirect("/ErrorHandler/PageNotFound");
    return Task.CompletedTask;
});
app.Run();
