using Microsoft.EntityFrameworkCore;

namespace WebApp.Model
{
    public class DbInitializer
    {
        public static void Initialize()
        {
            using (var context = new DataBaseContext())
            {
                context.Database.Migrate();
            }
        }
    }
}