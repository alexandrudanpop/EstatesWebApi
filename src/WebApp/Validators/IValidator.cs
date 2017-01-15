using System.Collections.Generic;

namespace WebApp.Validators
{
    public interface IValidator<T> where T: class
    {
        IReadOnlyList<string> Validate(T dto);
    }
}