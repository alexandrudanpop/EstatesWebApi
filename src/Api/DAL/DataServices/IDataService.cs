using System.Collections.Generic;

namespace Api.DAL.DataServices
{
    public interface IDataService<T> where T: class
    {
        T GetById(string id);

        IReadOnlyList<T> GetAll();

        IReadOnlyList<T> GetFilteredBy(string name);

        string Create(T dto);

        bool Update(T dto);

        void Delete(string id);
    }
}
