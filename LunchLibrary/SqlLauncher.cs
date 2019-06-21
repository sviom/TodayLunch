using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LunchLibrary
{
    public static class SqlLauncher
    {
        /// <summary>
        /// Object Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        public static T Insert<T>(T inputObject) where T : class
        {
            T addedObject = null;
            using (var db = new TodayLunchContext())
            {
                db.Entry(inputObject).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                var set = db.Set<T>();
                set.Add(inputObject);
                int saveResult = db.SaveChanges();
                if (saveResult > 0)
                    addedObject = inputObject;
            }
            return addedObject;
        }

        public static List<T> InsertList<T>(List<T> inputList) where T : class, ICommon
        {
            List<T> resultList = new List<T>();
            using (var db = new TodayLunchContext())
            {
                foreach (var item in inputList)
                {
                    var set = db.Set<T>();
                    set.Add(item);
                }

                int saveResult = db.SaveChanges();
                if (saveResult > 0)
                    resultList = inputList;
            }

            return resultList;
        }

        /// <summary>
        /// 전체 목록 얻어오기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAll<T>(Expression<Func<T, bool>> expression = null) where T : class, ICommon
        {
            List<T> returnList = new List<T>();
            try
            {
                using (var db = new TodayLunchContext())
                {
                    var dbSet = db.Set<T>();
                    if (expression != null)
                    {
                        returnList = dbSet.Where(expression).ToList();
                    }
                    else
                    {
                        returnList = dbSet.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Insert(new Log() { Message = ex.Message, StackTrace = ex.StackTrace });
            }
            return returnList;
        }

        public static bool Get<T>(ref T inputObject, Expression<Func<T, bool>> expression = null) where T : class, ICommon
        {
            try
            {
                using (var db = new TodayLunchContext())
                {
                    var dbSet = db.Set<T>();
                    if (expression != null)
                    {
                        inputObject = dbSet.Where(expression).FirstOrDefault();
                    }
                    else
                    {
                        inputObject = dbSet.FirstOrDefault();
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                Insert(new Log() { Message = ex.Message, StackTrace = ex.StackTrace });
            }
            return false;
        }

        /// <summary>
        /// Object 개수 Counting, 조건 없으면 전체 개수 가져오기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int Count<T>(Func<T, bool> func = null) where T : class
        {
            int count = 0;
            try
            {
                using (var db = new TodayLunchContext())
                {
                    var dbSet = db.Set<T>();
                    if (func != null)
                        count = dbSet.Where(func).ToList().Count;
                    else
                        count = dbSet.ToList().Count;
                }
            }
            catch(Exception ex)
            {
                Insert(new Log() { Message = ex.Message, StackTrace = ex.StackTrace });
            }
            return count;
        }
         
        public static bool Delete<T>(T deleteObject) where T : class, ICommon
        {
            bool result = false;
            try
            {
                if (deleteObject == null || deleteObject.Id == Guid.Empty)
                    return false;

                using (var db = new TodayLunchContext())
                {
                    db.Entry(deleteObject).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    var set = db.Set<T>();
                    set.Remove(deleteObject);
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

        public static T Update<T>(T updateObject) where T : class, ICommon
        {
            T addedObject = null;
            try
            {
                using (var db = new TodayLunchContext())
                {
                    db.Entry(updateObject).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    var set = db.Set<T>();
                    set.Update(updateObject);
                    int saveResult = db.SaveChanges();
                    if (saveResult > 0)
                        addedObject = updateObject;
                }
            }
            catch
            {

            }

            return addedObject;
        }
    }
}
