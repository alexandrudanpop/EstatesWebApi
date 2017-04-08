namespace Api.DAL.DataServices
{
    using System.Collections.Generic;

    public interface IDataService<T>
        where T : class
    {
        string Create(T dto);

        void Delete(string id);

        IReadOnlyList<T> GetAll();

        T GetById(string id);

        IReadOnlyList<T> GetFilteredBy(string name);

        bool Update(T dto);
    }
}