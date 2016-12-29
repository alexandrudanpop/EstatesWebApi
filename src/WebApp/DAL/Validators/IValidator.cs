using System.Collections.Generic;

namespace WebApp.DAL.Validators
{
    public interface IValidator<T> where T: class
    {
        IReadOnlyList<string> Validate(T dto);
    }
}