 

public static class WebApplicationExtensions
{
    public static void AddCorsForWebApplication(this WebApplication app)
    {
        
        app.UseCors(x =>
        {
            x.WithOrigins("http://localhost:8080");
            x.AllowAnyHeader();
            x.AllowAnyMethod();
        });
         
        
    }
}