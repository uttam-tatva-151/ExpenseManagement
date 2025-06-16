using CashCanvas.Web.Extensions;
using CashCanvas.Web.Hubs;
using CashCanvas.Web.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddConfigurationBindings(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSignalR();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
                    name: "default", 
                    pattern: "{controller=Authentication}/{action=Index}");

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
