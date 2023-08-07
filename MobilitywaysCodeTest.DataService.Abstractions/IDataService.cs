using MobilitywaysCodeTest.Entities;

namespace MobilitywaysCodeTest.DataService.Abstractions
{
    public interface IDataService<T> where T : Entity
    {
        Task Create(T entity);
        Task<T?> Read(int id);
        // MAINTAINABILITY/REUSABILITY: This method would almost certainly have filter, sort and pagination options if this were production software
        Task<List<T>> ReadMultiple();
    }
}