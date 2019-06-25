using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchLibrary.Models
{
    public class ModelAction
    {
        private static readonly Lazy<ModelAction> Lazy = new Lazy<ModelAction>(() => new ModelAction());
        public static ModelAction Instance => Lazy.Value;
        public ModelAction()
        {
        }

        public virtual bool Delete<T>(T input) where T : class, ICommon
        {
            return SqlLauncher.Delete(input);
        }

        public virtual T Insert<T>(T input) where T : class, ICommon
        {
            return SqlLauncher.Insert(input);
        }

        public virtual T Update<T>(T input) where T : class, ICommon
        {
            return SqlLauncher.Update(input);
        }

        public virtual List<T> GetAll<T>(Expression<Func<T, bool>> expression = null) where T :class, ICommon
        {
            return LunchLibrary.SqlLauncher.GetAll<T>(expression);
        }

        public virtual T Get<T>(Expression<Func<T, bool>> expression = null) where T : class, ICommon
        {
            try
            {
                return LunchLibrary.SqlLauncher.Get(expression);
            }
            catch (Exception ex)
            {
                Log.Report(ex);
            }
            return null;
        }
    }
}
