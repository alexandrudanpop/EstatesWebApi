using Microsoft.EntityFrameworkCore;

namespace WebApp.Model
{
    public class DBInitialization
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
