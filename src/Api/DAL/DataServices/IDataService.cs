using System.Collections.Generic;

namespace Api.DAL.DataServices
{
    public interface IDataService<T> where T: class
    {
        T GetById(int id);

        IReadOnlyList<T> GetAll();

        IReadOnlyList<T> GetFilteredBy(string name);

        int? Create(T dto);

        bool Update(T dto);

        void Delete(int id);
    }
}
