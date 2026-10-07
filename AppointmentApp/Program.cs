using Microsoft.EntityFrameworkCore;
namespace AppointmentApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
          

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(
            //        builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            

            var app = builder.Build();

           

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
