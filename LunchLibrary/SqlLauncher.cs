using System;
using System.Collections.Generic;
using System.Text;

namespace LunchLibrary
{
    public static class SqlLauncher
    {
        public static bool Insert<T>(T inputObject) where T : class
        {
            bool result = false;
            try
            {
                using (var db = new TodayLunchContext())
                {
                    db.Entry(inputObject).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    var set = db.Set<T>();
                    set.Add(inputObject);
                    int saveResult = db.SaveChanges();
                    if (saveResult > 0)
                        result = true;
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
