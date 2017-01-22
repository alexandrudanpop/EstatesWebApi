using System.Collections.Generic;

namespace Api.Validators
{
    public interface IValidator<T> where T: class
    {
        IReadOnlyList<string> Validate(T dto);
    }
}