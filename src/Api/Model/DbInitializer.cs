using Microsoft.EntityFrameworkCore;

namespace Api.Model
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