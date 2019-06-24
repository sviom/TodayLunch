using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LunchLibrary.Models
{
    public class ModelAction : ModelActionGuide
    {
        private static readonly Lazy<ModelAction> Lazy = new Lazy<ModelAction>(() => new ModelAction());
        public static ModelAction Instance
        {
            get
            {
                return Lazy.Value;
            }
        }
        public ModelAction()
        {
        }

        public override bool Delete<T>(T input)
        {
            return SqlLauncher.Delete(input);
        }

        public override T Insert<T>(T input)
        {
            return SqlLauncher.Insert(input);
        }

        public override T Update<T>(T input)
        {
            return SqlLauncher.Update(input);
        }

        public override List<T> GetAll<T>(Expression<Func<T, bool>> expression = null)
        {
            return LunchLibrary.SqlLauncher.GetAll<T>(expression);
        }

        public override T Get<T>(Expression<Func<T, bool>> expression = null)
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
