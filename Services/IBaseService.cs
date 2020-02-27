using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public interface IBaseService
    {
        T GetWithKeys<T>(params object[] keys) where T : class;
        T GetWithKeys<T>(object[] keys, IEnumerable<string> navications = null, IEnumerable<string> collections = null) where T : class;
        IQueryable<T> GetAll<T>(string included = "", bool readOnly = false) where T : class;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null, bool readOnly = false, string included = "") where T : class;
        T Insert<T>(T entity) where T : class;
        IEnumerable<T> InsertMany<T>(IEnumerable<T> entities) where T : class;
        T Remove<T>(T entity) where T : class;
        T Remove<T>(params object[] keys) where T : class;
        IEnumerable<T> RemoveMany<T>(IEnumerable<T> entities) where T : class;
        IEnumerable<T> RemoveMany<T>(IEnumerable<object[]> keysList) where T : class;
    }
}