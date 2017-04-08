namespace Core.Contracts
{
    using System.Collections.Generic;

    public interface IValidator<in T>
        where T : class
    {
        IReadOnlyList<string> Validate(T dto);
    }
}